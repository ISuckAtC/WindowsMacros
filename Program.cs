using System;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Macros
{

    class Program
    {
        #region Setup
        [StructLayout(LayoutKind.Sequential)]
        struct KEYBDINPUT
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public uint time;
            public long dwExtraInfo;
        }
        [StructLayout(LayoutKind.Sequential)]
        struct MOUSEINPUT
        {
            public int dx, dy;
            public uint mouseData, dwFlags, time;
            public long dwExtraInfo;
        }
        [StructLayout(LayoutKind.Explicit)]
        struct INPUTTYPES
        {
            [FieldOffset(0)]
            public MOUSEINPUT Mouse;
            [FieldOffset(0)]
            public KEYBDINPUT Keyboard;
        }
        [StructLayout(LayoutKind.Sequential)]
        struct INPUT
        {
            public uint type;
            public INPUTTYPES inputData;
        }

        [DllImport("User32.dll", SetLastError = true)]
        static extern uint SendInput(int inputCount, INPUT[] inputs, int size);

        [DllImport("User32.dll", SetLastError = true)]
        static extern short GetKeyState(int nVirtKey);
        [DllImport("User32.dll", SetLastError = true)]
        static extern bool GetCursorPos(long lpPoint);

        [DllImport("User32.dll", SetLastError = true)]
        static extern bool SetCursorPos(int x, int y);

        public struct POINT
        {
            public int x, y;
        }


        static void SendKey(ScanCodes key, bool up = false)
        {
            KEYBDINPUT kbi = new KEYBDINPUT();
            kbi.wScan = (ushort)key;
            kbi.dwFlags = 1 << 3 | (uint)(up ? 1 << 1 : 0);
            INPUT input = new INPUT();
            input.inputData.Keyboard = kbi;
            input.type = 1;
            unsafe
            {
                SendInput(1, new INPUT[] { input }, sizeof(INPUT));
                lastErr = Marshal.GetLastWin32Error();
            }
            //if (lastErr > 0) Console.WriteLine("ERROR: " + lastErr + " | key: " + key);
        }

        static void ClickMouse()
        {
            MoveMouse(0, 0, true, false);
        }

        static void ReleaseMouse()
        {
            MoveMouse(0, 0, false, true);
        }

        static void MoveMouse(int x, int y, bool click = false, bool release = false)
        {
            MOUSEINPUT mi = new MOUSEINPUT();
            mi.dx = (int)((float)x / (1920f / 1280f));
            mi.dy = (int)((float)y / (1920f / 1280f));
            mi.dwFlags = 1;
            if (click) mi.dwFlags = mi.dwFlags | 2;
            if (release) mi.dwFlags = mi.dwFlags | 4;
            INPUT input = new INPUT();
            input.type = 0;
            input.inputData.Mouse = mi;

            unsafe
            {
                SendInput(1, new INPUT[] { input }, sizeof(INPUT));
                lastErr = Marshal.GetLastWin32Error();
            }
            if (lastErr > 0) Console.WriteLine("ERROR: " + lastErr);
        }

        static public int lastErr;

        static public List<WinVirtualKey> pressedKeys = new List<WinVirtualKey>();


        public enum ScanCodes
        {
            Escape = 0x01,
            One = 0x02,
            Two = 0x03,
            Three = 0x04,
            Four = 0x05,
            Five = 0x06,
            Six = 0x07,
            Seven = 0x08,
            Eight = 0x09,
            Nine = 0x0A,
            Zero = 0x0B,
            Minus = 0x0C,
            Equals = 0x0D,
            Backspace = 0x0E,
            Tab = 0x0F,
            Q = 0x10,
            W = 0x11,
            E = 0x12,
            R = 0x13,
            T = 0x14,
            Y = 0x15,
            U = 0x16,
            I = 0x17,
            O = 0x18,
            P = 0x19,
            LeftBracket = 0x1A,
            RightBracket = 0x1B,
            Enter = 0x1C,
            LeftCtrl = 0x1D,
            A = 0x1E,
            S = 0x1F,
            D = 0x20,
            F = 0x21,
            G = 0x22,
            H = 0x23,
            J = 0x24,
            K = 0x25,
            L = 0x26,
            Semicolon = 0x27,
            Apostrophe = 0x28,
            Grave = 0x29,
            LeftShift = 0x2A,
            Backslash = 0x2B,
            Z = 0x2C,
            X = 0x2D,
            C = 0x2E,
            V = 0x2F,
            B = 0x30,
            N = 0x31,
            M = 0x32,
            Comma = 0x33,
            Period = 0x34,
            Slash = 0x35,
            RightShift = 0x36,
            NumPadMultiply = 0x37,
            LeftAlt = 0x38,
            Space = 0x39,
            Caps = 0x3A,
            F1 = 0x3B,
            F2 = 0x3C,
            F3 = 0x3D,
            F4 = 0x3E,
            F5 = 0x3F,
            F6 = 0x40,
            F7 = 0x41,
            F8 = 0x42,
            F9 = 0x43,
            F10 = 0x44,
            NumLock = 0x45,
            ScrollLock = 0x46,
            NumPad7 = 0x47,
            NumPad8 = 0x48,
            NumPad9 = 0x49,
            NumPadMinus = 0x4A,
            NumPad4 = 0x4B,
            NumPad5 = 0x4C,
            NumPad6 = 0x4D,
            NumPadPlus = 0x4E,
            NumPad1 = 0x4F,
            NumPad2 = 0x50,
            NumPad3 = 0x51,
            NumPad0 = 0x52,
            NumPadPeriod = 0x53
        }

        public enum WinVirtualKey
        {
            VK_LBUTTON = 0x01,
            VK_RBUTTON = 0x02,
            VK_CANCEL = 0x03,
            VK_MBUTTON = 0x04,
            VK_XBUTTON1 = 0x05,
            VK_XBUTTON2 = 0x06,
            VK_BACK = 0x08,
            VK_TAB = 0x09,
            VK_CLEAR = 0x0C,
            VK_RETURN = 0x0D,
            VK_SHIFT = 0x10,
            VK_CONTROL = 0x11,
            VK_MENU = 0x12,
            VK_PAUSE = 0x13,
            VK_CAPITAL = 0x14,
            VK_KANA = 0x15,
            VK_HANGUL = 0x15,
            VK_JUNJA = 0x17,
            VK_FINAL = 0x18,
            VK_HANJA = 0x19,
            VK_KANJI = 0x19,
            VK_ESCAPE = 0x1B,
            VK_CONVERT = 0x1C,
            VK_NONCONVERT = 0x1D,
            VK_ACCEPT = 0x1E,
            VK_MODECHANGE = 0x1F,
            VK_SPACE = 0x20,
            VK_PRIOR = 0x21,
            VK_NEXT = 0x22,
            VK_END = 0x23,
            VK_HOME = 0x24,
            VK_LEFT = 0x25,
            VK_UP = 0x26,
            VK_RIGHT = 0x27,
            VK_DOWN = 0x28,
            VK_SELECT = 0x29,
            VK_PRINT = 0x2A,
            VK_EXECUTE = 0x2B,
            VK_SNAPSHOT = 0x2C,
            VK_INSERT = 0x2D,
            VK_DELETE = 0x2E,
            VK_HELP = 0x2F,
            VK_LWIN = 0x5B,
            VK_RWIN = 0x5C,
            VK_APPS = 0x5D,
            VK_SLEEP = 0x5F,
            VK_NUMPAD0 = 0x60,
            VK_NUMPAD1 = 0x61,
            VK_NUMPAD2 = 0x62,
            VK_NUMPAD3 = 0x63,
            VK_NUMPAD4 = 0x64,
            VK_NUMPAD5 = 0x65,
            VK_NUMPAD6 = 0x66,
            VK_NUMPAD7 = 0x67,
            VK_NUMPAD8 = 0x68,
            VK_NUMPAD9 = 0x69,
            VK_MULTIPLY = 0x6A,
            VK_ADD = 0x6B,
            VK_SEPARATOR = 0x6C,
            VK_SUBTRACT = 0x6D,
            VK_DECIMAL = 0x6E,
            VK_DIVIDE = 0x6F,
            VK_F1 = 0x70,
            VK_F2 = 0x71,
            VK_F3 = 0x72,
            VK_F4 = 0x73,
            VK_F5 = 0x74,
            VK_F6 = 0x75,
            VK_F7 = 0x76,
            VK_F8 = 0x77,
            VK_F9 = 0x78,
            VK_F10 = 0x79,
            VK_F11 = 0x7A,
            VK_F12 = 0x7B,
            VK_F13 = 0x7C,
            VK_F14 = 0x7D,
            VK_F15 = 0x7E,
            VK_F16 = 0x7F,
            VK_F17 = 0x80,
            VK_F18 = 0x81,
            VK_F19 = 0x82,
            VK_F20 = 0x83,
            VK_F21 = 0x84,
            VK_F22 = 0x85,
            VK_F23 = 0x86,
            VK_F24 = 0x87,
            VK_NUMLOCK = 0x90,
            VK_SCROLL = 0x91,
            VK_OEM_NEC_EQUAL = 0x92,
            VK_OEM_FJ_JISHO = 0x92,
            VK_OEM_FJ_MASSHOU = 0x93,
            VK_OEM_FJ_TOUROKU = 0x94,
            VK_OEM_FJ_LOYA = 0x95,
            VK_OEM_FJ_ROYA = 0x96,
            VK_LSHIFT = 0xA0,
            VK_RSHIFT = 0xA1,
            VK_LCONTROL = 0xA2,
            VK_RCONTROL = 0xA3,
            VK_LMENU = 0xA4,
            VK_RMENU = 0xA5,
            VK_BROWSER_BACK = 0xA6,
            VK_BROWSER_FORWARD = 0xA7,
            VK_BROWSER_REFRESH = 0xA8,
            VK_BROWSER_STOP = 0xA9,
            VK_BROWSER_SEARCH = 0xAA,
            VK_BROWSER_FAVORITES = 0xAB,
            VK_BROWSER_HOME = 0xAC,
            VK_VOLUME_MUTE = 0xAD,
            VK_VOLUME_DOWN = 0xAE,
            VK_VOLUME_UP = 0xAF,
            VK_MEDIA_NEXT_TRACK = 0xB0,
            VK_MEDIA_PREV_TRACK = 0xB1,
            VK_MEDIA_STOP = 0xB2,
            VK_MEDIA_PLAY_PAUSE = 0xB3,
            VK_LAUNCH_MAIL = 0xB4,
            VK_LAUNCH_MEDIA_SELECT = 0xB5,
            VK_LAUNCH_APP1 = 0xB6,
            VK_LAUNCH_APP2 = 0xB7,
            VK_OEM_1 = 0xBA,
            VK_OEM_PLUS = 0xBB,
            VK_OEM_COMMA = 0xBC,
            VK_OEM_MINUS = 0xBD,
            VK_OEM_PERIOD = 0xBE,
            VK_OEM_2 = 0xBF,
            VK_OEM_3 = 0xC0,
            VK_OEM_4 = 0xDB,
            VK_OEM_5 = 0xDC,
            VK_OEM_6 = 0xDD,
            VK_OEM_7 = 0xDE,
            VK_OEM_8 = 0xDF,
            VK_OEM_AX = 0xE1,
            VK_OEM_102 = 0xE2,
            VK_ICO_HELP = 0xE3,
            VK_ICO_00 = 0xE4,
            VK_PROCESSKEY = 0xE5,
            VK_ICO_CLEAR = 0xE6,
            VK_PACKET = 0xE7,
            VK_OEM_RESET = 0xE9,
            VK_OEM_JUMP = 0xEA,
            VK_OEM_PA1 = 0xEB,
            VK_OEM_PA2 = 0xEC,
            VK_OEM_PA3 = 0xED,
            VK_OEM_WSCTRL = 0xEE,
            VK_OEM_CUSEL = 0xEF,
            VK_OEM_ATTN = 0xF0,
            VK_OEM_FINISH = 0xF1,
            VK_OEM_COPY = 0xF2,
            VK_OEM_AUTO = 0xF3,
            VK_OEM_ENLW = 0xF4,
            VK_OEM_BACKTAB = 0xF5,
            VK_ATTN = 0xF6,
            VK_CRSEL = 0xF7,
            VK_EXSEL = 0xF8,
            VK_EREOF = 0xF9,
            VK_PLAY = 0xFA,
            VK_ZOOM = 0xFB,
            VK_NONAME = 0xFC,
            VK_PA1 = 0xFD,
            VK_OEM_CLEAR = 0xFE,
            VK_ZERO = 0x30,
            VK_ONE = 0x31,
            VK_TWO = 0x32,
            VK_THREE = 0x33,
            VK_FOUR = 0x34,
            VK_FIVE = 0x35,
            VK_SIX = 0x36,
            VK_SEVEN = 0x37,
            VK_EIGHT = 0x38,
            VK_NINE = 0x39,
            VK_A = 0x41,
            VK_B = 0x42,
            VK_C = 0x43,
            VK_D = 0x44,
            VK_E = 0x45,
            VK_F = 0x46,
            VK_G = 0x47,
            VK_H = 0x48,
            VK_I = 0x49,
            VK_J = 0x4A,
            VK_K = 0x4B,
            VK_L = 0x4C,
            VK_M = 0x4D,
            VK_N = 0x4E,
            VK_O = 0x4F,
            VK_P = 0x50,
            VK_Q = 0x51,
            VK_R = 0x52,
            VK_S = 0x53,
            VK_T = 0x54,
            VK_U = 0x55,
            VK_V = 0x56,
            VK_W = 0x57,
            VK_X = 0x58,
            VK_Y = 0x59,
            VK_Z = 0x5A
        }

        static bool GetKeyDown(WinVirtualKey vk)
        {
            if (!pressedKeys.Contains(vk) && (GetKeyState((int)vk) & 0x80) == 0x80)
            {
                pressedKeys.Add(vk);
                return true;
            }
            return false;
        }

        static bool GetKey(WinVirtualKey vk)
        {
            return (GetKeyState((int)vk) & 0x80) == 0x80;
        }

        static bool GetKeyUp(WinVirtualKey vk)
        {
            if (pressedKeys.Contains(vk) && (GetKeyState((int)vk) & 0x80) != 0x00)
            {
                return true;
            }
            return false;
        }

        static POINT GetMousePosition()
        {
            unsafe
            {
                POINT p;
                GetCursorPos((long)&p);
                return p;
            }
        }

        static int RefreshKeys()
        {
            int count = 0;
            WinVirtualKey[] pressed = pressedKeys.ToArray();
            for (int i = 0; i < pressed.Length; i++)
            {
                if ((GetKeyState((int)pressed[i]) & 0x80) != 0x80)
                {
                    count++;
                    pressedKeys.Remove(pressed[i]);
                }
            }
            return count;
        }

        static void Wait(int milliseconds)
        {
            Thread.Sleep(milliseconds);
        }

        #endregion

        static void Main(string[] args)
        {
            bool exit = false;
            while (!exit)
            {

                // PUT YOUR CODE HERE
                
                // Example macro

                if (GetKeyDown(WinVirtualKey.VK_ONE))
                {
                    Console.WriteLine("The number 1 key was pressed");

                    // Move the mouse to the middle of the screen and click
                    SetCursorPos(960, 540);
                    ClickMouse();
                    Wait(17);
                    ClickMouse();
                    Wait(17);
                    ClickMouse();
                    Wait(17);

                    // Macro typing out the word "Hello"
                    SendKey(ScanCodes.H);
                    Wait(5);
                    SendKey(ScanCodes.E);
                    Wait(5);
                    SendKey(ScanCodes.L);
                    Wait(5);
                    SendKey(ScanCodes.L);
                    Wait(5);
                    SendKey(ScanCodes.O);
                }

                if (GetKeyDown(WinVirtualKey.VK_ESCAPE))
                {
                    Console.WriteLine("The escape key was pressed");

                    exit = true;
                }


                RefreshKeys();   
            }
        }
    }
}
