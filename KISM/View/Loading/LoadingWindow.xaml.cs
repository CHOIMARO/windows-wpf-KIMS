using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace KISM.View.Loading {
    /// <summary>
    /// LoadingWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LoadingWindow : Window {
        Timer closeTimer;
        string defaultMessage = "Loading";
        int intervalUnit = 1000;
        public LoadingWindow() {
            InitializeComponent();
            initCloseTimer();
            this.PreviewKeyDown += LoadingWindow_PreviewKeyDown;
        }
        private void LoadingWindow_PreviewKeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.System && e.SystemKey == Key.F4) {
                StaticAttribute.Function.loadingMessage.loadingViewClose();
                //StaticAttribute.Function.overtimeLoadingMessage.overtimeLoadingViewClose();
            }
        }

        private void initCloseTimer() {
            closeTimer = new Timer();
            closeTimer.Interval = 3 * intervalUnit;
            closeTimer.Elapsed += CloseTimer_Elapsed;
        }

        private void CloseTimer_Elapsed(object sender, ElapsedEventArgs e) {
            closeTimer.Stop();

            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate {
                Close();
            }));
        }

        public void setLoadingMedia(int closeTime = 3, string message = null) {
            LoadingMessage.Text = message != null ? message : defaultMessage;

            if (closeTime == 0) {
                closeTimer.Stop();
                return;
            }
            closeTimer.Interval = closeTime * intervalUnit;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            closeTimer.Start();
        }
    }
}
