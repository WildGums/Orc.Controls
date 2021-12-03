namespace Orc.Automation
{
    using System;
    using System.Linq;

    public static class TypeHelper
    {
        public static Type GetTypeByName(string fullName)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .FirstOrDefault(x => string.Equals(x.FullName, fullName));
        }
    }
}
