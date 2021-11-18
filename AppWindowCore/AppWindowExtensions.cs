using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Forms;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Windows.Graphics;
using System.Reflection;
using System.IO;

namespace AppWindowCore;

public static class AppWindowExtensions
{
    public static AppWindow GetAppWindow(this Microsoft.UI.Xaml.Window window)
    {
        IntPtr windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(window);

        return GetAppWindowFromWindowHandle(windowHandle);
    }
    public static AppWindow GetAppWindow(this Window wpfWindow)
    {
        // Get the HWND of the top level WPF window.
        var helper = new WindowInteropHelper(wpfWindow);
        IntPtr hwnd = (HwndSource.FromHwnd(helper.EnsureHandle())
            as System.Windows.Interop.IWin32Window).Handle;

        return GetAppWindowFromWindowHandle(hwnd);
    }
    public static AppWindow GetAppWindow(this Form winForm)
    {
        return GetAppWindowFromWindowHandle(winForm.Handle);
    }

    private static AppWindow GetAppWindowFromWindowHandle(IntPtr windowHandle)
    {
        WindowId windowId;
        windowId = Win32Interop.GetWindowIdFromWindow(windowHandle);

        return AppWindow.GetFromWindowId(windowId);
    }

    public static void Reposition(this AppWindow appWindow, int row, int col)
    {
        IntPtr hwndDesktop = PInvoke.User32.GetDesktopWindow();
        PInvoke.RECT rectParent;
        PInvoke.User32.GetClientRect(hwndDesktop, out rectParent);

        var width = (int)(rectParent.right - rectParent.left) / 3;
        var height = (int)(rectParent.bottom - rectParent.top) / 2;


        var winPosition = new RectInt32(width * col, height * row, width, height);

        appWindow.MoveAndResize(winPosition);
    }

    public static void ChangeIcon(this AppWindow appWindow, string iconFileName)
    {
        var path = Assembly.GetExecutingAssembly().Location;
        var filePath = Path.Combine(Path.GetDirectoryName(path), iconFileName);
        appWindow.SetIcon(filePath);

    }
}
