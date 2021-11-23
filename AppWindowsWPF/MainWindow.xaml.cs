using AppWindowCore;
using Microsoft.UI.Windowing;
using Microsoft.Windows.ApplicationModel.DynamicDependency;
using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Windows.Graphics;
using Windows.UI;

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

        protected override void OnClosed(System.EventArgs e)
        {
            base.OnClosed(e);

            Bootstrap.Shutdown();
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

        private System.Windows.Media.Color GetRandomColor()
        {
            return System.Windows.Media.Color.FromArgb(
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
            appWindow.TitleBar.ExtendsContentIntoTitleBar = (sender as CheckBox).IsChecked ?? false;
        }

        private void SetDragAreaClick(object sender, RoutedEventArgs e)
        {

            var newHeight = (int)rnd.Next(0, 200);
            DragAreaBorder.Height = newHeight;
            DragAreaBorder.Background = new SolidColorBrush(GetRandomColor());

            PresentationSource source = PresentationSource.FromVisual(this);

            double scaleX = 1.0, scaleY = 1.0;
            if (source != null)
            {
                scaleX = source.CompositionTarget.TransformToDevice.M11;
                scaleY = source.CompositionTarget.TransformToDevice.M22;
            }

            appWindow.TitleBar.SetDragRectangles(new[] {
                new RectInt32(0,0,
                    (int)(DragAreaBorder.ActualWidth * scaleX),
                    (int)((newHeight + (appWindow.TitleBar.ExtendsContentIntoTitleBar?0:appWindow.TitleBar.Height))*scaleY)) });
        }
    }
}
