using KISM.DAO.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KISM.StaticAttribute {
    static public class ConstAttribute {
        //public static string ip = "127.0.0.1";
        //public static int port = 9100;
        public static string ip = "192.168.71.30";
        public static int port = 2500;
        internal static userInfo userInfo = new userInfo();
        //internal static int pmit = 300;
        //public static string ip = "192.168.71.121";
        //public static int port = 9100;

        public const int WM_DEVICECHANGE = 0x219;//After the U disk is inserted, the bottom layer of the OS will automatitely detect it, and then send a "hardware device status changed" message to the application
        public const int DBT_DEVICEARRIVAL = 0x8000;  //It is used to indicate that the U disk is available. A device or media has been inserted into a piece and is now available.
        public const int DBT_CONFIGCHANGECANCELED = 0x0019;  //Request to change the current configuration (or cancel docking) has been cancelled.
        public const int DBT_CONFIGCHANGED = 0x0018;  //The current configuration has changed due to dock or unpinned.
        public const int DBT_CUSTOMEVENT = 0x8006; //Custom event occurs. Windows NT 4.0 and Windows 95: This value is not supported.
        public const int DBT_DEVICEQUERYREMOVE = 0x8001;  //Approval request to delete a device or media work. No application can deny this request and cancel the deletion.
        public const int DBT_DEVICEQUERYREMOVEFAILED = 0x8002;  //Request to delete a device or media piece has been cancelled.
        public const int DBT_DEVICEREMOVECOMPLETE = 0x8004;  //A device or media piece has been deleted.
        public const int DBT_DEVICEREMOVEPENDING = 0x8003;  //A piece of equipment or media is about to be deleted. There is no denying it.
        public const int DBT_DEVICETYPESPECIFIC = 0x8005;  //A device specific event occurs.
        public const int DBT_DEVNODES_CHANGED = 0x0007;  //A device has been added to or removed from the system.
        public const int DBT_QUERYCHANGECONFIG = 0x0017;  //License is required to change the current configuration (dock or unpin).
        public const int DBT_USERDEFINED = 0xFFFF;  //The meaning of this message is user-defined
        public const uint GENERIC_READ = 0x80000000;
        public const int GENERIC_WRITE = 0x40000000;
        public const int FILE_SHARE_READ = 0x1;
        public const int FILE_SHARE_WRITE = 0x2;
        public const int IOCTL_STORAGE_EJECT_MEDIA = 0x2d4808;
        public const int DBT_DEVTUP_VOLUME = 0x02;
        public const int DBT_DEVTUP_PORT = 0x03;
        public const int EYL_DEVICE_CONNECTED = 0x0007;

        static public List<string> serPortList = new List<string>();
        static public string usbDir = string.Empty;
        static public string currentConnectedSerial = string.Empty;


        internal const string loginSuccess = "로그인이 완료되었습니다.";
        internal const string loginFailed = "ID/PW가 틀렸습니다. 다시 시도해주세요.";
        internal const string accountAuthSuccess = "계정 권한이 확인되었습니다.";
        internal const string accountAuthFailed = "접근 권한이 없습니다.";
        internal const string checkedAccountInfo = "계정 정보가 올바르지 않습니다.\r\n    다시 확인하여 주십시요.";
        internal const string insertAccountInfoSuccess = "   계정 정보가 저장되었습니다.\r\n생성된 계정으로 로그인해주세요.";
        internal const string insertAccountInfoFailed = "계정 정보 저장에 실패했습니다.";
        internal const string updateAccountInfoSuccess = "   계정 정보가 변경되었습니다.\r\n변경된 계정으로 로그인해주세요.";
        internal const string updateAccountInfoFailed = "계정 정보 변경에 실패했습니다.";
        internal const string deleteAccountInfoSuccess = "          계정 정보가 삭제되었습니다.\r\n초기 설정된 계정으로 다시 로그인해주세요.";
        internal const string deleteAccountInfoFailed = "계정 정보 삭제에 실패했습니다.";
        internal const string insertMilitaryUnitInfoSuccess = "부대 정보가 저장되었습니다.";
        internal const string insertMilitaryUnitInfoFailed = "부대 정보 저장에 실패했습니다.";
        internal const string insertPurposeInfoSuccess = "용도 정보가 저장되었습니다.";
        internal const string insertPurposeInfoFailed = "용도 정보 저장에 실패했습니다.";
        internal const string tcpClientReconnect = "키 주입기와 재연결합니다.";

        internal const string serialConnectSettingFailed = "통신 설정을 다시 확인해주세요.";

        //Loading Message
        public const string keyGenWaitEventMessage = "암호키 생성 중입니다.\r\n잠시만 기다려주세요.";
        public const string keyGenSuccessEventMessage = "암호키 생성 완료되었습니다.";
        public const string keyGenFailedEventMessage = "암호키 생성 실패했습니다.";
        public const string usbConnect = " 드라이브 USB가 연결 되었습니다.";
        public const string usbDisconnect = " 드라이브 USB가 연결 해제 되었습니다.";
        public const string serConnect = " 시리얼 포트가 연결 되었습니다.";
        public const string serDisconnect = " 시리얼 포트가 연결 해제 되었습니다.";
        public const string reqUsbDisconnect = "암호키 생성이 완료되었습니다.\r\nUSB를 제거하여 암호키를 등록바랍니다.";
    }
}
