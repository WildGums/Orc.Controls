


//namespace Orc.Automation.Helpers
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Text;
//    using System.Threading;
//    using System.Windows.Automation;

//    /// <summary>
//    /// Class with various helper tools used in various places
//    /// </summary>
//    public static class Wait
//    {
//        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(1.0);

//        /// <summary>
//        /// Waits a little to allow inputs (mouse, keyboard, ...) to be processed.
//        /// </summary>
//        /// <param name="waitTimeout">An optional timeout. If no value or null is passed, the timeout is 100ms.</param>
//        public static void UntilInputIsProcessed(TimeSpan? waitTimeout = null) => Thread.Sleep((int)(waitTimeout ?? TimeSpan.FromMilliseconds(100.0)).TotalMilliseconds);

//        /// <summary>Waits until the given element is responsive.</summary>
//        /// <param name="automationElement">The element that should be waited for.</param>
//        /// <returns>True if the element was responsive, false otherwise.</returns>
//        public static bool UntilResponsive(AutomationElement automationElement) => Wait.UntilResponsive(automationElement, Wait.DefaultTimeout);

//        /// <summary>Waits until the given element is responsive.</summary>
//        /// <param name="automationElement">The element that should be waited for.</param>
//        /// <param name="timeout">The timeout of the waiting.</param>
//        /// <returns>True if the element was responsive, false otherwise.</returns>
//        public static bool UntilResponsive(AutomationElement automationElement, TimeSpan timeout)
//        {
//            AutomationElement element = automationElement;
//            TreeWalker controlViewWalker = automationElement.control;
//            while (element.Properties.NativeWindowHandle.ValueOrDefault == Win32Constants.FALSE)
//                element = controlViewWalker.GetParent(element);
//            return Wait.UntilResponsive((IntPtr)element.Properties.NativeWindowHandle, timeout);
//        }

//        /// <summary>
//        /// Waits until the given hwnd is responsive.
//        /// See: https://blogs.msdn.microsoft.com/oldnewthing/20161118-00/?p=94745
//        /// </summary>
//        /// <param name="hWnd">The hwnd that should be waited for.</param>
//        /// <returns>True if the hwnd was responsive, false otherwise.</returns>
//        public static bool UntilResponsive(IntPtr hWnd) => Wait.UntilResponsive(hWnd, Wait.DefaultTimeout);

//        /// <summary>
//        /// Waits until the given hwnd is responsive.
//        /// See: https://blogs.msdn.microsoft.com/oldnewthing/20161118-00/?p=94745
//        /// </summary>
//        /// <param name="hWnd">The hwnd that should be waited for.</param>
//        /// <param name="timeout">The timeout of the waiting.</param>
//        /// <returns>True if the hwnd was responsive, false otherwise.</returns>
//        public static bool UntilResponsive(IntPtr hWnd, TimeSpan timeout)
//        {
//            IntPtr num = User32.SendMessageTimeout(hWnd, 0U, UIntPtr.Zero, IntPtr.Zero, SendMessageTimeoutFlags.SMTO_NORMAL, (uint)timeout.TotalMilliseconds, out UIntPtr _);
//            Thread.Sleep(20);
//            IntPtr zero = IntPtr.Zero;
//            return num != zero;
//        }
//    }
//}
