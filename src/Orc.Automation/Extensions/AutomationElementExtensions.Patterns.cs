namespace Orc.Automation
{
    using System;
    using System.Linq;
    using System.Windows.Automation;
    using Catel;

    public static partial class AutomationElementExtensions
    {
        #region Get Value
        public static T GetValue<T>(this AutomationElement element)
        {
            Argument.IsNotNull(() => element);

            if (typeof(T) == typeof(double))
            {
                return (T)(object)element.RunPatternFunc<RangeValuePattern, double>(x => x.Current.Value);
            }

            if (typeof(T) == typeof(string))
            {
                return (T)(object)element.RunPatternFunc<ValuePattern, string>(x => x.Current.Value);
            }

            throw new AutomationException("Can't get value");
        }

        public static bool TryGetValue(this AutomationElement element, out double value)
        {
            Argument.IsNotNull(() => element);

            var localValue = 0d;
            value = 0d;

            var result = element.TryRunPatternFunc<RangeValuePattern>(x => localValue = x.Current.Value);
            if (result)
            {
                value = localValue;

                return true;
            }

            return false;
        }
        #endregion

        #region Set Value
        public static void SetValue(this AutomationElement element, double value)
        {
            Argument.IsNotNull(() => element);

            element.RunPatternFunc<RangeValuePattern>(x => x.SetValue(value));
        }

        public static void SetValue(this AutomationElement element, string value)
        {
            Argument.IsNotNull(() => element);

            element.RunPatternFunc<ValuePattern>(x => x.SetValue(value));
        }

        public static bool TrySetValue(this AutomationElement element, double value)
        {
            Argument.IsNotNull(() => element);

            return element.TryRunPatternFunc<RangeValuePattern>(x => x.SetValue(value));
        }

        public static bool TrySetValue(this AutomationElement element, string value)
        {
            Argument.IsNotNull(() => element);

            return element.TryRunPatternFunc<ValuePattern>(x => x.SetValue(value));
        }
        #endregion

        #region Range
        public static double GetRangeMinimum(this AutomationElement element)
        {
            return element.RunPatternFunc<RangeValuePattern, double>(x => x.Current.Minimum);
        }

        public static double GetRangeMaximum(this AutomationElement element)
        {
            Argument.IsNotNull(() => element);

            return element.RunPatternFunc<RangeValuePattern, double>(x => x.Current.Maximum);
        }

        public static double GetRangeSmallChange(this AutomationElement element)
        {
            Argument.IsNotNull(() => element);

            return element.RunPatternFunc<RangeValuePattern, double>(x => x.Current.SmallChange);
        }

        public static double GetRangeLargeChange(this AutomationElement element)
        {
            Argument.IsNotNull(() => element);

            return element.RunPatternFunc<RangeValuePattern, double>(x => x.Current.LargeChange);
        }

        public static bool TryGetRangeSmallChange(this AutomationElement element, out double smallChange)
        {
            Argument.IsNotNull(() => element);

            return element.TryRunPatternFunc<RangeValuePattern, double>(x => x.Current.SmallChange, out smallChange);
        }

        public static bool TryGetRangeLargeChange(this AutomationElement element, out double largeChange)
        {
            Argument.IsNotNull(() => element);

            return element.TryRunPatternFunc<RangeValuePattern, double>(x => x.Current.LargeChange, out largeChange);
        }

        public static bool TryGetRangeMinimum(this AutomationElement element, out double minimum)
        {
            Argument.IsNotNull(() => element);

            return element.TryRunPatternFunc<RangeValuePattern, double>(x => x.Current.Minimum, out minimum);
        }

        public static bool TryGetRangeMaximum(this AutomationElement element, out double maximum)
        {
            Argument.IsNotNull(() => element);

            return element.TryRunPatternFunc<RangeValuePattern, double>(x => x.Current.Maximum, out maximum);
        }
        #endregion

        #region Select
        public static void Select(this AutomationElement element)
        {
            Argument.IsNotNull(() => element);

            element.RunPatternFunc<SelectionItemPattern>(x => x.Select());
        }

        public static bool GetIsSelected(this AutomationElement element)
        {
            Argument.IsNotNull(() => element);

            return element.RunPatternFunc<SelectionItemPattern, bool>(x => x.Current.IsSelected);
        }

        public static bool TrySetSelection(this AutomationElement element, bool isSelected)
        {
            if (!TryGetIsSelected(element, out var isCurrentlySelected))
            {
                return false;
            }

            if (Equals(isSelected, isCurrentlySelected))
            {
                return true;
            }

            if (isSelected)
            {
                var container = element.GetSelectionContainer();
                var canSelectMultiply = container.CanSelectMultiple();
                if (canSelectMultiply)
                {
                    return TryAddToSelection(element);
                }

                return TrySelect(element);
            }

            return TryDeselect(element);
        }

        public static AutomationElement GetSelectionContainer(this AutomationElement element)
        {
            Argument.IsNotNull(() => element);

            return element.RunPatternFunc<SelectionItemPattern, AutomationElement>(x => x.Current.SelectionContainer);
        }

        public static bool TryAddToSelection(this AutomationElement element)
        {
            Argument.IsNotNull(() => element);

            return element.TryRunPatternFunc<SelectionItemPattern>(x => x.AddToSelection());
        }

        public static bool TrySelect(this AutomationElement element)
        {
            Argument.IsNotNull(() => element);

            return element.TryRunPatternFunc<SelectionItemPattern>(x => x.Select());
        }

        public static bool TryDeselect(this AutomationElement element)
        {
            Argument.IsNotNull(() => element);

            return element.TryRunPatternFunc<SelectionItemPattern>(x => x.RemoveFromSelection());
        }

        public static bool TryGetIsSelected(this AutomationElement element, out bool isSelected)
        {
            Argument.IsNotNull(() => element);

            isSelected = false;
            var localIsSelected = false;
            if (element.TryRunPatternFunc<SelectionItemPattern>(x => localIsSelected = x.Current.IsSelected))
            {
                isSelected = localIsSelected;

                return true;
            }

            return false;
        }
        #endregion

        #region Select Item
        public static AutomationElement[] GetSelection(this AutomationElement container)
        {
            Argument.IsNotNull(() => container);

            return container.RunPatternFunc<SelectionPattern, AutomationElement[]>(x => x.Current.GetSelection());
        }

        public static bool CanSelectMultiple(this AutomationElement container)
        {
            Argument.IsNotNull(() => container);

            return container.RunPatternFunc<SelectionPattern, bool>(x => x.Current.CanSelectMultiple);
        }

        public static bool TrySelectItem(this AutomationElement containerElement, int index, out AutomationElement selectItem)
        {
            Argument.IsNotNull(() => containerElement);

            selectItem = GetChild(containerElement, index);
            return selectItem?.TrySelect() == true;
        }
        #endregion

        #region Invoke
        public static void Invoke(this AutomationElement element)
        {
            element.RunPatternFunc<InvokePattern>(x => x.Invoke());
        }

        public static bool TryInvoke(this AutomationElement element)
        {
            return element.TryRunPatternFunc<InvokePattern>(x => x.Invoke());
        }
        #endregion

        #region Toggle
        /// <summary>
        /// Toggle element if TogglePattern is available
        /// </summary>
        /// <param name="element">input element</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">if element is null</exception>
        /// <exception cref="AutomationException">if Toggle pattern is not supported</exception>
        public static bool? Toggle(this AutomationElement element)
        {
            Argument.IsNotNull(() => element);

            if (TryToggle(element, out var state))
            {
                return state;
            }

            throw new AutomationException("Can't toggle, pattern not available");
        }

        public static bool TrySetToggleState(this AutomationElement element, bool? newState)
        {
            if (!TryGetToggleState(element, out var toggleState))
            {
                return false;
            }

            if (toggleState == newState)
            {
                return true;
            }

            if (!TryToggle(element, out toggleState))
            {
                return false;
            }
            
            if (toggleState == newState)
            {
                return true;
            }

            if (!TryToggle(element, out toggleState))
            {
                return false;
            }

            if (toggleState == newState)
            {
                return true;
            }

            //After 2 toggles result doesn't match requested value
            return false;
        }

        public static bool? GetToggleState(this AutomationElement element)
        {
            if (TryGetToggleState(element, out var toggleState))
            {
                return toggleState;
            }

            throw new AutomationException("Can't get toggle state");
        }

        public static bool TryToggle(this AutomationElement element)
        {
            return element.TryRunPatternFunc<TogglePattern>(x => x.Toggle());
        }

        public static bool TryToggle(this AutomationElement element, out bool? toggleState)
        {
            toggleState = null;

            return TryToggle(element) && TryGetToggleState(element, out toggleState);
        }

        public static bool TryGetToggleState(this AutomationElement element, out bool? toggleState)
        {
            var state = ToggleState.Off;

            var result = element.TryRunPatternFunc<TogglePattern>(x => state = x.Current.ToggleState);

            toggleState = state switch
            {
                ToggleState.Off => false,
                ToggleState.On => true,
                ToggleState.Indeterminate => null,
                _ => throw new ArgumentOutOfRangeException()
            };

            return result;
        }
        #endregion

        #region Expand/Collapse
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public static void Expand(this AutomationElement element)
        {
            element.RunPatternFunc<ExpandCollapsePattern>(x => x.Expand());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public static void Collapse(this AutomationElement element)
        {
            element.RunPatternFunc<ExpandCollapsePattern>(x => x.Collapse());
        }

        public static bool GetIsExpanded(this AutomationElement element)
        {
            return element.RunPatternFunc<ExpandCollapsePattern, ExpandCollapseState>(x => x.Current.ExpandCollapseState) == ExpandCollapseState.Expanded;
        }

        public static bool TryExpand(this AutomationElement element)
        {
            return element.TryRunPatternFunc<ExpandCollapsePattern>(x => x.Expand());
        }

        public static bool TryCollapse(this AutomationElement element)
        {
            return element.TryRunPatternFunc<ExpandCollapsePattern>(x => x.Collapse());
        }
        #endregion

        #region Window
        public static void CloseWindow(this AutomationElement element)
        {
            element.RunPatternFunc<WindowPattern>(x => x.Close());
        }

        public static bool TryCloseWindow(this AutomationElement element)
        {
            return element.TryRunPatternFunc<WindowPattern>(x => x.Close());
        }
        #endregion

        /// <summary>
        /// Try to Invoke, Toggle, Select...if this patterns not implemented, depends on useMouse parameter use Mouse Input
        /// </summary>
        /// <param name="element"></param>
        /// <param name="useMouse"></param>
        /// <returns></returns>
        public static bool TryClick(this AutomationElement element, bool useMouse = true)
        {
            Argument.IsNotNull(() => element);

            try
            {
                if (!element.TryInvoke() && !element.TryToggle() && !element.TrySelect() && useMouse)
                {
                    MouseClick(element);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            return true;
        }
        
        public static TResult RunPatternFunc<TPattern, TResult>(this AutomationElement element, Func<TPattern, TResult> func)
            where TPattern : BasePattern
        {
            Argument.IsNotNull(() => element);

            return TryRunPatternFunc(element, func, out var funcResult)
                ? funcResult
                : throw new AutomationException($"Can't run pattern {typeof(TPattern).Name}");
        }

        public static void RunPatternFunc<TPattern>(this AutomationElement element, Action<TPattern> action)
            where TPattern : BasePattern
        {
            Argument.IsNotNull(() => element);

            var result = TryRunPatternFunc(element, action);
            if (!result)
            {
                throw new AutomationException($"Can't run pattern {typeof(TPattern).Name}");
            }
        }

        public static bool TryRunPatternFunc<TPattern, TResult>(this AutomationElement element, Func<TPattern, TResult> func, out TResult functionResult)
            where TPattern : BasePattern
        {
            Argument.IsNotNull(() => element);

            functionResult = default;
            TResult localFuncResult = default;
            if (TryRunPatternFunc(element, (TPattern pattern) => localFuncResult = func(pattern)))
            {
                functionResult = localFuncResult;
                return true;
            }

            return false;
        }

        public static bool TryRunPatternFunc<TPattern>(this AutomationElement element, Action<TPattern> action)
            where TPattern : BasePattern
        {
            Argument.IsNotNull(() => element);

            var automationPattern = TryGetPattern<TPattern>(element);
            if (automationPattern is null)
            {
                return false;
            }

            try
            {
                action?.Invoke(automationPattern);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return false;
            }

            return true;
        }

        public static TPattern TryGetPattern<TPattern>(this AutomationElement element)
            where TPattern : BasePattern
        {
            var patternField = typeof(TPattern).GetField("Pattern");
            if (patternField?.GetValue(null) is not AutomationPattern pattern)
            {
                return null;
            }

            var supportedPatterns = element.GetSupportedPatterns();
            var automationPattern = supportedPatterns?.FirstOrDefault(x => x.ProgrammaticName == pattern.ProgrammaticName);
            if (automationPattern is null)
            {
                return null;
            }

            var currentPattern = element.GetCurrentPattern(automationPattern) as TPattern;

            return currentPattern;
        }
    }
}
