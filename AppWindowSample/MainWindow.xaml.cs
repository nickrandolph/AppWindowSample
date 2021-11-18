using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics;
using Windows.Graphics.Display;
using AppWindowCore;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AppWindowSample
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        AppWindow appWindow;
        public MainWindow()
        {
            this.InitializeComponent();

            appWindow = this.GetAppWindow();

        }

        private void PositionWindowClick(object sender, RoutedEventArgs e)
        {
            var row = Grid.GetRow(sender as Button);
            var col = Grid.GetColumn(sender as Button);

            appWindow.Reposition(row, col);
        }

        private void ChangeIconClick(object sender, RoutedEventArgs e)
        {
            appWindow.ChangeIcon("icon1.ico");
        }

        private void ChangePresenterClick(object sender, RoutedEventArgs e)
        {
            if (Enum.TryParse<AppWindowPresenterKind>((sender as Button).Content.ToString(), out var presenterKind))
            {
                appWindow.SetPresenter(presenterKind);
            }
        }
    }


}
