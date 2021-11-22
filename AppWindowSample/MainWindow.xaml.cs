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
using Windows.UI;

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

        private void OverlappedPresenterPropertyCheckChanged(object sender, RoutedEventArgs e)
        {
            var presenter = appWindow.Presenter as OverlappedPresenter;
            if (presenter is null)
            {
                return;
            }

            var check = sender as CheckBox;
            var propertyName = check.Content as string;
            var property = typeof(OverlappedPresenter).GetProperty(propertyName);
            property.SetValue(presenter, check.IsChecked);
        }

        private void OverlappedPresenterTitleBarAndBorderCheckChanged(object sender, RoutedEventArgs e)
        {
            var presenter = appWindow.Presenter as OverlappedPresenter;
            if (presenter is null)
            {
                return;
            }
            var hasBorder = HasBorderCheckBox.IsChecked ?? true;
            var hasTitleBar = HasTitleBarCheckBox.IsChecked ?? true;
            presenter.SetBorderAndTitleBar(hasBorder, hasTitleBar);
        }

        private Random rnd { get; } = new Random();

        private Color GetRandomColor()
        {
            return Color.FromArgb(
                (byte)rnd.Next(0, 255),
                (byte)rnd.Next(0, 255),
                (byte)rnd.Next(0, 255),
                (byte)rnd.Next(0, 255));
        }
        private void TitleBarRandomColorClick(object sender, RoutedEventArgs e)
        {
            var property = typeof(AppWindowTitleBar).GetProperty((sender as Button).Content.ToString());
            var clr = GetRandomColor();
            property.SetValue(appWindow.TitleBar, clr);
        }

        private void ChangeIconAndMenuClick(object sender, RoutedEventArgs e)
        {

            if (Enum.TryParse<IconShowOptions>((sender as Button).Content.ToString(), out var showOptions))
            {
                appWindow.TitleBar.IconShowOptions = showOptions;
            }
        }

        private void ToggleClientAreaChanged(object sender, RoutedEventArgs e)
        {
            appWindow.TitleBar.ExtendsContentIntoTitleBar = (sender as CheckBox).IsChecked??false;
        }

        private void SetDragAreaClick(object sender, RoutedEventArgs e)
        {
            
            var newHeight = (int)rnd.Next(0, 200);
            DragAreaBorder.Height = newHeight;
            DragAreaBorder.Background = new SolidColorBrush(GetRandomColor());
            appWindow.TitleBar.SetDragRectangles(new[] {
                new RectInt32(0,0, 
                    (int)(DragAreaBorder.ActualWidth*this.Content.XamlRoot.RasterizationScale), 
                    (int)(newHeight*this.Content.XamlRoot.RasterizationScale) + (appWindow.TitleBar.ExtendsContentIntoTitleBar?0:appWindow.TitleBar.Height)) });
        }
    }


}
