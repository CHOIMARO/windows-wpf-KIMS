using External.CoreEngine;
using External.DAO;
using External.Database;
using External.IObservable;
using External.IObservable.Serial;
using External.TCP;
using External.Util;
using Log.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace External.StaticAttribute {
    public class Function {
        static public string sqlPath = Environment.CurrentDirectory + @"\KeyIssuedTerminal.sqlite";
        static public string interAuthString = "KISM3";
        static internal int ijidx = -1;
        static internal string ip = string.Empty;
        static internal int port = -1;

        static public Socket tcpClient;
        static public NetworkStream stream;

        //static public TcpConnectService tcpConnectService = new TcpConnectServiceImpl();
        static public InjectorService injectorService = new InjectorService();
        static public KeyRelationService keyRelationService = new KeyRelationService();
        static public DistributionService distributionService = new DistributionService();
        static public TcpReceivedService tcpReceivedService = new TcpReceivedService();
        static public TcpClientService tcpClientService = new TcpClientService();

        static public CoreEngineService coreEngineService = new CoreEngineService();

        static public LogInfoService logInfoService = new LogInfoService();

        #region Tracker

        static public TcpIsConnectTracker tcpIsConnectTracker = new TcpIsConnectTracker();
        static internal TcpReceivedDataTracker tcpReceivedDataTracker = new TcpReceivedDataTracker();
        static public SerialReceivedDataTracker serialReceivedDataTracker = new SerialReceivedDataTracker();

        #endregion

        #region Util
        static internal EncryptionCommand encryptionCommand = new EncryptionCommand();
        static internal TcpMessageProcess messageProcess = new TcpMessageProcess();
        static internal LogCommand logCommand = new LogCommandImpl();
        #endregion

    }
}
