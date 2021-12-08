namespace Orc.Automation
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Input;

#pragma warning disable IDE1006 // Naming Styles
    // https://gist.github.com/DrustZ/640912b9d5cb745a3a56971c9bd58ac7
    internal static class NativeMethods
    {
        //User32 wrappers cover API's used for Mouse input
        #region User32
        // Two special bitmasks we define to be able to grab
        // shift and character information out of a VKey.
        internal const int VKeyShiftMask = 0x0100;
        internal const int VKeyCharMask = 0x00FF;

        // Various Win32 constants
        internal const int KeyeventfExtendedkey = 0x0001;
        internal const int KeyeventfKeyup = 0x0002;
        internal const int KeyeventfScancode = 0x0008;

        internal const int MouseeventfVirtualdesk = 0x4000;

        internal const int SmXvirtualscreen = 76;
        internal const int SmYvirtualscreen = 77;
        internal const int SmCxvirtualscreen = 78;
        internal const int SmCyvirtualscreen = 79;

        internal const int XButton1 = 0x0001;
        internal const int XButton2 = 0x0002;
        internal const int WheelDelta = 120;

        internal const int InputMouse = 0;
        internal const int InputKeyboard = 1;

        // Various Win32 data structures
        [StructLayout(LayoutKind.Sequential)]
        internal struct Input
        {
            internal int type;
            internal Inputunion union;
        };

        [StructLayout(LayoutKind.Explicit)]
        internal struct Inputunion
        {
            [FieldOffset(0)]
            internal Mouseinput mouseInput;
            [FieldOffset(0)]
            internal Keybdinput keyboardInput;
        };

        [StructLayout(LayoutKind.Sequential)]
        internal struct Mouseinput
        {
            internal int dx;

            internal int dy;

            internal int mouseData;
            internal int dwFlags;
            internal int time;
            internal IntPtr dwExtraInfo;
        };

        [StructLayout(LayoutKind.Sequential)]
        internal struct Keybdinput
        {
            internal short wVk;
            internal short wScan;
            internal int dwFlags;
            internal int time;
            internal IntPtr dwExtraInfo;
        };

        [Flags]
        internal enum SendMouseInputFlags
        {
            Move = 0x0001,
            LeftDown = 0x0002,
            LeftUp = 0x0004,
            RightDown = 0x0008,
            RightUp = 0x0010,
            MiddleDown = 0x0020,
            MiddleUp = 0x0040,
            XDown = 0x0080,
            XUp = 0x0100,
            Wheel = 0x0800,
            Absolute = 0x8000,
        };

        // Importing various Win32 APIs that we need for input
        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        internal static extern int GetSystemMetrics(int nIndex);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern int MapVirtualKey(int nVirtKey, int nMapType);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern int SendInput(int nInputs, ref Input mi, int cbSize);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern short VkKeyScan(char ch);

        #endregion
    }


    /// <summary>
    /// Exposes a simple interface to common mouse operations, allowing the user to simulate mouse input.
    /// </summary>
    public static class MouseInput
    {
        /// <summary>
        /// Clicks a mouse button.
        /// </summary>
        /// <param name="mouseButton">The mouse button to click.</param>
        public static void Click(MouseButton mouseButton = MouseButton.Left)
        {
            Down(mouseButton);
            Up(mouseButton);
        }

        /// <summary>
        /// Double-clicks a mouse button.
        /// </summary>
        /// <param name="mouseButton">The mouse button to click.</param>
        public static void DoubleClick(MouseButton mouseButton = MouseButton.Left)
        {
            Click(mouseButton);
            Click(mouseButton);
        }

        /// <summary>
        /// Performs a mouse-down operation for a specified mouse button.
        /// </summary>
        /// <param name="mouseButton">The mouse button to use.</param>
        public static void Down(MouseButton mouseButton = MouseButton.Left)
        {
            switch (mouseButton)
            {
                case MouseButton.Left:
                    SendMouseInput(0, 0, 0, NativeMethods.SendMouseInputFlags.LeftDown);
                    break;
                case MouseButton.Right:
                    SendMouseInput(0, 0, 0, NativeMethods.SendMouseInputFlags.RightDown);
                    break;
                case MouseButton.Middle:
                    SendMouseInput(0, 0, 0, NativeMethods.SendMouseInputFlags.MiddleDown);
                    break;
                case MouseButton.XButton1:
                    SendMouseInput(0, 0, NativeMethods.XButton1, NativeMethods.SendMouseInputFlags.XDown);
                    break;
                case MouseButton.XButton2:
                    SendMouseInput(0, 0, NativeMethods.XButton2, NativeMethods.SendMouseInputFlags.XDown);
                    break;
                default:
                    throw new InvalidOperationException("Unsupported MouseButton input.");
            }
        }

        /// <summary>
        /// Moves the mouse pointer to the specified screen coordinates.
        /// </summary>
        /// <param name="point">The screen coordinates to move to.</param>
        public static void MoveTo(Point point)
        {
            SendMouseInput(point.X, point.Y, 0, NativeMethods.SendMouseInputFlags.Move | NativeMethods.SendMouseInputFlags.Absolute);
        }

        /// <summary>
        /// Resets the system mouse to a clean state.
        /// </summary>
        public static void Reset()
        {
            MoveTo(new Point(0, 0));

            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                SendMouseInput(0, 0, 0, NativeMethods.SendMouseInputFlags.LeftUp);
            }

            if (Mouse.MiddleButton == MouseButtonState.Pressed)
            {
                SendMouseInput(0, 0, 0, NativeMethods.SendMouseInputFlags.MiddleUp);
            }

            if (Mouse.RightButton == MouseButtonState.Pressed)
            {
                SendMouseInput(0, 0, 0, NativeMethods.SendMouseInputFlags.RightUp);
            }

            if (Mouse.XButton1 == MouseButtonState.Pressed)
            {
                SendMouseInput(0, 0, NativeMethods.XButton1, NativeMethods.SendMouseInputFlags.XUp);
            }

            if (Mouse.XButton2 == MouseButtonState.Pressed)
            {
                SendMouseInput(0, 0, NativeMethods.XButton2, NativeMethods.SendMouseInputFlags.XUp);
            }
        }

        /// <summary>
        /// Simulates scrolling of the mouse wheel up or down.
        /// </summary>
        /// <param name="lines">The number of lines to scroll. Use positive numbers to scroll up and negative numbers to scroll down.</param>
        public static void Scroll(double lines)
        {
            var amount = (int)(NativeMethods.WheelDelta * lines);

            SendMouseInput(0, 0, amount, NativeMethods.SendMouseInputFlags.Wheel);
        }

        /// <summary>
        /// Performs a mouse-up operation for a specified mouse button.
        /// </summary>
        /// <param name="mouseButton">The mouse button to use.</param>
        public static void Up(MouseButton mouseButton)
        {
            switch (mouseButton)
            {
                case MouseButton.Left:
                    SendMouseInput(0, 0, 0, NativeMethods.SendMouseInputFlags.LeftUp);
                    break;
                case MouseButton.Right:
                    SendMouseInput(0, 0, 0, NativeMethods.SendMouseInputFlags.RightUp);
                    break;
                case MouseButton.Middle:
                    SendMouseInput(0, 0, 0, NativeMethods.SendMouseInputFlags.MiddleUp);
                    break;
                case MouseButton.XButton1:
                    SendMouseInput(0, 0, NativeMethods.XButton1, NativeMethods.SendMouseInputFlags.XUp);
                    break;
                case MouseButton.XButton2:
                    SendMouseInput(0, 0, NativeMethods.XButton2, NativeMethods.SendMouseInputFlags.XUp);
                    break;
                default:
                    throw new InvalidOperationException("Unsupported MouseButton input.");
            }
        }

        /// <summary>
        /// Sends mouse input.
        /// </summary>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        /// <param name="data">scroll wheel amount</param>
        /// <param name="flags">SendMouseInputFlags flags</param>

        private static void SendMouseInput(double x, double y, int data, NativeMethods.SendMouseInputFlags flags)
        {
            var intflags = (int)flags;

            if ((intflags & (int)NativeMethods.SendMouseInputFlags.Absolute) != 0)
            {
                // Absolute position requires normalized coordinates.
                NormalizeCoordinates(ref x, ref y);
                intflags |= NativeMethods.MouseeventfVirtualdesk;
            }

            var mi = new NativeMethods.Input
            {
                type = NativeMethods.InputMouse
            };
            mi.union.mouseInput.dx = (int)x;
            mi.union.mouseInput.dy = (int)y;
            mi.union.mouseInput.mouseData = data;
            mi.union.mouseInput.dwFlags = intflags;
            mi.union.mouseInput.time = 0;
            mi.union.mouseInput.dwExtraInfo = new IntPtr(0);

            if (NativeMethods.SendInput(1, ref mi, Marshal.SizeOf(mi)) == 0)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        private static void NormalizeCoordinates(ref double x, ref double y)
        {
            var vScreenWidth = NativeMethods.GetSystemMetrics(NativeMethods.SmCxvirtualscreen);
            var vScreenHeight = NativeMethods.GetSystemMetrics(NativeMethods.SmCyvirtualscreen);
            var vScreenLeft = NativeMethods.GetSystemMetrics(NativeMethods.SmXvirtualscreen);
            var vScreenTop = NativeMethods.GetSystemMetrics(NativeMethods.SmYvirtualscreen);

            // Absolute input requires that input is in 'normalized' coords - with the entire
            // desktop being (0,0)...(65536,65536). Need to convert input x,y coords to this
            // first.
            //
            // In this normalized world, any pixel on the screen corresponds to a block of values
            // of normalized coords - eg. on a 1024x768 screen,
            // y pixel 0 corresponds to range 0 to 85.333,
            // y pixel 1 corresponds to range 85.333 to 170.666,
            // y pixel 2 correpsonds to range 170.666 to 256 - and so on.
            // Doing basic scaling math - (x-top)*65536/Width - gets us the start of the range.
            // However, because int math is used, this can end up being rounded into the wrong
            // pixel. For example, if we wanted pixel 1, we'd get 85.333, but that comes out as
            // 85 as an int, which falls into pixel 0's range - and that's where the pointer goes.
            // To avoid this, we add on half-a-"screen pixel"'s worth of normalized coords - to
            // push us into the middle of any given pixel's range - that's the 65536/(Width*2)
            // part of the formula. So now pixel 1 maps to 85+42 = 127 - which is comfortably
            // in the middle of that pixel's block.
            // The key ting here is that unlike points in coordinate geometry, pixels take up
            // space, so are often better treated like rectangles - and if you want to target
            // a particular pixel, target its rectangle's midpoint, not its edge.
            x = (x - vScreenLeft) * 65536d / vScreenWidth /*+ 65536d / (vScreenWidth * 2)*/;
            y = (y - vScreenTop) * 65536d / vScreenHeight /*+ 65536d / (vScreenHeight * 2)*/;
        }
    }

    /// <summary>
    /// Exposes a simple interface to common keyboard operations, allowing the user to simulate keyboard input.
    /// </summary>
    /// 
    /// The following code types "Hello world" with the specified casing,
    /// and then types "hello, capitalized world" which will be in all caps because
    /// the left shift key is being held down.
    public static class KeyboardInput
    {
        #region Public Members

        /// <summary>
        /// Presses down a key.
        /// </summary>
        /// <param name="key">The key to press.</param>
        public static void Press(Key key)
        {
            SendKeyboardInput(key, true);
        }

        /// <summary>
        /// Releases a key.
        /// </summary>
        /// <param name="key">The key to release.</param>
        public static void Release(Key key)
        {
            SendKeyboardInput(key, false);
        }

        /// <summary>
        /// Resets the system keyboard to a clean state.
        /// </summary>
        public static void Reset()
        {
            foreach (Key key in Enum.GetValues(typeof(Key)))
            {
                if (key != Key.None && (Keyboard.GetKeyStates(key) & KeyStates.Down) > 0)
                {
                    Release(key);
                }
            }
        }

        /// <summary>
        /// Performs a press-and-release operation for the specified key, which is effectively equivallent to typing.
        /// </summary>
        /// <param name="key">The key to press.</param>
        public static void Type(Key key)
        {
            Press(key);
            Release(key);
        }

        /// <summary>
        /// Types the specified text.
        /// </summary>
        /// <param name="text">The text to type.</param>
        public static void Type(string text)
        {
            foreach (var c in text)
            {
                // We get the vKey value for the character via a Win32 API. We then use bit masks to pull the
                // upper and lower bytes to get the shift state and key information. We then use WPF KeyInterop
                // to go from the vKey key info into a System.Windows.Input.Key data structure. This work is
                // necessary because Key doesn't distinguish between upper and lower case, so we have to wrap
                // the key type inside a shift press/release if necessary.
                int vKeyValue = NativeMethods.VkKeyScan(c);
                var keyIsShifted = (vKeyValue & NativeMethods.VKeyShiftMask) == NativeMethods.VKeyShiftMask;
                var key = KeyInterop.KeyFromVirtualKey(vKeyValue & NativeMethods.VKeyCharMask);

                if (keyIsShifted)
                {
                    Type(key, new[] { Key.LeftShift });
                }
                else
                {
                    Type(key);
                }
            }
        }

        #endregion

        #region Private Members

        /// <summary>
        /// Types a key while a set of modifier keys are being pressed. Modifer keys
        /// are pressed in the order specified and released in reverse order.
        /// </summary>
        /// <param name="key">Key to type.</param>
        /// <param name="modifierKeys">Set of keys to hold down with key is typed.</param>
        private static void Type(Key key, Key[] modifierKeys)
        {
            foreach (var modiferKey in modifierKeys)
            {
                Press(modiferKey);
            }

            Type(key);

            foreach (var modifierKey in modifierKeys.Reverse())
            {
                Release(modifierKey);
            }
        }

        /// <summary>
        /// Injects keyboard input into the system.
        /// </summary>
        /// <param name="key">Indicates the key pressed or released. Can be one of the constants defined in the Key enum.</param>
        /// <param name="press">True to inject a key press, false to inject a key release.</param>
        private static void SendKeyboardInput(Key key, bool press)
        {
            var ki = new NativeMethods.Input
            {
                type = NativeMethods.InputKeyboard
            };

            ki.union.keyboardInput.wVk = (short)KeyInterop.VirtualKeyFromKey(key);
            ki.union.keyboardInput.wScan = (short)NativeMethods.MapVirtualKey(ki.union.keyboardInput.wVk, 0);

            var dwFlags = 0;

            if (ki.union.keyboardInput.wScan > 0)
            {
                dwFlags |= NativeMethods.KeyeventfScancode;
            }

            if (!press)
            {
                dwFlags |= NativeMethods.KeyeventfKeyup;
            }

            ki.union.keyboardInput.dwFlags = dwFlags;

            if (ExtendedKeys.Contains(key))
            {
                ki.union.keyboardInput.dwFlags |= NativeMethods.KeyeventfExtendedkey;
            }

            ki.union.keyboardInput.time = 0;
            ki.union.keyboardInput.dwExtraInfo = new IntPtr(0);

            if (NativeMethods.SendInput(1, ref ki, Marshal.SizeOf(ki)) == 0)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        // From the SDK:
        // The extended-key flag indicates whether the keystroke message originated from one of
        // the additional keys on the enhanced keyboard. The extended keys consist of the ALT and
        // CTRL keys on the right-hand side of the keyboard; the INS, DEL, HOME, END, PAGE UP,
        // PAGE DOWN, and arrow keys in the clusters to the left of the numeric keypad; the NUM LOCK
        // key; the BREAK (CTRL+PAUSE) key; the PRINT SCRN key; and the divide (/) and ENTER keys in
        // the numeric keypad. The extended-key flag is set if the key is an extended key. 
        //
        // - docs appear to be incorrect. Use of Spy++ indicates that break is not an extended key.
        // Also, menu key and windows keys also appear to be extended.
        private static readonly Key[] ExtendedKeys = 
        {
            Key.RightAlt,
            Key.RightCtrl,
            Key.NumLock,
            Key.Insert,
            Key.Delete,
            Key.Home,
            Key.End,
            Key.Prior,
            Key.Next,
            Key.Up,
            Key.Down,
            Key.Left,
            Key.Right,
            Key.Apps,
            Key.RWin,
            Key.LWin
        };
        // Note that there are no distinct values for the following keys:
        // numpad divide
        // numpad enter

        #endregion
    }

#pragma warning restore IDE1006 // Naming Styles
}
