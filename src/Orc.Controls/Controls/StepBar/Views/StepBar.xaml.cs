﻿namespace Orc.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using Catel.MVVM;
    using Catel.MVVM.Views;
    using Catel.Threading;
    using Catel.Windows.Threading;

    public sealed partial class StepBar
    {
        static StepBar()
        {
            typeof(StepBar).AutoDetectViewPropertiesToSubscribe();
        }

        public StepBar()
        {
            InitializeComponent();
        }

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof(Orientation), typeof(Orientation),
            typeof(StepBar), new PropertyMetadata(Orientation.Horizontal));


        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public IList<IStepBarItem> Items
        {
            get { return (IList<IStepBarItem>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(nameof(Items), typeof(IList<IStepBarItem>),
            typeof(StepBar));


        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public IStepBarItem SelectedItem
        {
            get { return (IStepBarItem)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(nameof(SelectedItem), typeof(IStepBarItem),
            typeof(StepBar), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        protected override void OnViewModelPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnViewModelPropertyChanged(e);

            if (e.HasPropertyChanged(nameof(StepBarViewModel.SelectedItem)))
            {
                Dispatcher.BeginInvoke(async () =>
                {
                    stepbarListBox.CenterSelectedItem();

                    // We need to await the animation
                    await TaskShim.Delay(StepBarConfiguration.AnimationDuration);
                });
            }
        }
    }
}