using Core.IObservable;
using Core.Util;
using External.CoreEngine;
using External.Database;
using External.Serial;
using External.TCP;
using External.UDP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.StaticAttribute {

    public static class Function {
        #region External

        static public UdpConnectService udpConnectService = new UdpConnectService();
        static public SerialCommSettingService serialCommSettingService = new SerialCommSettingService();
        static public SerialService serialService = new SerialService();

        //renewal
        static public CoreEngineService coreEngineService = new CoreEngineService();

        static internal AccountService accountService = new AccountService();
        static internal InjectorService injectorService = new InjectorService();
        static internal KeyRelationService keyRelationService = new KeyRelationService();
        static internal DistributionService distributionService = new DistributionService();
        static internal TcpSendService tcpSendService = new TcpSendService();
        static internal MilitaryUnitService militaryUnitService = new MilitaryUnitService();
        static internal PurposeInfoService purposeInfoService = new PurposeInfoService();
        static internal CommInfoService commInfoService = new CommInfoService();
        static internal LogInfoService logInfoService = new LogInfoService();
        #endregion

        #region Observer
        static internal TcpReceivedDataTracker tcpReceivedDataTracker = new TcpReceivedDataTracker();
        static public TcpIsConnectTracker tcpIsConnectTracker = new TcpIsConnectTracker();

        #endregion

        #region Util
        static internal EncryptionCommand encryptionCommand = new EncryptionCommand();
        public static string ip = "192.168.71.30";
        public static int port = 2500;

        #endregion
    }
}
