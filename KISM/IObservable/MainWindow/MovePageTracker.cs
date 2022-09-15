using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KISM.IObservable.MainWindow {
    public class MovePageTracker:IObservable<int> {
        private List<IObserver<int>> observers;
        public MovePageTracker() {
            observers = new List<IObserver<int>>();
        }
        public IDisposable Subscribe(IObserver<int> observer) {
            if (!observers.Contains(observer)) {
                observers.Add(observer);
            }
            return new Unsubscriber(observers, observer);
        }

        private class Unsubscriber : IDisposable {
            private List<IObserver<int>> observers;
            private IObserver<int> observer;

            public Unsubscriber(List<IObserver<int>> observers, IObserver<int> observer) {
                this.observers = observers;
                this.observer = observer;
            }

            public void Dispose() {
                if (observer != null && observers.Contains(observer)) {
                    observers.Remove(observer);
                }
            }
        }


        public void TrackMovePageNotify(int state) {
            foreach (var observer in observers) {
                
                    observer.OnNext(state);
                
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
