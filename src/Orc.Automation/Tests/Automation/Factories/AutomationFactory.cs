namespace Orc.Automation
{
    using Catel.IoC;

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
}