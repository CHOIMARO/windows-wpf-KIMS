using KISM.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KISM.IObservable.KeyRegistrationManagementPage {
    public class ExpirationKeyTracker : IObservable<List<OverExpKeyDAO>> {
        private List<IObserver<List<OverExpKeyDAO>>> observers;
        public ExpirationKeyTracker() {
            observers = new List<IObserver<List<OverExpKeyDAO>>>();
        }
        public IDisposable Subscribe(IObserver<List<OverExpKeyDAO>> observer) {
            if (!observers.Contains(observer)) {
                observers.Add(observer);
            }
            return new Unsubscriber(observers, observer);
        }

        private class Unsubscriber : IDisposable {
            private List<IObserver<List<OverExpKeyDAO>>> observers;
            private IObserver<List<OverExpKeyDAO>> observer;

            public Unsubscriber(List<IObserver<List<OverExpKeyDAO>>> observers, IObserver<List<OverExpKeyDAO>> observer) {
                this.observers = observers;
                this.observer = observer;
            }

            public void Dispose() {
                if (observer != null && observers.Contains(observer)) {
                    observers.Remove(observer);
                }
            }
        }

        public void TrackExpirationNotify(List<OverExpKeyDAO> state) {
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
                }
            }

            observers.Clear();
        }
    }
}
