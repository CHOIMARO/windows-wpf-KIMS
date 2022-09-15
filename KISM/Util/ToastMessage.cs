using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;
using ToastNotifications.Messages;
using System.Windows;
using KISM.StaticAttribute.Enum;

namespace KISM.Util {
    public class ToastMessage {
        Notifier notifier = new Notifier(cfg => {
            cfg.PositionProvider = new WindowPositionProvider(
                parentWindow: Application.Current.MainWindow,
                corner: Corner.BottomRight,
                offsetX: 10,
                offsetY: 10);

            cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                notificationLifetime: TimeSpan.FromSeconds(3),
                maximumNotificationCount: MaximumNotificationCount.FromCount(5));

            cfg.Dispatcher = Application.Current.Dispatcher;
        });

        internal void showMessage(toastStateEnum toastState, string message = "show message") {
            switch (toastState) {
                case toastStateEnum.INFORMATION:
                    notifier.ShowInformation(message);
                    break;
                case toastStateEnum.SUCCESS:
                    notifier.ShowSuccess(message);
                    break;
                case toastStateEnum.WARNING:
                    notifier.ShowWarning(message);
                    break;
                case toastStateEnum.ERROR:
                    notifier.ShowError(message);
                    break;
            }
        }
    }
}
