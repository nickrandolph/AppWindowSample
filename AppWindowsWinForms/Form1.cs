using AppWindowCore;
using Microsoft.UI.Windowing;
using Microsoft.Windows.ApplicationModel.DynamicDependency;
using System;
using System.Drawing;
using System.Windows.Forms;
using Windows.Graphics;

namespace AppWindowsWinForms
{
    public partial class Form1 : Form
    {
        AppWindow appWindow;
        public Form1()
        {
            InitializeComponent();

            var success =  Bootstrap.TryInitialize(0x00010000, out _);
            if (!success)
            {
                MessageBox.Show("Unable to initialize Windows App SDK - Make sure it's installed");
                return;
            }


            appWindow = this.GetAppWindow();
        }


        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Bootstrap.Shutdown();
        }

        private void RepositionClick(object sender, EventArgs e)
        {
            var tag = ((sender as Button).Tag+"").Split(",");
            var col = int.Parse(tag[0]);
            var row = int.Parse(tag[1]);
            appWindow.Reposition(row, col);
        }

        private void ChangeIconClick(object sender, EventArgs e)
        {
            appWindow.ChangeIcon("icon1.ico");
        }

        private void ChangePresenterClick(object sender, EventArgs e)
        {
            if (Enum.TryParse<AppWindowPresenterKind>((sender as Button).Text.ToString(), out var presenterKind))
            {
                appWindow.SetPresenter(presenterKind);
            }
        }

        private void OverlappedPresenterPropertyCheckedChanged(object sender, EventArgs e)
        {
            var presenter = appWindow.Presenter as OverlappedPresenter;
            if (presenter is null)
            {
                return;
            }

            var check = sender as CheckBox;
            var propertyName = check.Text;
            var property = typeof(OverlappedPresenter).GetProperty(propertyName);
            property.SetValue(presenter, check.Checked);
        }

        private void OverlappedPresenterTitleBarAndBorderCheckChanged(object sender, EventArgs e)
        {
            var presenter = appWindow.Presenter as OverlappedPresenter;
            if (presenter is null)
            {
                return;
            }
            var hasBorder = HasBorderCheckBox.Checked;
            var hasTitleBar = HasTitleBarCheckBox.Checked;
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

        private Windows.UI.Color GetRandomWindowsColor()
        {
            return Windows.UI.Color.FromArgb(
                (byte)rnd.Next(0, 255),
                (byte)rnd.Next(0, 255),
                (byte)rnd.Next(0, 255),
                (byte)rnd.Next(0, 255));
        }

        private void TitleBarRandomColorClick(object sender, EventArgs e)
        {
            var property = typeof(AppWindowTitleBar).GetProperty((sender as Button).Text.ToString());
            var clr = GetRandomWindowsColor();
            property.SetValue(appWindow.TitleBar, clr);
        }

        private void ChangeIconAndMenuClick(object sender, EventArgs e)
        {

            if (Enum.TryParse<IconShowOptions>((sender as Button).Text.ToString(), out var showOptions))
            {
                appWindow.TitleBar.IconShowOptions = showOptions;
            }
        }

        private void ToggleClientAreaChanged(object sender, EventArgs e)
        {
            appWindow.TitleBar.ExtendsContentIntoTitleBar = (sender as CheckBox).Checked;
        }

        private void SetDragAreaClick(object sender, EventArgs e)
        {
            var newHeight = (int)rnd.Next(0, 200);
            DragAreaBorder.Height = newHeight;
            DragAreaBorder.BackColor = GetRandomColor();
            appWindow.TitleBar.SetDragRectangles(new[] {
                new RectInt32(0,0,
                    (int)DragAreaBorder.Width,
                    (int)newHeight + (appWindow.TitleBar.ExtendsContentIntoTitleBar?0:appWindow.TitleBar.Height)) });

        }

    }
}
