namespace Orc.Controls
{
    using Catel;

    public static class ICalloutExtensions
    {
        public static string GetCalloutConfigurationKeyPrefix(this ICallout callout)
        {
            Argument.IsNotNull(() => callout);

            return GetCalloutConfigurationKeyPrefix(callout.Name, callout.Version);
        }

        public static string GetCalloutConfigurationKeyPrefix(string name, string version)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                name = "Unnamed";
            }

            if (string.IsNullOrWhiteSpace(version))
            {
                version = "Default";
            }

            var value = $"Callouts.{name}.{version}";
            return value;
        }
    }
}
