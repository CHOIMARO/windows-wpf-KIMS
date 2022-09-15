using KISM.View.Function.Logout;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace KISM.ViewModel.Function.Logout {
    public class AutoLogoutPageVM : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        void onPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        internal void SetObserver(dynamic info) {
            StaticAttribute.Function.loginTimerTracker.Subscribe(info);
        }

        internal void UnsetObserver(dynamic info) {
            StaticAttribute.Function.loginTimerTracker.UnSubscribe(info);
        }
    }
}
