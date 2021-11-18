using AppWindowCore;
using Microsoft.UI.Windowing;
using Microsoft.Windows.ApplicationModel.DynamicDependency;
using System;
using System.Windows.Forms;

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
    }
}
