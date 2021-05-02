namespace Orc.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Catel;
    using Catel.Configuration;

    public abstract class CalloutWatcherBase
    {
        protected readonly ICalloutManager _calloutManager;
        protected readonly IConfigurationService _configurationService;

        public CalloutWatcherBase(ICalloutManager calloutManager, IConfigurationService configurationService)
        {
            Argument.IsNotNull(() => calloutManager);
            Argument.IsNotNull(() => configurationService);

            _calloutManager = calloutManager;
            _configurationService = configurationService;

            IsOneTimeCallout = true;
        }

        public Guid? Id { get; protected set; }

        public string Name { get; protected set; }

        public virtual string ConfigurationPrefix
        {
            get
            {
                return $"Callouts.{Id?.ToString() ?? Name}.{Version}";
            }
        }

        public virtual string LastShownConfigurationName
        {
            get
            {
                return $"{ConfigurationPrefix}.LastShown";
            }
        }

        public virtual string Version => "1.0.0";

        public bool HasShown { get; private set; }

        public bool IsOneTimeCallout { get; protected set; }

        public DateTime LastShownUtc
        {
            get { return _configurationService.GetRoamingValue(LastShownConfigurationName, DateTime.MinValue); }
        }

        public TimeSpan ShowInterval { get; protected set; }

        public virtual ICallout Callout
        {
            get
            {
                ICallout callout = null;

                if (Id.HasValue)
                {
                    callout = _calloutManager.FindCallout(Id.Value);
                }

                if (callout is null)
                {
                    callout = _calloutManager.FindCallout(Name);
                }

                return callout;
            }
        }

        protected virtual void Show()
        {
            var callout = Callout;
            if (callout is null)
            {
                return;
            }

            if (IsOneTimeCallout && LastShownUtc != DateTime.MinValue)
            {
                return;
            }

            callout.Show();

            HasShown = true;

            _configurationService.SetRoamingValue(LastShownConfigurationName, DateTime.UtcNow);
        }

        protected virtual void Hide()
        {
            Callout?.Hide();
        }
    }
}
