namespace Orc.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Threading;
    using Catel;
    using Catel.Collections;
    using Catel.Logging;
    using Catel.MVVM;

    public class CalloutManager : ICalloutManager
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly List<ICallout> _callouts;

        public CalloutManager()
        {
            _callouts = new List<ICallout>();
        }

        public List<ICallout> Callouts
        {
            get
            {
                var calloutsCopy = _callouts.ToList();
                return calloutsCopy;
            }
        }

        public event EventHandler<CalloutEventArgs> Registered;
        public event EventHandler<CalloutEventArgs> Unregistered;

        public event EventHandler<CalloutEventArgs> Showing;
        public event EventHandler<CalloutEventArgs> Hiding;

        public void Register(ICallout callout)
        {
            Argument.IsNotNull(() => callout);

            Log.Debug($"Registering callout '{callout}'");

            _callouts.Add(callout);

            SubscribeToCallout(callout);

            Registered?.Invoke(this, new CalloutEventArgs(callout));
        }

        public void Unregister(ICallout callout)
        {
            Argument.IsNotNull(() => callout);

            Log.Debug($"Unregistering callout '{callout}'");

            UnsubscribeFromCallout(callout);

            // Make sure to hide
            callout.Hide();

            _callouts.Remove(callout);

            Unregistered?.Invoke(this, new CalloutEventArgs(callout));
        }

        public void Clear()
        {
            _callouts.ForEach(x => UnsubscribeFromCallout(x));
            _callouts.Clear();
        }

        //public void ShowAllCallouts()
        //{
        //    foreach (var callout in Callouts)
        //    {
        //        if (callout is Controls.CalloutViewModel vm)
        //        {
        //            if (vm.Delay is not null && vm.Delay > 0)
        //            {
        //                var timer = new DispatcherTimer();
        //                timer.Interval = TimeSpan.FromSeconds((int)vm.Delay);
        //                timer.Tick += (object o, EventArgs e) =>
        //                {
        //                    vm.IsOpen = true;
        //                    timer.Stop();
        //                };
        //                timer.Start();
        //            }
        //            else
        //            {
        //                vm.IsOpen = true;
        //            }
        //        }
        //    }
        //}

        //public void ShowCallout(UIElement element)
        //{
        //    foreach (var callout in Callouts)
        //    {
        //        if (callout is CalloutViewModel vm)
        //        {
        //            if (vm.PlacementTarget == element)
        //            {
        //                vm.IsOpen = true;
        //            }
        //        }
        //    }
        //}

        private void SubscribeToCallout(ICallout callout)
        {
            callout.Showing += OnCalloutShowing;
            callout.Hiding += OnCalloutHiding;
        }

        private void UnsubscribeFromCallout(ICallout callout)
        {
            callout.Showing -= OnCalloutShowing;
            callout.Hiding -= OnCalloutHiding;
        }

        private void OnCalloutShowing(object sender, CalloutEventArgs e)
        {
            Showing?.Invoke(this, e);
        }

        private void OnCalloutHiding(object sender, CalloutEventArgs e)
        {
            Hiding?.Invoke(this, e);
        }
    }
}
