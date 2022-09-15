using Core.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Core.UseCase.TCP {
    public class TcpConnectUseCase : UseCase, IObserver<External.DAO.TcpIsConnectDAO> {
        string ip = "192.168.71.30";
        int port = 2500;
        public void Connect() {
            SetTcpInfo(ip, port);
            Task.Run(() => External.StaticAttribute.Function.tcpClientService.Connect(ip, port));
        }

        public bool Disconnect() {
            return External.StaticAttribute.Function.tcpClientService.Disconnect();
        }
        public void SetIsConnectObserver(dynamic info) {
            StaticAttribute.Function.tcpIsConnectTracker.Subscribe(info);
            External.StaticAttribute.Function.tcpClientService.SetIsConnectObserver(this);
        }
        public void OnNext(External.DAO.TcpIsConnectDAO value) {
            StaticAttribute.Function.tcpIsConnectTracker.TrackConnectNotify(new TcpIsConnectDAO {
                stat = value.stat
            });
        }
        private void SetTcpInfo(string ip, int port) {
            var tcpClientSetting = StaticAttribute.Function.commInfoService.SelectCommInfoItem();
            if (tcpClientSetting.Count > 0) {
                
                this.ip = tcpClientSetting[tcpClientSetting.Count-1].ip;
                this.port = tcpClientSetting[tcpClientSetting.Count-1].port;
            }
        }
        public void OnCompleted() {
            throw new NotImplementedException();
        }

        public void OnError(Exception error) {
            throw new NotImplementedException();
        }
    }
}
