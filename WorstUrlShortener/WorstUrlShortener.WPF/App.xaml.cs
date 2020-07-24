using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WPF;
using Application = Xamarin.Forms.Application;
using Point = Xamarin.Forms.Point;

namespace WorstUrlShortener.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private NotifyIcon notifyIcon;
        private bool isExit;

        protected override void OnStartup(StartupEventArgs e)
        {
            Forms.Init();

            base.OnStartup(e);

            notifyIcon = new NotifyIcon();
            notifyIcon.MouseUp += NotifyIconOnMouseUp;
            notifyIcon.MouseMove += NotifyIconOnMouseMove;
            notifyIcon.Icon = WorstUrlShortener.WPF.Properties.Resources.trayicon;
            notifyIcon.Visible = true;

            CreateContextMenu();
        }

        private void CreateContextMenu()
        {
            notifyIcon.ContextMenuStrip = new ContextMenuStrip();
            notifyIcon.ContextMenuStrip.Items.Add("Exit", WorstUrlShortener.WPF.Properties.Resources.trayicon.ToBitmap()).Click += (s, e) => this.ExitApplication();
        }

        private void ExitApplication()
        {
            isExit = true;
            if (this.MainWindow != null)
            {
                this.MainWindow.Closing -= MainWindow_Closing;
                // this.MainWindow.LostFocus -= MainWindow_LostFocus;
                this.MainWindow.Close();
                this.MainWindow = null;
            }
            notifyIcon.Dispose();
            notifyIcon = null;

            // Stop the application
            Current.Shutdown();
        }

        private void ToggleWindow()
        {
            // Create window when it is opened for the first time
            if (this.MainWindow == null)
            {
                this.MainWindow = new FormsApplicationPage
                {
                    Title = "'Worst' URL Shortener",
                    Height = 800,
                    Width = 350,
                    Topmost = true,
                    ShowInTaskbar = false,
                    ResizeMode = ResizeMode.NoResize,
                    WindowStyle = WindowStyle.ToolWindow
                };
                ((FormsApplicationPage)this.MainWindow).LoadApplication(new WorstUrlShortener.App(string.Empty));
                this.MainWindow.Closing += MainWindow_Closing;
                Application.Current.SendStart();
            }

            // Hide the window when it is visible
            if (this.MainWindow.IsVisible)
            {
                // Hide!
                this.MainWindow.Deactivated -= this.MainWindowOnDeactivated;
                this.MainWindow.Hide();
                Application.Current.SendSleep();
            }
            // Show the window when it is not visible
            else
            {
                // Position window 
                this.MainWindow.Left = SystemParameters.WorkArea.Width - this.MainWindow.Width - 50;
                this.MainWindow.Top = SystemParameters.WorkArea.Height - this.MainWindow.Height - 50;
                // Show!
                this.MainWindow.Show();
                this.MainWindow.Activate();
                this.MainWindow.Deactivated += this.MainWindowOnDeactivated;
                Application.Current.SendResume();
            }
        }

        // Toggle the window on a left click on the icon
        private void NotifyIconOnMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                this.ToggleWindow();
        }

        // Toggle when the window 'X' was clicked
        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (!this.isExit)
            {
                e.Cancel = true;
                // Only hide the window to avoid recreating it when it should get displayed again
                this.ToggleWindow();
            }
        }

        private System.Drawing.Point? lastMousePositionInIcon;

        // Store the current position of the mouse on the icon to check if the mouse clicked inside the icon
        // when the window gets deactivated to avoid a duplicated window toggle
        private void NotifyIconOnMouseMove(object sender, MouseEventArgs e)
        {
            lastMousePositionInIcon = Control.MousePosition;
        }

        /// <summary>
        /// Called when clicked outside the window.
        /// Toggles the window to get hidden.
        /// </summary> 
        private void MainWindowOnDeactivated(object sender, EventArgs e)
        {
            // Check if the deactivation came by clicking the icon since this already toggles the window
            if (lastMousePositionInIcon.HasValue && lastMousePositionInIcon == Control.MousePosition)
                return;
            ToggleWindow();
        }
    }
}
