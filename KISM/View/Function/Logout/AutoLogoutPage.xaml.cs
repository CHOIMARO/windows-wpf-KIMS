using KISM.DAO;
using KISM.ViewModel.Function.Logout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace KISM.View.Function.Logout {
    /// <summary>
    /// AutoLogoutPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class AutoLogoutPage : Window, IObserver<LoginTimerDAO> {
        AutoLogoutPageVM autoLogoutPageVM;
        public AutoLogoutPage() {
            InitializeComponent();
            autoLogoutPageVM = new AutoLogoutPageVM();
            DataContext = autoLogoutPageVM;
            Init();
        }
        private void Init() {
            remainLogoutTime.Text = "1분 0초";
        }

        private void YesBtn_Click(object sender, RoutedEventArgs e) {            
            GetWindow(this).Close();
            StaticAttribute.Function.movePageTracker.TrackMovePageNotify(601);
        }

        private void NoBtn_Click(object sender, RoutedEventArgs e) {
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            autoLogoutPageVM.SetObserver(this);
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e) {
            autoLogoutPageVM.UnsetObserver(this);
        }

        public void OnNext(LoginTimerDAO value) {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate {
                remainLogoutTime.Text = value.time;
            }));
        }

        public void OnError(Exception error) {
            throw new NotImplementedException();
        }

        public void OnCompleted() {
            throw new NotImplementedException();
        }
    }
}
