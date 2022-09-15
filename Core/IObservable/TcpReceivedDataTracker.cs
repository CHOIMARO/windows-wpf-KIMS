using Core.DAO.JSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IObservable {
    public class TcpReceivedDataTracker : IObservable<ReceivedFromKISDAO> {
        private List<IObserver<ReceivedFromKISDAO>> observers;
        public TcpReceivedDataTracker() {
            observers = new List<IObserver<ReceivedFromKISDAO>>();
        }
        public IDisposable Subscribe(IObserver<ReceivedFromKISDAO> observer) {
            if (!observers.Contains(observer)) {
                observers.Add(observer);
            }
            return new Unsubscriber(observers, observer);
        }
        public IDisposable Unsubscribe(IObserver<ReceivedFromKISDAO> observer) {
            if (observers.Contains(observer)) {
                observers.Add(observer);
            }
            return new Unsubscriber(observers, observer);
        }


        private class Unsubscriber : IDisposable {
            private List<IObserver<ReceivedFromKISDAO>> observers;
            private IObserver<ReceivedFromKISDAO> observer;

            public Unsubscriber(List<IObserver<ReceivedFromKISDAO>> observers, IObserver<ReceivedFromKISDAO> observer) {
                this.observers = observers;
                this.observer = observer;
            }

            public void Dispose() {
                if (observer != null && observers.Contains(observer)) {
                    observers.Remove(observer);
                }
            }
        }

        public void TrackReceivedDataNotify(ReceivedFromKISDAO state) {
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
