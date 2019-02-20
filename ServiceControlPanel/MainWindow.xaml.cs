using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ServiceControlPanel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ServiceController _sc;
        private readonly DispatcherTimer _dispatcherTimer = new DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();
            _sc = new ServiceController("FeederCapturer");
            _dispatcherTimer.Tick += new EventHandler(DispatcherTimer_Tick);
            _dispatcherTimer.Interval = new TimeSpan(1000);
            _dispatcherTimer.Start();
        }
        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            UpdateStatus();
        }
        private void WndMainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void BtnResume_Click(object sender, RoutedEventArgs e)
        {

            if (_sc.Status == ServiceControllerStatus.Stopped)
                _sc.Start();
            else if (_sc.Status == ServiceControllerStatus.Paused)
                _sc.Continue();

        }

        private void BtnPause_Click(object sender, RoutedEventArgs e)
        {
            if (_sc.Status == ServiceControllerStatus.Running)
            {
                _sc.Pause();
            }
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            if (_sc.Status != ServiceControllerStatus.Stopped)
            {
                _sc.Stop();
            }
        }

        private void BtnViewLogger_Click(object sender, RoutedEventArgs e)
        {

        }    

        private void UpdateStatus()
        {
            _sc = new ServiceController("FeederCapturer");
            LblServiceStatus.Content = _sc.Status.ToString();
        }
    }
}
