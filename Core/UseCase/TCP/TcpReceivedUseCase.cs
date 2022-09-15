using Core.DAO.JSON;
using Core.StaticAttribute.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UseCase.TCP {
    public class TcpReceivedUseCase : UseCase, IObserver<External.DAO.JSON.ReceivedFromKISDAO> {
        public void SetObserver(dynamic info) {
            StaticAttribute.Function.tcpReceivedDataTracker.Subscribe(info);
            External.StaticAttribute.Function.tcpClientService.SetObserver(this);
        }
        private ReceivedFromKISDAO ReceivedDataDAO(External.DAO.JSON.ReceivedFromKISDAO receivedFromKISData)
                    => new ReceivedFromKISDAO {
                        sig = receivedFromKISData.sig,
                        ver = receivedFromKISData.ver,
                        type = (typeEnum)receivedFromKISData.type,
                        cmd = (commandEnum)receivedFromKISData.cmd,
                        msg = receivedFromKISData.msg
                    };

        public void OnNext(External.DAO.JSON.ReceivedFromKISDAO value) {
            StaticAttribute.Function.tcpReceivedDataTracker.TrackReceivedDataNotify(ReceivedDataDAO(value));

        }

        public void OnCompleted() {
            throw new NotImplementedException();
        }

        public void OnError(Exception error) {
            throw new NotImplementedException();
        }
    }
}
