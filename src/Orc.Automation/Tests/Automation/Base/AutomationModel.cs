namespace Orc.Automation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Windows.Automation;
    using Automation;
    using Catel;
    using Catel.Caching;
    using Catel.Data;
    using Catel.IoC;
    using Catel.Reflection;

    public abstract class AutomationBase
    {
        private readonly CacheStorage<Type, AutomationBase> _simulations = new();
        private readonly CacheStorage<Type, AutomationBase> _maps = new();

        public AutomationBase(AutomationElement element)
        {
            Argument.IsNotNull(() => element);

            Element = element;

            Factory = new AutomationFactory();
        }

        public AutomationElement Element { get; }
        protected AutomationFactory Factory { get; }
        protected By By => new (Element);

        public T Simulate<T>()
            where T : AutomationBase
        {
            return (T) _simulations.GetFromCacheOrFetch(typeof(T), () => Factory.Create<T>(this));
        }

        public T Map<T>()
            where T : AutomationBase
        {
            return (T)_maps.GetFromCacheOrFetch(typeof(T), () => Factory.Create<T>(this));
        }

        protected virtual void OnEvent(object sender, AutomationEventArgs args)
        {

        }
    }


    public static class ByExtensions
    {
        public static By Id(this By by)
        {
            Argument.IsNotNull(() => by);

            var property = AutomationHelper.GetCallingProperty();
            if (property is null)
            {
                throw new AutomationException("Can't find calling property");
            }

            return by.Id(property);
        }

        public static By Name(this By by)
        {
            Argument.IsNotNull(() => by);

            var property = AutomationHelper.GetCallingProperty();
            if (property is null)
            {
                throw new AutomationException("Can't find calling property");
            }

            return by.Name(property);
        }

        public static T One<T>(this By by)
        {
            Argument.IsNotNull(() => by);

            var result = by.One();
            if (result is null)
            {
                return default;
            }

            return (T) AutomationHelper.WrapAutomationObject(typeof(T), result);
        }

        public static List<T> Many<T>(this By by)
        {
            Argument.IsNotNull(() => by);

            var result = by.Many();
            if (result is null)
            {
                return default;
            }

            return (List<T>)AutomationHelper.WrapAutomationObject(typeof(List<T>), result);
        }
    }


    public class By
    {
        private readonly AutomationElement _element;
        private readonly SearchContext _searchContext = new();

        public By(AutomationElement element)
        {
            Argument.IsNotNull(() => element);

            _element = element;
        }

        public By Id(string id)
        {
            _searchContext.Id = id;

            return this;
        }

        public By Name(string name)
        {
            _searchContext.Name = name;

            return this;
        }

        public By ControlType(ControlType controlType)
        {
            _searchContext.ControlType = controlType;

            return this;
        }

        public By ClassName(string className)
        {
            _searchContext.ClassName = className;

            return this;
        }

        public By Condition(Condition condition)
        {
            _searchContext.Condition = condition;

            return this;
        }

        public AutomationElement One()
        {
            return _element.Find(_searchContext);
        }

        public IList<AutomationElement> Many()
        {
            return _element.FindAll(_searchContext)?.ToList();
        }
    }


    public class AutomationFactory
    {
        public T Create<T>(object element)
            where T : AutomationBase
        {
            var typeFactory = this.GetTypeFactory();

            if (element is AutomationControl control)
            {
                return typeFactory.CreateInstanceWithParametersAndAutoCompletion<T>(control)
                       ?? typeFactory.CreateInstanceWithParametersAndAutoCompletion<T>(control.Element);
            }

            return typeFactory.CreateInstanceWithParametersAndAutoCompletion<T>(element);
        }
    }


    public static class AutomationElementAccessorExtensions
    {
        public static T GetValue<T>(this AutomationElementAccessor automationElementAccessor)
        {
            Argument.IsNotNull(() => automationElementAccessor);

            return (T)automationElementAccessor.GetValue();
        }

        public static T GetValue<T>(this AutomationElementAccessor automationElementAccessor, string propertyName)
        {
            Argument.IsNotNull(() => automationElementAccessor);

            return (T) automationElementAccessor.GetValue(propertyName);
        }

        public static object GetValue(this AutomationElementAccessor automationElementAccessor)
        {
            Argument.IsNotNull(() => automationElementAccessor);

            var propertyName = AutomationHelper.GetCallingProperty();
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new AutomationException("Can't find calling property");
            }

            return automationElementAccessor.GetValue(propertyName);
        }

        public static void SetValue(this AutomationElementAccessor automationElementAccessor, object value)
        {
            Argument.IsNotNull(() => automationElementAccessor);

            var propertyName = AutomationHelper.GetCallingProperty();
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new AutomationException("Can't find calling property");
            }

            automationElementAccessor.SetValue(propertyName, value);
        }

        public static T Execute<T>(this AutomationElementAccessor automationElementAccessor, string methodName, params object[] parameters)
        {
            Argument.IsNotNull(() => automationElementAccessor);

            return (T)automationElementAccessor.Execute(methodName, parameters);
        }
    }
    
    public class AutomationElementAccessor
    {
        private readonly InvokePattern _invokePattern;
        private readonly ValuePattern _valuePattern;

        public AutomationElementAccessor(AutomationElement element)
        {
            Argument.IsNotNull(() => element);

            _valuePattern = element.TryGetPattern<ValuePattern>();
            _invokePattern = element.TryGetPattern<InvokePattern>();

            if (_invokePattern is not null && _valuePattern is not null)
            {
                System.Windows.Automation.Automation.AddAutomationEventHandler(InvokePattern.InvokedEvent, element, TreeScope.Element, OnEventInvoke);
            }
        }

        private void OnEventInvoke(object sender, System.Windows.Automation.AutomationEventArgs e)
        {
            var result = _valuePattern.Current.Value;

            var automationResult = AutomationResultContainer.FromStr(result);

            var eventName = automationResult.LastEventName;
            var eventData = automationResult.LastEventArgs?.ExtractValue();

            AutomationEvent?.Invoke(this, new AutomationEventArgs
            {
                EventName = eventName,
                Data = eventData
            });
        }
        
        public object GetValue(string propertyName)
        {
            Argument.IsNotNull(() => propertyName);
            Argument.IsNotNullOrEmpty(() => propertyName);

            var result = Execute(GetDependencyPropertyMethodRun.ConvertPropertyToCommandName(propertyName), null, 20);
            return result;
        }

        public void SetValue(string propertyName, object value)
        {
            Argument.IsNotNull(() => propertyName);
            Argument.IsNotNullOrEmpty(() => propertyName);

            var automationValues = AutomationValueList.Create(value);
            var result = Execute(SetDependencyPropertyMethodRun.ConvertPropertyToCommandName(propertyName), automationValues, 20);
        }

        public object Execute(string methodName, params object[] parameters)
        {
            var automationValues = AutomationValueList.Create(parameters);
            return Execute(methodName, automationValues, 20);
        }

        private object Execute(string methodName, AutomationValueList parameters, int delay = 200)
        {
            var method = new AutomationMethod
            {
                Name = methodName,
                Parameters = parameters
            };

            var result = Execute(method, delay);
            var resultValue = result?.ExtractValue();

            return resultValue;
        }

        private AutomationValue Execute(AutomationMethod method, int delay)
        {
            var methodStr = method?.ToString();
            if (string.IsNullOrWhiteSpace(methodStr))
            {
                return null;
            }

            _valuePattern.SetValue(methodStr);

            Thread.Sleep(delay);

            try
            {
                _invokePattern?.Invoke();
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }

            Thread.Sleep(delay);

            var result = _valuePattern.Current.Value;
            var automationResultContainer = AutomationResultContainer.FromStr(result);

            return automationResultContainer.LastInvokedMethodResult;
        }

        public event EventHandler<AutomationEventArgs> AutomationEvent;
    }

    public class AutomationControl : AutomationBase
    {
        public AutomationControl(AutomationElement element) 
            : base(element)
        {
        }
    }

    public class CustomAutomationControl : AutomationControl
    {
        protected AutomationElementAccessor Access { get; }

        public CustomAutomationControl(AutomationElement element)
            : base(element)
        {
            Access = new AutomationElementAccessor(element);
            Access.AutomationEvent += OnEvent;
        }
    }



    //public abstract class AutomationModel : ModelBase
    //{
    //    private readonly CacheStorage<string, SearchContext> _cachedPartSearchContexts = new();
    //    private readonly Dictionary<string, object> _cachedProperties = new();

    //    public AutomationModel(AutomationElement target)
    //    {
    //        Argument.IsNotNull(() => target);

    //        Element = target;
    //    }

    //    public AutomationModel(AutomationControl targetControl)
    //    {
    //        Argument.IsNotNull(() => targetControl);

    //        Element = targetControl.Element;
    //    }

    //    public virtual AutomationElement Element { get; }

    //    public By By => new By(Element);

    //    protected override T GetValueFromPropertyBag<T>(string propertyName)
    //    {
    //        if (TryGetControlPart<T>(propertyName, out var controlPart))
    //        {
    //            return controlPart;
    //        }

    //        //if (TryGetControlMap(propertyName, out var controlMap))
    //        //{
    //        //    return controlMap;
    //        //}

    //        //Manage ordinary properties
    //        return base.GetValueFromPropertyBag<T>(propertyName);
    //    }

    //    protected override void SetValueToPropertyBag<TValue>(string propertyName, TValue value)
    //    {
    //        //Manage ordinary properties
    //        base.SetValueToPropertyBag(propertyName, value);
    //    }

    //    //protected virtual bool TryGetControlMap<T>(string propertyName, out T controlMap)
    //    //{
    //    //    controlMap = default;

    //    //    if (_cachedProperties.TryGetValue(propertyName, out var controlMapObj))
    //    //    {
    //    //        controlMap = (T)controlMapObj;

    //    //        return true;
    //    //    }


    //    //}

    //    protected virtual bool TryGetControlPart<T>(string propertyName, out T controlPart)
    //    {
    //        controlPart = default;

    //        if (_cachedProperties.TryGetValue(propertyName, out var controlPartObj))
    //        {
    //            controlPart = (T)controlPartObj;

    //            return true;
    //        }

    //        var searchContext = _cachedPartSearchContexts.GetFromCacheOrFetch(propertyName, () => GetSearchContext(propertyName));
    //        if (searchContext is null)
    //        {
    //            return false;
    //        }

    //        controlPart = GetPart<T>(propertyName, searchContext);
    //        if (searchContext.IsCached)
    //        {
    //            _cachedProperties[propertyName] = controlPart;
    //        }

    //        return true;
    //    }

    //    protected virtual SearchContext GetSearchContext(string propertyName)
    //    {
    //        var type = GetType();

    //        var property = type.GetRuntimeProperties().FirstOrDefault(x => Equals(x.Name, propertyName));
    //        var controlPartAttribute = property?.GetAttribute<ControlPartAttribute>();
    //        if (controlPartAttribute is null)
    //        {
    //            return null;
    //        }

    //        var searchContext = controlPartAttribute.GetSearchContext();
    //        if (searchContext.IsEmpty)
    //        {
    //            searchContext.Id = propertyName;
    //        }

    //        return searchContext;
    //    }

    //    protected virtual TAutomationElement GetPart<TAutomationElement>(string propertyName, SearchContext searchContext)
    //    {
    //        return Element.FindOneOrMany<TAutomationElement>(searchContext);
    //    }

    //    public static implicit operator AutomationElement(AutomationModel model)
    //    {
    //        return model?.Element;
    //    }
    //}
}
