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
    public static AppWindow GetAppWindowForWinUI(this Microsoft.UI.Xaml.Window window)
    {
        var windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(window);

        return GetAppWindowFromWindowHandle(windowHandle);
    }
    public static AppWindow GetAppWindowForWPF(this Window window)
    {
        var hwnd = new WindowInteropHelper(window).EnsureHandle();
        return GetAppWindowFromWindowHandle(hwnd);
    }
    public static AppWindow GetAppWindowForWinForms(this Form window)
    {
        return GetAppWindowFromWindowHandle(window.Handle);
    }

    private static AppWindow GetAppWindowFromWindowHandle(IntPtr windowHandle)
    {
        var windowId = Win32Interop.GetWindowIdFromWindow(windowHandle);
        return AppWindow.GetFromWindowId(windowId);
    }

    public static void Reposition(this AppWindow appWindow, int row, int col, int totalRows = 2, int totalColumns = 3)
    {
        IntPtr hwndDesktop = PInvoke.User32.GetDesktopWindow();
        PInvoke.RECT rectParent;
        PInvoke.User32.GetClientRect(hwndDesktop, out rectParent);

        var width = (int)(rectParent.right - rectParent.left) / totalColumns;
        var height = (int)(rectParent.bottom - rectParent.top) / totalRows;


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
