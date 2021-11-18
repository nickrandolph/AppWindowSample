using AppWindowCore;
using Microsoft.UI.Windowing;
using Microsoft.Windows.ApplicationModel.DynamicDependency;
using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace AppWindowsWPF
{
    public partial class MainWindow : Window
    {
        AppWindow appWindow; 
        public MainWindow()
        {
            InitializeComponent();

            var success = Bootstrap.TryInitialize(0x00010000, out _);
            if (!success)
            {
                MessageBox.Show("Unable to initialize Windows App SDK - Make sure it's installed");
                return;
            }

            appWindow = this.GetAppWindow();

        }

        private void PositionWindowClick(object sender, RoutedEventArgs e)
        {
            var row = Grid.GetRow(sender as Button);
            var col = Grid.GetColumn(sender as Button);

            appWindow.Reposition(row, col);
        }
        protected override void OnClosed(System.EventArgs e)
        {
            base.OnClosed(e);

            Bootstrap.Shutdown();
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
