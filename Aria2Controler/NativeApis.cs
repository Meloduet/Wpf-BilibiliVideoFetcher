using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Aria2Controler
{
    public struct POINT
    {
        public int X;

        public int Y;

        public POINT(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }

    public enum WindowShowStyle : uint
    {
        Hide,
        ShowNormal,
        ShowMinimized,
        ShowMaximized,
        Maximize = 3u,
        ShowNormalNoActivate,
        Show,
        Minimize,
        ShowMinNoActivate,
        ShowNoActivate,
        Restore,
        ShowDefault,
        ForceMinimized
    }
    internal static class NativeApis
    {
        public const int SW_HIDE = 0;

        public const int SW_SHOWNORMAL = 1;

        public const int SW_NORMAL = 1;

        public const int SW_SHOWMINIMIZED = 2;

        public const int SW_SHOWMAXIMIZED = 3;

        public const int SW_MAXIMIZE = 3;

        public const int SW_SHOWNOACTIVATE = 4;

        public const int SW_SHOW = 5;

        public const int SW_MINIMIZE = 6;

        public const int SW_SHOWMINNOACTIVE = 7;

        public const int SW_SHOWNA = 8;

        public const int SW_RESTORE = 9;

        public const int SW_SHOWDEFAULT = 10;

        public const int SW_FORCEMINIMIZE = 11;

        public const int SW_MAX = 11;

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern bool OpenIcon(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(POINT p);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);

        public static bool ShowWindow(IntPtr hWnd, WindowShowStyle nCmdShow)
        {
            return NativeApis.ShowWindow(hWnd, (int)nCmdShow);
        }

        public static bool HideWindow(IntPtr hWnd)
        {
            return NativeApis.ShowWindow(hWnd, 0);
        }

        public static string GetText(IntPtr hWnd)
        {
            StringBuilder stringBuilder = new StringBuilder(NativeApis.GetWindowTextLength(hWnd) + 1);
            NativeApis.GetWindowText(hWnd, stringBuilder, stringBuilder.Capacity);
            return stringBuilder.ToString();
        }

        public static IntPtr GetTopLevelWindow(IntPtr hWnd)
        {
            IntPtr parent;
            while ((parent = NativeApis.GetParent(hWnd)) != IntPtr.Zero)
            {
                hWnd = parent;
            }
            return hWnd;
        }
    }
}
