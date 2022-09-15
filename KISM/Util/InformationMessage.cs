using KISM.View.Information;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace KISM.Util {
    public class InformationMessage {
        static internal void InformationShowDialog(string message) {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate {
                InformationWindow informationWindow = new InformationWindow();
                informationWindow.informationMessageSetting(message);
                informationWindow.ShowDialog();
            }));
        }
        static internal void InformationShowDialogExpandSize(string message) {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate {
                InformationWindow informationWindow = new InformationWindow();
                informationWindow.informationMessageSetting(message);
                informationWindow.settingWindow();
                informationWindow.ShowDialog();
            }));
        }
    }
}
