using KISM.View.Loading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace KISM.Util {
    public class LoadingMessage {
        public string message = string.Empty;
        Timer loadingTimer;
        bool receivedMsg = false;
        public void setMessage(string message) {
            this.message = message;
            setLoadingTimer();
        }
        public void setLoadingTimer() {
            if (loadingTimer != null) {
                loadingTimer.Change(Timeout.Infinite, Timeout.Infinite);
                loadingTimer.Dispose();
                loadingTimer = null;
            }
            loadingTimer = new Timer(loadingViewCallBack);
            loadingTimer.Change(0, Timeout.Infinite);
        }
        public void loadingViewCallBack(object state) {
            loadingView();
            loadingTimer.Change(Timeout.Infinite, Timeout.Infinite);
            if (!receivedMsg) {
                InformationMessage.InformationShowDialog("                  주입기가 응답하지 않습니다.\r\n" +
                                                                                 "연결을 확인하시고, PC와 앱 종료 후, 다시 연결해보세요.");
            }
            receivedMsg = false;
        }
        public void loadingView(int time = 3) {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate {
                try {
                    StaticAttribute.Function.loadingWindow = new LoadingWindow();
                    StaticAttribute.Function.loadingWindow.setLoadingMedia(time, message);
                    StaticAttribute.Function.loadingWindow.ShowDialog();
                } catch (Exception e) {
                    Console.WriteLine("Loading view error : " + e);
                }
            }));
        }
        public void loadingViewClose() {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate {
                if (loadingTimer != null) {
                    loadingTimer.Change(Timeout.Infinite, Timeout.Infinite);
                    StaticAttribute.Function.loadingWindow.Close();
                    receivedMsg = true;
                }
            }));
        }
    }
}
