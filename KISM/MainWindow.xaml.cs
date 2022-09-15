using KISM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KISM {
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : NavigationWindow {
        MainWindowVM mainWindowVM;
        public MainWindow() {
            InitializeComponent();
            mainWindowVM = new MainWindowVM();
            mainWindowVM.SetObserver();
            mainWindowVM.GetSerialPort();

            this.PreviewMouseUp += MainWindow_PreviewMouseUp;
            this.PreviewKeyDown += MainWindow_PreviewKeyDown;
        }

        private void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.System && e.SystemKey == Key.F4) {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }

        private void MainWindow_PreviewMouseUp(object sender, MouseButtonEventArgs e) {
            mainWindowVM.LoginTimeExtensionBtn_Click();
        }

        private void NavigationWindow_Loaded(object sender, RoutedEventArgs e) {
            //WindowInteropHelper helper = new WindowInteropHelper(this);
            //HwndSource source = HwndSource.FromHwnd(helper.Handle);
            //source.AddHook(new HwndSourceHook(this.WndProc));
        }
        IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled) {
            int devType = 0;

            if (msg == StaticAttribute.ConstAttribute.WM_DEVICECHANGE) {
                switch (wParam.ToInt32()) {
                    case StaticAttribute.ConstAttribute.DBT_DEVICEARRIVAL:
                        devType = Marshal.ReadInt32(lParam, 4);
                        mainWindowVM.CheckedDeviceConnect(devType);

                        break;
                    case StaticAttribute.ConstAttribute.DBT_DEVICEREMOVECOMPLETE:
                        devType = Marshal.ReadInt32(lParam, 4);
                        mainWindowVM.CheckedDeviceDisconnect(devType);
                        break;
                    default:
                        break;
                }
            }
            return IntPtr.Zero;
        }
    }
}
