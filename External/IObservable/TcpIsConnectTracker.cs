using External.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.IObservable {
    public class TcpIsConnectTracker :IObservable<TcpIsConnectDAO>{
        private List<IObserver<TcpIsConnectDAO>> observers;
        public TcpIsConnectTracker() {
            observers = new List<IObserver<TcpIsConnectDAO>>();
        }
        public IDisposable Subscribe(IObserver<TcpIsConnectDAO> observer) {
            if (!observers.Contains(observer)) {
                observers.Add(observer);
            }
            return new Unsubscriber(observers, observer);
        }

        private class Unsubscriber : IDisposable {
            private List<IObserver<TcpIsConnectDAO>> observers;
            private IObserver<TcpIsConnectDAO> observer;

            public Unsubscriber(List<IObserver<TcpIsConnectDAO>> observers, IObserver<TcpIsConnectDAO> observer) {
                this.observers = observers;
                this.observer = observer;
            }

            public void Dispose() {
                if (observer != null && observers.Contains(observer)) {
                    observers.Remove(observer);
                }
            }
        }

        public void TrackConnectNotify(TcpIsConnectDAO state) {
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
