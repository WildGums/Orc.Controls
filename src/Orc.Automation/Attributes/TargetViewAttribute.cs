namespace Orc.Automation
{
    using System.Linq;
    using System.Reflection;

    public class TargetViewAttribute : AutomationAttribute
    {
        public static object GetTargetView(object source)
        {
            var targetControlProperty = GetTargetViewProperty(source);
            return targetControlProperty?.GetValue(source);
        }

        public static PropertyInfo GetTargetViewProperty(object source)
        {
            return source.GetType().GetProperties().FirstOrDefault(prop => IsDefined(prop, typeof(TargetAttribute)));
        }
    }
}
