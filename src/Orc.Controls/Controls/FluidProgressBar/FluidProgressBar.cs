// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FluidProgressBar1.cs" company="WildGums">
//   Copyright (c) 2008 - 2020 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Shapes;
    using Catel.Logging;

    [TemplatePart(Name = "PART_Dot1", Type = typeof(Rectangle))]
    [TemplatePart(Name = "PART_Dot2", Type = typeof(Rectangle))]
    [TemplatePart(Name = "PART_Dot3", Type = typeof(Rectangle))]
    [TemplatePart(Name = "PART_Dot4", Type = typeof(Rectangle))]
    [TemplatePart(Name = "PART_Dot5", Type = typeof(Rectangle))]
    [TemplatePart(Name = "PART_Canvas", Type = typeof(Canvas))]
    public class FluidProgressBar : Control, IDisposable
    {
        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly Dictionary<int, KeyFrameDetails> _keyFrameMap;
        private readonly Dictionary<int, KeyFrameDetails> _opKeyFrameMap;
        private bool _isStoryboardRunning;

        private Storyboard _sb;

        private Canvas _canvas;
        private Rectangle _dot1;
        private Rectangle _dot2;
        private Rectangle _dot3;
        private Rectangle _dot4;
        private Rectangle _dot5;
        #endregion

        #region Constructors
        public FluidProgressBar()
        {
            _keyFrameMap = new Dictionary<int, KeyFrameDetails>();
            _opKeyFrameMap = new Dictionary<int, KeyFrameDetails>();

            SizeChanged += OnSizeChanged;
            Loaded += OnLoaded;
            IsVisibleChanged += OnIsVisibleChanged;
        }
        #endregion

        #region Method
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _canvas = GetTemplateChild("PART_Canvas") as Canvas;
            if (_canvas is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_Canvas'");
            }
            

            _dot1 = GetTemplateChild("PART_Dot1") as Rectangle;
            if (_dot1 is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_Dot1'");
            }

            _dot2 = GetTemplateChild("PART_Dot2") as Rectangle;
            if (_dot2 is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_Dot2'");
            }

            _dot3 = GetTemplateChild("PART_Dot3") as Rectangle;
            if (_dot3 is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_Dot3'");
            }

            _dot4 = GetTemplateChild("PART_Dot4") as Rectangle;
            if (_dot4 is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_Dot4'");
            }

            _dot5 = GetTemplateChild("PART_Dot5") as Rectangle;
            if (_dot5 is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_Dot5'");
            }

            _sb = CreateStoryBoard();
        }
        #endregion

        private Storyboard CreateStoryBoard()
        {
            var storyboard = new Storyboard
            {
                RepeatBehavior = RepeatBehavior.Forever,
                AutoReverse = false,
                Duration = new Duration(TimeSpan.FromMilliseconds(4400))
            };

            var xFrame1 = CreateXFrames(0d, _dot1, 0d, 0d, 0d, 0d);
            var xFrame2 = CreateXFrames(100d, _dot2, 0.1d, 3.1d, 5.1d, 8.1d);
            var xFrame3 = CreateXFrames(200d, _dot3, 0.1d, 3.1d, 5.1d, 8.1d);
            var xFrame4 = CreateXFrames(300d, _dot4, 0.1d, 3.1d, 5.1d, 8.1d);
            var xFrame5 = CreateXFrames(400d, _dot5, 0.1d, 3.1d, 5.1d, 8.1d);

            var opacityFrame1 = CreateOpacityFrames(0d, _dot1);
            var opacityFrame2 = CreateOpacityFrames(100d, _dot2);
            var opacityFrame3 = CreateOpacityFrames(200d, _dot3);
            var opacityFrame4 = CreateOpacityFrames(300d, _dot4);
            var opacityFrame5 = CreateOpacityFrames(400d, _dot5);

            var children = storyboard.Children;

            children.Add(xFrame1);
            children.Add(xFrame2);
            children.Add(xFrame3);
            children.Add(xFrame4);
            children.Add(xFrame5);

            children.Add(opacityFrame1);
            children.Add(opacityFrame2);
            children.Add(opacityFrame3);
            children.Add(opacityFrame4);
            children.Add(opacityFrame5);

            return storyboard;
        }

        private DoubleAnimationUsingKeyFrames CreateOpacityFrames(double beginTime, object target)
        {
            var frames = new DoubleAnimationUsingKeyFrames
            {
                BeginTime = TimeSpan.FromMilliseconds(beginTime)
            };

            frames.SetCurrentValue(Storyboard.TargetPropertyProperty, new PropertyPath(nameof(Opacity)));
            frames.SetCurrentValue(Storyboard.TargetProperty, target);

            StoreOpacityFrame(0, new DiscreteDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0)), 
                Value = 1d
            }, frames);

            StoreOpacityFrame(1, new DiscreteDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(2500)), 
                Value = 0d
            }, frames);

            return frames;
        }

        private DoubleAnimationUsingKeyFrames CreateXFrames(double beginTime, object target, params double[] values)
        {
            var frames = new DoubleAnimationUsingKeyFrames
            {
                BeginTime = TimeSpan.FromMilliseconds(beginTime)
            };
            
            frames.SetCurrentValue(Storyboard.TargetPropertyProperty, new PropertyPath("(Canvas.Left)"));
            frames.SetCurrentValue(Storyboard.TargetProperty, target);

            var progressBarEaseOut = new ExponentialEase
            {
                EasingMode = EasingMode.EaseOut,
                Exponent = 2d
            };

            var progressBarEaseIn = new ExponentialEase
            {
                EasingMode = EasingMode.EaseIn,
                Exponent = 2d
            };
            
            StoreKeyFrame(0, new LinearDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0)), 
                Value = values[0]
            }, frames);

            StoreKeyFrame(1, new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(500)),
                Value = values[1],
                EasingFunction = progressBarEaseOut
            }, frames);

            StoreKeyFrame(2, new LinearDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(2000)),
                Value = values[2]
            }, frames);

            StoreKeyFrame(3, new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(2500)),
                Value = values[3], 
                EasingFunction = progressBarEaseIn
            }, frames);

            return frames;
        }

        private void StoreKeyFrame(int index, DoubleKeyFrame doubleKeyFrame, DoubleAnimationUsingKeyFrames keyFrames)
        {
            if (!_keyFrameMap.TryGetValue(index, out var keyFrame))
            {
                keyFrame = new KeyFrameDetails();
                _keyFrameMap[index] = keyFrame;
            }

            keyFrame.KeyFrameTime = doubleKeyFrame.KeyTime;
            keyFrame.KeyFrames.Add(doubleKeyFrame);

            keyFrames.KeyFrames.Add(doubleKeyFrame);
        }

        private void StoreOpacityFrame(int index, DoubleKeyFrame doubleOpacityFrame, DoubleAnimationUsingKeyFrames keyFrames)
        {
            if (!_opKeyFrameMap.TryGetValue(index, out var opFrame))
            {
                opFrame = new KeyFrameDetails();
                _opKeyFrameMap[index] = opFrame;
            }

            opFrame.KeyFrameTime = doubleOpacityFrame.KeyTime;
            opFrame.KeyFrames.Add(doubleOpacityFrame);

            keyFrames.KeyFrames.Add(doubleOpacityFrame);
        }

        #region Internal class
        private class KeyFrameDetails
        {
            public KeyFrameDetails()
            {
                KeyFrames = new List<DoubleKeyFrame>();
            }

            #region Properties
            public KeyTime KeyFrameTime { get; set; }
            public List<DoubleKeyFrame> KeyFrames { get; set; }
            #endregion
        }
        #endregion

        #region Delay
        /// <summary>
        /// Delay Dependency Property
        /// </summary>
        public static readonly DependencyProperty DelayProperty =
            DependencyProperty.Register(nameof(Delay), typeof(Duration), typeof(FluidProgressBar),
                new FrameworkPropertyMetadata(new Duration(TimeSpan.FromMilliseconds(100)), OnDelayChanged));

        /// <summary>
        /// Gets or sets the Delay property. This dependency property 
        /// indicates the delay between adjacent animation timeLines.
        /// </summary>
        public Duration Delay
        {
            get { return (Duration)GetValue(DelayProperty); }
            set { SetValue(DelayProperty, value); }
        }

        /// <summary>
        /// Handles changes to the Delay property.
        /// </summary>
        /// <param name="d">FluidProgressBar</param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnDelayChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pBar = (FluidProgressBar)d;
            var oldDelay = (Duration)e.OldValue;
            var newDelay = pBar.Delay;
            pBar.OnDelayChanged(oldDelay, newDelay);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the Delay property.
        /// </summary>
        /// <param name="oldDelay">Old Value</param>
        /// <param name="newDelay">New Value</param>
        protected virtual void OnDelayChanged(Duration oldDelay, Duration newDelay)
        {
            var isActive = _isStoryboardRunning;
            if (isActive)
            {
                StopFluidAnimation();
            }

            UpdateTimelineDelay(newDelay);

            if (isActive)
            {
                StartFluidAnimation();
            }
        }
        #endregion

        #region DotWidth
        /// <summary>
        /// DotWidth Dependency Property
        /// </summary>
        public static readonly DependencyProperty DotWidthProperty =
            DependencyProperty.Register(nameof(DotWidth), typeof(double), typeof(FluidProgressBar),
                new FrameworkPropertyMetadata(4.0, OnDotWidthChanged));

        /// <summary>
        /// Gets or sets the DotWidth property. This dependency property 
        /// indicates the width of each of the dots.
        /// </summary>
        public double DotWidth
        {
            get { return (double)GetValue(DotWidthProperty); }
            set { SetValue(DotWidthProperty, value); }
        }

        /// <summary>
        /// Handles changes to the DotWidth property.
        /// </summary>
        /// <param name="d">FluidProgressBar</param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnDotWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pBar = (FluidProgressBar)d;
            var oldDotWidth = (double)e.OldValue;
            var newDotWidth = pBar.DotWidth;
            pBar.OnDotWidthChanged(oldDotWidth, newDotWidth);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the DotWidth property.
        /// </summary>
        /// <param name="oldDotWidth">Old Value</param>
        /// <param name="newDotWidth">New Value</param>
        protected virtual void OnDotWidthChanged(double oldDotWidth, double newDotWidth)
        {
            if (_isStoryboardRunning)
            {
                RestartStoryboardAnimation();
            }
        }
        #endregion

        #region DotHeight
        /// <summary>
        /// DotHeight Dependency Property
        /// </summary>
        public static readonly DependencyProperty DotHeightProperty =
            DependencyProperty.Register(nameof(DotHeight), typeof(double), typeof(FluidProgressBar),
                new FrameworkPropertyMetadata(4.0, OnDotHeightChanged));

        /// <summary>
        /// Gets or sets the DotHeight property. This dependency property 
        /// indicates the height of each of the dots.
        /// </summary>
        public double DotHeight
        {
            get { return (double)GetValue(DotHeightProperty); }
            set { SetValue(DotHeightProperty, value); }
        }

        /// <summary>
        /// Handles changes to the DotHeight property.
        /// </summary>
        /// <param name="d">FluidProgressBar</param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnDotHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pBar = (FluidProgressBar)d;
            var oldDotHeight = (double)e.OldValue;
            var newDotHeight = pBar.DotHeight;
            pBar.OnDotHeightChanged(oldDotHeight, newDotHeight);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the DotHeight property.
        /// </summary>
        /// <param name="oldDotHeight">Old Value</param>
        /// <param name="newDotHeight">New Value</param>
        protected virtual void OnDotHeightChanged(double oldDotHeight, double newDotHeight)
        {
            if (_isStoryboardRunning)
            {
                RestartStoryboardAnimation();
            }
        }
        #endregion

        #region DotRadiusX
        /// <summary>
        /// DotRadiusX Dependency Property
        /// </summary>
        public static readonly DependencyProperty DotRadiusXProperty =
            DependencyProperty.Register(nameof(DotRadiusX), typeof(double), typeof(FluidProgressBar),
                new FrameworkPropertyMetadata(0.0, OnDotRadiusXChanged));

        /// <summary>
        /// Gets or sets the DotRadiusX property. This dependency property 
        /// indicates the corner radius width of each of the dot.
        /// </summary>
        public double DotRadiusX
        {
            get { return (double)GetValue(DotRadiusXProperty); }
            set { SetValue(DotRadiusXProperty, value); }
        }

        /// <summary>
        /// Handles changes to the DotRadiusX property.
        /// </summary>
        /// <param name="d">FluidProgressBar</param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnDotRadiusXChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pBar = (FluidProgressBar)d;
            var oldDotRadiusX = (double)e.OldValue;
            var newDotRadiusX = pBar.DotRadiusX;
            pBar.OnDotRadiusXChanged(oldDotRadiusX, newDotRadiusX);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the DotRadiusX property.
        /// </summary>
        /// <param name="oldDotRadiusX">Old Value</param>
        /// <param name="newDotRadiusX">New Value</param>
        protected virtual void OnDotRadiusXChanged(double oldDotRadiusX, double newDotRadiusX)
        {
            if (_isStoryboardRunning)
            {
                RestartStoryboardAnimation();
            }
        }
        #endregion

        #region DotRadiusY
        /// <summary>
        /// DotRadiusY Dependency Property
        /// </summary>
        public static readonly DependencyProperty DotRadiusYProperty =
            DependencyProperty.Register(nameof(DotRadiusY), typeof(double), typeof(FluidProgressBar),
                new FrameworkPropertyMetadata(0.0, OnDotRadiusYChanged));

        /// <summary>
        /// Gets or sets the DotRadiusY property. This dependency property 
        /// indicates the corner height of each of the dots.
        /// </summary>
        public double DotRadiusY
        {
            get { return (double)GetValue(DotRadiusYProperty); }
            set { SetValue(DotRadiusYProperty, value); }
        }

        /// <summary>
        /// Handles changes to the DotRadiusY property.
        /// </summary>
        /// <param name="d">FluidProgressBar</param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnDotRadiusYChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pBar = (FluidProgressBar)d;
            var oldDotRadiusY = (double)e.OldValue;
            var newDotRadiusY = pBar.DotRadiusY;
            pBar.OnDotRadiusYChanged(oldDotRadiusY, newDotRadiusY);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the DotRadiusY property.
        /// </summary>
        /// <param name="oldDotRadiusY">Old Value</param>
        /// <param name="newDotRadiusY">New Value</param>
        protected virtual void OnDotRadiusYChanged(double oldDotRadiusY, double newDotRadiusY)
        {
            if (_isStoryboardRunning)
            {
                RestartStoryboardAnimation();
            }
        }
        #endregion

        #region DurationA
        /// <summary>
        /// DurationA Dependency Property
        /// </summary>
        public static readonly DependencyProperty DurationAProperty =
            DependencyProperty.Register(nameof(DurationA), typeof(Duration), typeof(FluidProgressBar),
                new FrameworkPropertyMetadata(new Duration(TimeSpan.FromSeconds(0.5)), OnDurationAChanged));

        /// <summary>
        /// Gets or sets the DurationA property. This dependency property 
        /// indicates the duration of the animation from the start point till KeyFrameA.
        /// </summary>
        public Duration DurationA
        {
            get { return (Duration)GetValue(DurationAProperty); }
            set { SetValue(DurationAProperty, value); }
        }

        /// <summary>
        /// Handles changes to the DurationA property.
        /// </summary>
        /// <param name="d">FluidProgressBar</param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnDurationAChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pBar = (FluidProgressBar)d;
            var oldDurationA = (Duration)e.OldValue;
            var newDurationA = pBar.DurationA;
            pBar.OnDurationAChanged(oldDurationA, newDurationA);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the DurationA property.
        /// </summary>
        /// <param name="oldDurationA">Old Value</param>
        /// <param name="newDurationA">New Value</param>
        protected virtual void OnDurationAChanged(Duration oldDurationA, Duration newDurationA)
        {
            var isActive = _isStoryboardRunning;
            if (isActive)
            {
                StopFluidAnimation();
            }

            UpdateKeyTimes(1, newDurationA);

            if (isActive)
            {
                StartFluidAnimation();
            }
        }
        #endregion

        #region DurationB
        /// <summary>
        /// DurationB Dependency Property
        /// </summary>
        public static readonly DependencyProperty DurationBProperty =
            DependencyProperty.Register(nameof(DurationB), typeof(Duration), typeof(FluidProgressBar),
                new FrameworkPropertyMetadata(new Duration(TimeSpan.FromSeconds(1.5)), OnDurationBChanged));

        /// <summary>
        /// Gets or sets the DurationB property. This dependency property 
        /// indicates the duration of the animation from the KeyFrameA till KeyFrameB.
        /// </summary>
        public Duration DurationB
        {
            get { return (Duration)GetValue(DurationBProperty); }
            set { SetValue(DurationBProperty, value); }
        }

        /// <summary>
        /// Handles changes to the DurationB property.
        /// </summary>
        /// <param name="d">FluidProgressBar</param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnDurationBChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pBar = (FluidProgressBar)d;
            var oldDurationB = (Duration)e.OldValue;
            var newDurationB = pBar.DurationB;
            pBar.OnDurationBChanged(oldDurationB, newDurationB);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the DurationB property.
        /// </summary>
        /// <param name="oldDurationB">Old Value</param>
        /// <param name="newDurationB">New Value</param>
        protected virtual void OnDurationBChanged(Duration oldDurationB, Duration newDurationB)
        {
            var isActive = _isStoryboardRunning;
            if (isActive)
            {
                StopFluidAnimation();
            }

            UpdateKeyTimes(2, newDurationB);

            if (isActive)
            {
                StartFluidAnimation();
            }
        }
        #endregion

        #region DurationC
        /// <summary>
        /// DurationC Dependency Property
        /// </summary>
        public static readonly DependencyProperty DurationCProperty =
            DependencyProperty.Register(nameof(DurationC), typeof(Duration), typeof(FluidProgressBar),
                new FrameworkPropertyMetadata(new Duration(TimeSpan.FromSeconds(0.5)), OnDurationCChanged));

        /// <summary>
        /// Gets or sets the DurationC property. This dependency property 
        /// indicates the duration of the animation from KeyFrameB till the end point.
        /// </summary>
        public Duration DurationC
        {
            get { return (Duration)GetValue(DurationCProperty); }
            set { SetValue(DurationCProperty, value); }
        }

        /// <summary>
        /// Handles changes to the DurationC property.
        /// </summary>
        /// <param name="d">FluidProgressBar</param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnDurationCChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pBar = (FluidProgressBar)d;
            var oldDurationC = (Duration)e.OldValue;
            var newDurationC = pBar.DurationC;
            pBar.OnDurationCChanged(oldDurationC, newDurationC);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the DurationC property.
        /// </summary>
        /// <param name="oldDurationC">Old Value</param>
        /// <param name="newDurationC">New Value</param>
        protected virtual void OnDurationCChanged(Duration oldDurationC, Duration newDurationC)
        {
            var isActive = _isStoryboardRunning;
            if (isActive)
            {
                StopFluidAnimation();
            }

            UpdateKeyTimes(3, newDurationC);

            if (isActive)
            {
                StartFluidAnimation();
            }
        }
        #endregion

        #region KeyFrameA
        /// <summary>
        /// KeyFrameA Dependency Property
        /// </summary>
        public static readonly DependencyProperty KeyFrameAProperty =
            DependencyProperty.Register(nameof(KeyFrameA), typeof(double), typeof(FluidProgressBar),
                new FrameworkPropertyMetadata(0.33, OnKeyFrameAChanged));

        /// <summary>
        /// Gets or sets the KeyFrameA property. This dependency property 
        /// indicates the first KeyFrame position after the initial keyframe.
        /// </summary>
        public double KeyFrameA
        {
            get { return (double)GetValue(KeyFrameAProperty); }
            set { SetValue(KeyFrameAProperty, value); }
        }

        /// <summary>
        /// Handles changes to the KeyFrameA property.
        /// </summary>
        /// <param name="d">FluidProgressBar</param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnKeyFrameAChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pBar = (FluidProgressBar)d;
            var oldKeyFrameA = (double)e.OldValue;
            var newKeyFrameA = pBar.KeyFrameA;
            pBar.OnKeyFrameAChanged(oldKeyFrameA, newKeyFrameA);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the KeyFrameA property.
        /// </summary>
        /// <param name="oldKeyFrameA">Old Value</param>
        /// <param name="newKeyFrameA">New Value</param>
        protected virtual void OnKeyFrameAChanged(double oldKeyFrameA, double newKeyFrameA)
        {
            RestartStoryboardAnimation();
        }
        #endregion

        #region KeyFrameB
        /// <summary>
        /// KeyFrameB Dependency Property
        /// </summary>
        public static readonly DependencyProperty KeyFrameBProperty =
            DependencyProperty.Register(nameof(KeyFrameB), typeof(double), typeof(FluidProgressBar),
                new FrameworkPropertyMetadata(0.63, OnKeyFrameBChanged));

        /// <summary>
        /// Gets or sets the KeyFrameB property. This dependency property 
        /// indicates the second KeyFrame position after the initial keyframe.
        /// </summary>
        public double KeyFrameB
        {
            get { return (double)GetValue(KeyFrameBProperty); }
            set { SetValue(KeyFrameBProperty, value); }
        }

        /// <summary>
        /// Handles changes to the KeyFrameB property.
        /// </summary>
        /// <param name="d">FluidProgressBar</param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnKeyFrameBChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pBar = (FluidProgressBar)d;
            var oldKeyFrameB = (double)e.OldValue;
            var newKeyFrameB = pBar.KeyFrameB;
            pBar.OnKeyFrameBChanged(oldKeyFrameB, newKeyFrameB);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the KeyFrameB property.
        /// </summary>
        /// <param name="oldKeyFrameB">Old Value</param>
        /// <param name="newKeyFrameB">New Value</param>
        protected virtual void OnKeyFrameBChanged(double oldKeyFrameB, double newKeyFrameB)
        {
            RestartStoryboardAnimation();
        }
        #endregion

        #region Oscillate
        /// <summary>
        /// Oscillate Dependency Property
        /// </summary>
        public static readonly DependencyProperty OscillateProperty =
            DependencyProperty.Register(nameof(Oscillate), typeof(bool), typeof(FluidProgressBar),
                new FrameworkPropertyMetadata(false, OnOscillateChanged));

        /// <summary>
        /// Gets or sets the Oscillate property. This dependency property 
        /// indicates whether the animation should oscillate.
        /// </summary>
        public bool Oscillate
        {
            get { return (bool)GetValue(OscillateProperty); }
            set { SetValue(OscillateProperty, value); }
        }

        /// <summary>
        /// Handles changes to the Oscillate property.
        /// </summary>
        /// <param name="d">FluidProgressBar</param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnOscillateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pBar = (FluidProgressBar)d;
            var oldOscillate = (bool)e.OldValue;
            var newOscillate = pBar.Oscillate;
            pBar.OnOscillateChanged(oldOscillate, newOscillate);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the Oscillate property.
        /// </summary>
        /// <param name="oldOscillate">Old Value</param>
        /// <param name="newOscillate">New Value</param>
        protected virtual void OnOscillateChanged(bool oldOscillate, bool newOscillate)
        {
            if (_sb == null)
            {
                return;
            }

            StopFluidAnimation();

            _sb.SetCurrentValue(Timeline.AutoReverseProperty, newOscillate);
            _sb.SetCurrentValue(Timeline.DurationProperty, newOscillate ? ReverseDuration : TotalDuration);

            StartFluidAnimation();
        }
        #endregion

        #region ReverseDuration
        /// <summary>
        /// ReverseDuration Dependency Property
        /// </summary>
        public static readonly DependencyProperty ReverseDurationProperty =
            DependencyProperty.Register(nameof(ReverseDuration), typeof(Duration), typeof(FluidProgressBar),
                new FrameworkPropertyMetadata(new Duration(TimeSpan.FromSeconds(2.9)), OnReverseDurationChanged));

        /// <summary>
        /// Gets or sets the ReverseDuration property. This dependency property 
        /// indicates the duration of the total animation in reverse.
        /// </summary>
        public Duration ReverseDuration
        {
            get { return (Duration)GetValue(ReverseDurationProperty); }
            set { SetValue(ReverseDurationProperty, value); }
        }

        /// <summary>
        /// Handles changes to the ReverseDuration property.
        /// </summary>
        /// <param name="d">FluidProgressBar</param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnReverseDurationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pBar = (FluidProgressBar)d;
            var oldReverseDuration = (Duration)e.OldValue;
            var newReverseDuration = pBar.ReverseDuration;
            pBar.OnReverseDurationChanged(oldReverseDuration, newReverseDuration);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the ReverseDuration property.
        /// </summary>
        /// <param name="oldReverseDuration">Old Value</param>
        /// <param name="newReverseDuration">New Value</param>
        protected virtual void OnReverseDurationChanged(Duration oldReverseDuration, Duration newReverseDuration)
        {
            if (_sb == null || !Oscillate)
            {
                return;
            }

            _sb.SetCurrentValue(Timeline.DurationProperty, newReverseDuration);
            RestartStoryboardAnimation();
        }
        #endregion

        #region TotalDuration
        /// <summary>
        /// TotalDuration Dependency Property
        /// </summary>
        public static readonly DependencyProperty TotalDurationProperty =
            DependencyProperty.Register(nameof(TotalDuration), typeof(Duration), typeof(FluidProgressBar),
                new FrameworkPropertyMetadata(new Duration(TimeSpan.FromSeconds(4.4)), OnTotalDurationChanged));

        /// <summary>
        /// Gets or sets the TotalDuration property. This dependency property 
        /// indicates the duration of the complete animation.
        /// </summary>
        public Duration TotalDuration
        {
            get { return (Duration)GetValue(TotalDurationProperty); }
            set { SetValue(TotalDurationProperty, value); }
        }

        /// <summary>
        /// Handles changes to the TotalDuration property.
        /// </summary>
        /// <param name="d">FluidProgressBar</param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnTotalDurationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pBar = (FluidProgressBar)d;
            var oldTotalDuration = (Duration)e.OldValue;
            var newTotalDuration = pBar.TotalDuration;
            pBar.OnTotalDurationChanged(oldTotalDuration, newTotalDuration);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the TotalDuration property.
        /// </summary>
        /// <param name="oldTotalDuration">Old Value</param>
        /// <param name="newTotalDuration">New Value</param>
        protected virtual void OnTotalDurationChanged(Duration oldTotalDuration, Duration newTotalDuration)
        {
            if (_sb == null || Oscillate)
            {
                return;
            }

            _sb.SetCurrentValue(Timeline.DurationProperty, newTotalDuration);

            RestartStoryboardAnimation();
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Handles the Loaded event
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // Update the key frames
            UpdateKeyFrames();
            // Start the animation
            StartFluidAnimation();
        }

        /// <summary>
        /// Handles the SizeChanged event
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Restart the animation
            RestartStoryboardAnimation();
        }

        /// <summary>
        /// Handles the IsVisibleChanged event
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                UpdateKeyFrames();
                StartFluidAnimation();
            }
            else
            {
                StopFluidAnimation();
            }
        }
        #endregion

        #region Helpers
        /// <summary>
        /// Starts the animation
        /// </summary>
        private void StartFluidAnimation()
        {
            if (_sb == null || (_isStoryboardRunning))
            {
                return;
            }

            _sb.Begin();
            _isStoryboardRunning = true;
        }

        /// <summary>
        /// Stops the animation
        /// </summary>
        private void StopFluidAnimation()
        {
            if (_sb == null || (!_isStoryboardRunning))
            {
                return;
            }

            // Move the timeline to the end and stop the animation
            _sb.SeekAlignedToLastTick(TimeSpan.FromSeconds(0));
            _sb.Stop();
            _isStoryboardRunning = false;
        }

        /// <summary>
        /// Stops the animation, updates the keyframes and starts the animation
        /// </summary>
        private void RestartStoryboardAnimation()
        {
            StopFluidAnimation();
            UpdateKeyFrames();
            StartFluidAnimation();
        }

        /// <summary>
        /// Update the key value of each keyframe based on the current width of the FluidProgressBar
        /// </summary>
        private void UpdateKeyFrames()
        {
            // Get the current width of the FluidProgressBar
            var width = ActualWidth;
            // Update the values only if the current width is greater than Zero and is visible
            if (!(width > 0.0) || Visibility != Visibility.Visible)
            {
                return;
            }

            const int point0 = 0;
            var pointA = width * KeyFrameA;
            var pointB = width * KeyFrameB;
            var pointC = width - DotWidth;

            // Update the keyframes stored in the map
            UpdateKeyFrame(0, point0);
            UpdateKeyFrame(1, pointA);
            UpdateKeyFrame(2, pointB);
            UpdateKeyFrame(3, pointC);

            var height = ActualHeight;
            var dotHeight = DotHeight;

            _dot1.SetCurrentValue(Canvas.TopProperty, (height - dotHeight) / 2);
            _dot2.SetCurrentValue(Canvas.TopProperty, (height - dotHeight) / 2);
            _dot3.SetCurrentValue(Canvas.TopProperty, (height - dotHeight) / 2);
            _dot4.SetCurrentValue(Canvas.TopProperty, (height - dotHeight) / 2);
            _dot5.SetCurrentValue(Canvas.TopProperty, (height - dotHeight) / 2);
        }

        /// <summary>
        /// Update the key value of the keyframes stored in the map
        /// </summary>
        /// <param name="key">Key of the dictionary</param>
        /// <param name="newValue">New value to be given to the key value of the keyframes</param>
        private void UpdateKeyFrame(int key, double newValue)
        {
            if (!_keyFrameMap.ContainsKey(key))
            {
                return;
            }

            foreach (var frame in _keyFrameMap[key].KeyFrames.Where(frame => frame is LinearDoubleKeyFrame || frame is EasingDoubleKeyFrame))
            {
                frame.SetCurrentValue(DoubleKeyFrame.ValueProperty, newValue);
            }
        }

        /// <summary>
        /// Updates the duration of each of the keyframes stored in the map
        /// </summary>
        /// <param name="key">Key of the dictionary</param>
        /// <param name="newDuration">The new duration.</param>
        private void UpdateKeyTimes(int key, Duration newDuration)
        {
            switch (key)
            {
                case 1:
                    UpdateKeyTime(1, newDuration);
                    UpdateKeyTime(2, newDuration + DurationB);
                    UpdateKeyTime(3, newDuration + DurationB + DurationC);
                    break;

                case 2:
                    UpdateKeyTime(2, DurationA + newDuration);
                    UpdateKeyTime(3, DurationA + newDuration + DurationC);
                    break;

                case 3:
                    UpdateKeyTime(3, DurationA + DurationB + newDuration);
                    break;

                default:
                    break;
            }

            // Update the opacity animation duration based on the complete duration
            // of the animation
            UpdateOpacityKeyTime(1, DurationA + DurationB + DurationC);
        }

        /// <summary>
        /// Updates the duration of each of the keyframes stored in the map
        /// </summary>
        /// <param name="key">Key of the dictionary</param>
        /// <param name="newDuration">New value to be given to the duration value of the keyframes</param>
        private void UpdateKeyTime(int key, Duration newDuration)
        {
            if (!_keyFrameMap.ContainsKey(key))
            {
                return;
            }

            var newKeyTime = KeyTime.FromTimeSpan(newDuration.TimeSpan);
            _keyFrameMap[key].KeyFrameTime = newKeyTime;

            foreach (var frame in _keyFrameMap[key].KeyFrames.Where(frame => frame is LinearDoubleKeyFrame || frame is EasingDoubleKeyFrame))
            {
                frame.SetCurrentValue(DoubleKeyFrame.KeyTimeProperty, newKeyTime);
            }
        }

        /// <summary>
        /// Updates the duration of the second keyframe of all the opacity animations
        /// </summary>
        /// <param name="key">Key of the dictionary</param>
        /// <param name="newDuration">New value to be given to the duration value of the keyframes</param>
        private void UpdateOpacityKeyTime(int key, Duration newDuration)
        {
            if (!_opKeyFrameMap.ContainsKey(key))
            {
                return;
            }

            var newKeyTime = KeyTime.FromTimeSpan(newDuration.TimeSpan);
            _opKeyFrameMap[key].KeyFrameTime = newKeyTime;

            foreach (var frame in _opKeyFrameMap[key].KeyFrames.OfType<DiscreteDoubleKeyFrame>())
            {
                frame.SetCurrentValue(DoubleKeyFrame.KeyTimeProperty, newKeyTime);
            }
        }

        /// <summary>
        /// Updates the delay between consecutive timeLines
        /// </summary>
        /// <param name="newDelay">Delay duration</param>
        private void UpdateTimelineDelay(Duration newDelay)
        {
            var nextDelay = new Duration(TimeSpan.FromSeconds(0));

            if (_sb == null)
            {
                return;
            }

            for (var i = 0; i < _sb.Children.Count; i++)
            {
                // The first five animations are for translation
                // The next five animations are for opacity
                if (i == 5)
                {
                    nextDelay = newDelay;
                }
                else
                {
                    nextDelay += newDelay;
                }

                if (_sb.Children[i] is DoubleAnimationUsingKeyFrames timeline)
                {
                    timeline.SetCurrentValue(Timeline.BeginTimeProperty, nextDelay.TimeSpan);
                }
            }
        }
        #endregion

        #region IDisposable Implementation
        /// <summary>
        /// Releases all resources used by an instance of the FluidProgressBar class.
        /// </summary>
        /// <remarks>
        /// This method calls the virtual Dispose(bool) method, passing in 'true', and then suppresses 
        /// finalization of the instance.
        /// </remarks>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged resources before an instance of the FluidProgressBar class is reclaimed by garbage collection.
        /// </summary>
        /// <remarks>
        /// NOTE: Leave out the finalizer altogether if this class doesn't own unmanaged resources itself, 
        /// but leave the other methods exactly as they are.
        /// This method releases unmanaged resources by calling the virtual Dispose(bool), passing in 'false'.
        /// </remarks>
        ~FluidProgressBar()
        {
            Dispose(false);
        }

        /// <summary>
        /// Releases the unmanaged resources used by an instance of the FluidProgressBar class and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">'true' to release both managed and unmanaged resources; 'false' to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            // free managed resources here
            SizeChanged -= OnSizeChanged;
            Loaded -= OnLoaded;
            IsVisibleChanged -= OnIsVisibleChanged;

            // free native resources if there are any.			
        }
        #endregion
    }
}
