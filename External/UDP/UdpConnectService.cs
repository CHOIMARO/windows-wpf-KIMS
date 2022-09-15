using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace External.UDP {
    public class UdpConnectService : Service {
        public void connect() {
            int recv = 0;
            byte[] data = new byte[1024];

            IPEndPoint ep = new IPEndPoint(IPAddress.Any, 9050);
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            server.Bind(ep);

            Console.WriteLine("Waiting for a client...");

            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            EndPoint remoteEP = (EndPoint)sender;

            recv = server.ReceiveFrom(data, ref remoteEP);

            Console.WriteLine("[first] Message received from {0}", remoteEP.ToString());
            Console.WriteLine("[first] received data : {0}", Encoding.UTF8.GetString(data, 0, recv));

            string welcome = "Welcome to udp server";
            data = Encoding.UTF8.GetBytes(welcome);
            server.SendTo(data, remoteEP);

            while (true) {
                data = new byte[1024];
                recv = server.ReceiveFrom(data, ref remoteEP);
                string recvData = Encoding.UTF8.GetString(data, 0, recv);
                Console.WriteLine("received data : {0}", recvData);

                server.SendTo(Encoding.UTF8.GetBytes(recvData), remoteEP);
                Console.WriteLine("send data : {0}", Encoding.UTF8.GetString(data, 0, recv));
                Console.WriteLine("");
            }
        }

        private void interAuth() { //상호 인증

        }
        private void getCryptoKey() { //암호키 등록을 위해 암호키 가져오기

        }
        private void setCryptoKey() {//암호키 주입

        }
        private void getCryptoKeyInformation() { // 암호키 이력 가져오기

        }
        private void setAccountInformation() { //주입기 계정 정보 설정

        }
    }
}
