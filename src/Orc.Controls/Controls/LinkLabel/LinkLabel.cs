// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinkLabel.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------



#if NET || NETCORE

namespace Orc.Controls
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Automation.Peers;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Navigation;
    using Automation;
    using Catel.Logging;



    /// <summary>
    /// A label looking like the known hyperlink.
    /// </summary>
    [TemplatePart(Name = "PART_InnerHyperlink", Type = typeof(Hyperlink))]
    [StyleTypedProperty(Property = nameof(HyperlinkStyle), StyleTargetType = typeof(Hyperlink))]
    public class LinkLabel : Label
    {
        #region Fields
        /// <summary>
        /// The <see cref="ILog">log</see> object.
        /// </summary>
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes the <see cref="LinkLabel"/> class.
        /// </summary>
        static LinkLabel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LinkLabel), new FrameworkPropertyMetadata(typeof(LinkLabel)));

            ClickEvent = EventManager.RegisterRoutedEvent(nameof(Click), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(LinkLabel));
            RequestNavigateEvent = EventManager.RegisterRoutedEvent(nameof(RequestNavigate), RoutingStrategy.Bubble, typeof(RequestNavigateEventHandler), typeof(LinkLabel));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkLabel"/> class.
        /// </summary>
        public LinkLabel()
        {
            Unloaded += OnLinkLabelUnloaded;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        [Category("Common Properties"), Bindable(true)]
        public Uri Url
        {
            get { return GetValue(UrlProperty) as Uri; }
            set { SetValue(UrlProperty, value); }
        }

        /// <summary>
        /// DependencyProperty definition as the backing store for Url
        /// </summary>
        public static readonly DependencyProperty UrlProperty =
            DependencyProperty.Register(nameof(Url), typeof(Uri), typeof(LinkLabel), new UIPropertyMetadata(OnUrlChanged));

        /// <summary>
        /// Indicates whether url has a value.
        /// </summary>
        /// <value><c>true</c> if this instance has URL; otherwise, <c>false</c>.</value>
        public bool HasUrl
        {
            get { return (bool)GetValue(HasUrlProperty); }
            private set { SetValue(HasUrlProperty, value); }
        }

        /// <summary>
        /// DependencyProperty definition as the backing store for HasUrl
        /// </summary>
        public static readonly DependencyProperty HasUrlProperty = DependencyProperty.Register(nameof(HasUrl),
            typeof(bool), typeof(LinkLabel), new UIPropertyMetadata(false));

        /// <summary>
        /// Gets or sets the hyperlink style.
        /// </summary>
        /// <value>The hyperlink style.</value>
        public Style HyperlinkStyle
        {
            get { return GetValue(HyperlinkStyleProperty) as Style; }
            set { SetValue(HyperlinkStyleProperty, value); }
        }

        /// <summary>
        /// DependencyProperty definition as the backing store for HyperlinkStyle
        /// </summary>
        public static readonly DependencyProperty HyperlinkStyleProperty =
            DependencyProperty.Register(nameof(HyperlinkStyle), typeof(Style), typeof(LinkLabel));

        /// <summary>
        /// Gets or sets the hover foreground.
        /// </summary>
        /// <value>The hover foreground.</value>
        [Category("Brushes"), Bindable(true)]
        public Brush HoverForeground
        {
            get { return GetValue(HoverForegroundProperty) as Brush; }
            set { SetValue(HoverForegroundProperty, value); }
        }

        /// <summary>
        /// DependencyProperty definition as the backing store for HoverForeground
        /// </summary>
        public static readonly DependencyProperty HoverForegroundProperty =
            DependencyProperty.Register(nameof(HoverForeground), typeof(Brush), typeof(LinkLabel));

        /// <summary>
        /// Gets or sets the link label behavior.
        /// </summary>
        /// <value>The link label behavior.</value>
        [Category("Common Properties"), Bindable(true)]
        public LinkLabelBehavior LinkLabelBehavior
        {
            get { return (LinkLabelBehavior)GetValue(LinkLabelBehaviorProperty); }
            set { SetValue(LinkLabelBehaviorProperty, value); }
        }

        /// <summary>
        /// DependencyProperty definition as the backing store for LinkLabelBehavior
        /// </summary>
        public static readonly DependencyProperty LinkLabelBehaviorProperty =
            DependencyProperty.Register(nameof(LinkLabelBehavior), typeof(LinkLabelBehavior), typeof(LinkLabel));

        /// <summary>
        /// Wrapper for the ClickBehavior dependency property.
        /// </summary>
        /// <value>The click behavior.</value>
        [Category("Common Properties"), Bindable(true)]
        public LinkLabelClickBehavior ClickBehavior
        {
            get { return (LinkLabelClickBehavior)GetValue(ClickBehaviorProperty); }
            set { SetValue(ClickBehaviorProperty, value); }
        }

        /// <summary>
        /// DependencyProperty definition as the backing store for ClickBehavior
        /// </summary>
        public static readonly DependencyProperty ClickBehaviorProperty =
            DependencyProperty.Register(nameof(ClickBehavior),
                typeof(LinkLabelClickBehavior), typeof(LinkLabel), new UIPropertyMetadata(LinkLabelClickBehavior.Undefined, OnClickBehaviorChanged));

        /// <summary>
        /// Gets or sets the command parameter.
        /// </summary>
        /// <value>The command parameter.</value>
        [Localizability(LocalizationCategory.NeverLocalize), Bindable(true), Category("Action")]
        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        /// <summary>
        /// DependencyProperty definition as the backing store for CommandParameter
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register(nameof(CommandParameter), typeof(object), typeof(LinkLabel));

        /// <summary>
        /// Gets or sets the command.
        /// </summary>
        /// <value>The command.</value>
        [Localizability(LocalizationCategory.NeverLocalize), Bindable(true), Category("Action")]
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        /// <summary>
        /// DependencyProperty definition as the backing store for Command
        /// </summary>
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(LinkLabel));

        /// <summary>
        /// Gets or sets the command target.
        /// </summary>
        /// <value>The command target.</value>
        [Bindable(true), Category("Action")]
        public IInputElement CommandTarget
        {
            get { return (IInputElement)GetValue(CommandTargetProperty); }
            set { SetValue(CommandTargetProperty, value); }
        }

        /// <summary>
        /// DependencyProperty definition as the backing store for CommandTarget
        /// </summary>
        public static readonly DependencyProperty CommandTargetProperty =
            DependencyProperty.Register(nameof(CommandTarget), typeof(IInputElement), typeof(LinkLabel));
        #endregion

        #region Events
        /// <summary>
        /// ClickEvent
        /// </summary>
        [Category("Behavior")] public static readonly RoutedEvent ClickEvent;

        /// <summary>
        /// Occurs when [click].
        /// </summary>
        public event RoutedEventHandler Click
        {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }

        /// <summary>
        /// RequestNavigateEvent
        /// </summary>
        [Category("Behavior")] public static readonly RoutedEvent RequestNavigateEvent;

        /// <summary>
        /// Occurs when [request navigate].
        /// </summary>
        public event RequestNavigateEventHandler RequestNavigate
        {
            add { AddHandler(RequestNavigateEvent, value); }
            remove { RemoveHandler(RequestNavigateEvent, value); }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Handles the Unloaded event of the LinkLabel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void OnLinkLabelUnloaded(object sender, RoutedEventArgs e)
        {
            // Clear events
            Click -= OpenBrowserBehaviorImpl;
        }

        /// <summary>
        /// Handles change of property Url.
        /// </summary>
        /// <param name="sender">A sender.</param>
        /// <param name="args">Event args.</param>
        private static void OnUrlChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            if (sender is LinkLabel typedSender)
            {
                var url = args.NewValue as Uri;

                typedSender.SetCurrentValue(HasUrlProperty, url is not null && !string.IsNullOrEmpty(url.OriginalString));
                typedSender.SetCurrentValue(IsEnabledProperty, typedSender.HasUrl);
            }
        }

        /// <summary>
        /// Handles a change of the ClickBehavior property.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="args">The event arguments.</param>
        private static void OnClickBehaviorChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            if (sender is not LinkLabel label)
            {
                return;
            }

            if (!args.Property.Name.Equals(ClickBehaviorProperty.Name, StringComparison.Ordinal))
            {
                return;
            }

            var previous = (LinkLabelClickBehavior)args.OldValue;
            var next = (LinkLabelClickBehavior)args.NewValue;

            if (previous == LinkLabelClickBehavior.OpenUrlInBrowser)
            {
                label.Click -= OpenBrowserBehaviorImpl;
            }

            if (next == LinkLabelClickBehavior.OpenUrlInBrowser)
            {
                label.Click += OpenBrowserBehaviorImpl;
            }
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate"/>.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (GetTemplateChild("PART_InnerHyperlink") is Hyperlink innerHyperlink)
            {
                innerHyperlink.Click += OnInnerHyperlinkClick;
                innerHyperlink.RequestNavigate += OnInnerHyperlinkRequestNavigate;
            }
        }

        /// <summary>
        /// Handles the RequestNavigate event of the InnerHyperlink control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Navigation.RequestNavigateEventArgs"/> instance containing the event data.</param>
        private void OnInnerHyperlinkRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            var args = new RequestNavigateEventArgs(e.Uri, string.Empty)
            {
                Source = this,
                RoutedEvent = RequestNavigateEvent
            };
            RaiseEvent(args);
        }

        /// <summary>
        /// Handles the Click event of the InnerHyperlink control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void OnInnerHyperlinkClick(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(ClickEvent, this));
        }

        /// <summary>
        /// Handles the click event of the default linklabel.
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="args">Event arguments</param>
        private static void OpenBrowserBehaviorImpl(object sender, RoutedEventArgs args)
        {
            var hyperlinkSender = sender as Hyperlink;
            var linklabelSender = sender as LinkLabel;
            if (hyperlinkSender is null && linklabelSender is null)
            {
                return;
            }

            var destinationUrl = hyperlinkSender is not null ? hyperlinkSender.NavigateUri : linklabelSender.Url;
            if (destinationUrl is null || string.IsNullOrEmpty(destinationUrl.ToString()))
            {
                return;
            }

            try
            {
                Mouse.OverrideCursor = Cursors.AppStarting;

                try
                {
#if NETCORE
                    // UseShellExecute is disabled by default in NETCORE
                    var processStartInfo = new ProcessStartInfo
                    {
                        FileName = destinationUrl.ToString(),
                        UseShellExecute = true
                    };

                    Process.Start(processStartInfo);
#else
                   //[SL:20090827] Changed hardcoded call to IE and let the OS determine what program to use.
                    Process.Start(destinationUrl.ToString()); 
#endif
                }
                catch (Win32Exception ex)
                {
                    Log.Warning(ex, "Default handler for http-scheme not valid in Windows");

                    var processStartInfo = new ProcessStartInfo(@"iexplore.exe", destinationUrl.ToString())
                    {
                        UseShellExecute = false
                    };
                    Process.Start(processStartInfo);
                }
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Failed to start process to open '{0}'", destinationUrl);
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }
        #endregion

        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new LinkLabelAutomationPeer(this);
        }
    }
}

#endif
