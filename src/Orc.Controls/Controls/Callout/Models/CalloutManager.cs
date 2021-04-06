namespace Orc.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Threading;
    using Catel;
    using Catel.Collections;
    using Catel.MVVM;

    public class CalloutManager : ICalloutManager
    {
        public CalloutManager()
        {
            _callouts = new List<IViewModel>();
        }

        private IList<IViewModel> _callouts;

        public IList<IViewModel> Callouts
        {
            get
            {
                IList<IViewModel> calloutsCopy = _callouts.ToList();

                return calloutsCopy;
            }
            set
            {
                _callouts = value;
            }
        }

        public void Register(IViewModel callout)
        {
            Argument.IsNotNull(() => callout); // a callout null-ot dob, a calloutusercontroloknak null a viewmodellje.

            _callouts.Add(callout);
        }

        public void Unregister(IViewModel callout)
        {
            _callouts.Remove(callout);
        }

        public void RemoveAllCallouts()
        {
            _callouts.Clear();
        }

        public void ShowAllCallouts()
        {
            foreach (var callout in Callouts)
            {
                if (callout is Controls.CalloutViewModel vm)
                {
                    if (vm.Delay is not null && vm.Delay > 0)
                    {
                        DispatcherTimer timer = new DispatcherTimer();
                        timer.Interval = TimeSpan.FromSeconds((int)vm.Delay);
                        timer.Tick += (object o, EventArgs e) =>
                        {
                            vm.IsOpen = true;
                            timer.Stop();
                        };
                        timer.Start();
                    }
                    else
                    {
                        vm.IsOpen = true;
                    }
                }
            }

        }

        public void ShowCallout(UIElement element)
        {
            foreach (var callout in Callouts)
            {
                if (callout is CalloutViewModel vm)
                {
                    if (vm.PlacementTarget == element)
                    {
                        vm.IsOpen = true;
                    }
                }
            }
        }
    }
}
