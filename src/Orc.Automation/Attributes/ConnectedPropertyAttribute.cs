namespace Orc.Automation.Attributes
{
    using System.Collections.Generic;
    using System.Reflection;

    public class ConnectedPropertyAttribute : AutomationAttribute
    {
        public string ConnectedWith { get; set; }

        //public static IList<(PropertyInfo TargetProperty, PropertyInfo ViewProperty)> GetConnectedProperties(object target, object view)
        //{
        //    var targetType = target.GetType();
        //    var targetViewType = view.GetType();

        //    var targetConnectedProperties = targetType.GetPropertiesDecoratedWith<ConnectedPropertyAttribute>();
        //    var targetViewConnectedProperties = targetViewType.GetPropertiesDecoratedWith<ConnectedPropertyAttribute>();


        //    var alreadyAddedTargetProperties
        //    foreach (var connectedPropertyAttribute in targetConnectedProperties)
        //    {
        //        var propertyInfo = connectedPropertyAttribute.Key;
        //        var attribute = connectedPropertyAttribute.Value;

        //        var targetPropertyName = propertyInfo.Name;
        //        var viewPropertyName = attribute.ConnectedWith;


        //    }
        //}
    }
}
