using KISM.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KISM.IObservable.MainWindow {
    public class LoginTimerTracker : IObservable<LoginTimerDAO>{
        private List<IObserver<LoginTimerDAO>> observers;
        public LoginTimerTracker() {
            observers = new List<IObserver<LoginTimerDAO>>();
        }
        public IDisposable Subscribe(IObserver<LoginTimerDAO> observer) {
            if (!observers.Contains(observer)) {
                observers.Add(observer);
            }
            
            return new Unsubscriber(observers, observer);
        }
        public IDisposable UnSubscribe(IObserver<LoginTimerDAO> observer) {
            if (observers.Contains(observer)) {
                observers.Remove(observer);
            }
            return new Unsubscriber(observers, observer);
        }

        private class Unsubscriber : IDisposable {
            private List<IObserver<LoginTimerDAO>> observers;
            private IObserver<LoginTimerDAO> observer;

            public Unsubscriber(List<IObserver<LoginTimerDAO>> observers, IObserver<LoginTimerDAO> observer) {
                this.observers = observers;
                this.observer = observer;
            }

            public void Dispose() {
                if (observer != null && observers.Contains(observer)) {
                    observers.Remove(observer);
                }
            }
        }


        public void TrackLoginTimerNotify(LoginTimerDAO state) {
            foreach (var observer in observers) {
                if (state == null) {
                    observer.OnError(new NotImplementedException());
                } else {
                    observer.OnNext(state);
                }
            }
        }

        public void EndTransmission() {
            foreach (var observer in observers.ToArray()) {
                if (observers.Contains(observer)) {
                    observer.OnCompleted();
                    observers.Remove(observer);
                }
            }

            //observers.Clear();
        }
    }
}
