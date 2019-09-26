// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpenToolCommandExtension.cs" company="WildGums">
//   Copyright (c) 2008 - 2019 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using Catel.MVVM;
    using Catel.Windows;
    using Catel.Windows.Markup;

    public class OpenToolCommandExtension : UpdatableMarkupExtension
    {
        #region Fields
        private readonly Type _frameworkElementType;
        private readonly Type _toolType;
        #endregion

        #region Constructors
        public OpenToolCommandExtension(Type toolType, Type frameworkElementType)
        {
            _toolType = toolType;
            _frameworkElementType = frameworkElementType;
        }
        #endregion

        #region Methods
        protected override object ProvideDynamicValue(IServiceProvider serviceProvider)
        {
            return new Command<object>(OnOpenTool, CanAttachTool);
        }

        private bool CanAttachTool(object parameter)
        {
            if (!(TargetObject is FrameworkElement targetObject))
            {
                return false;
            }

            var contextMenu = targetObject.FindLogicalAncestorByType<ContextMenu>()
                              ?? targetObject.FindLogicalOrVisualAncestor(x => x.GetType() == typeof(ContextMenu)) as ContextMenu;

            if (!(contextMenu?.PlacementTarget is FrameworkElement placementTarget))
            {
                return false;
            }

            if (placementTarget.GetType() == _frameworkElementType)
            {
                return placementTarget.AttachTool(_toolType) != null;
            }

            return true;
        }

        private void OnOpenTool(object parameter)
        {
            if (!(TargetObject is FrameworkElement targetObject))
            {
                return;
            }

            var contextMenu = targetObject.FindLogicalAncestorByType<ContextMenu>() 
                              ?? targetObject.FindLogicalOrVisualAncestor(x => x.GetType() == typeof(ContextMenu)) as ContextMenu;

            if (!(contextMenu?.PlacementTarget is FrameworkElement placementTarget))
            {
                return;
            }

            if (placementTarget.GetType() == _frameworkElementType)
            {
                placementTarget.AttachAndOpenTool(_toolType, parameter);
                return;
            }

            var ancestorElement = placementTarget.FindLogicalOrVisualAncestor(x => x.GetType() == _frameworkElementType) as FrameworkElement;
            ancestorElement?.AttachAndOpenTool(_toolType, parameter);
        }
        #endregion
    }
}
