using KISM.DAO;
using KISM.DAO.JSON.KISM3;
using KISM.DAO.KeyRel;
using KISM.DAO.MilUnit;
using KISM.DAO.Ppose;
using KISM.DAO.TCP;
using KISM.StaticAttribute.Enum;
using KISM.Util;
using KISM.View.Function.KeyDeletion;
using KISM.View.Function.KeyGeneration;
using KISM.View.Function.KeyRegistration;
using KISM.View.Function.KeySending;
using KISM.View.Function.Logout;
using KISM.View.Loading;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace KISM.ViewModel.SubPageVM {
    class KeyRegistrationManagementPageVM : INotifyPropertyChanged, IObserver<LoginTimerDAO>{
        public event PropertyChangedEventHandler PropertyChanged;
        void onPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        ConnectedInjectorDAO connectedInjectorDAO = new ConnectedInjectorDAO();
        KeyInformationDAO keyInformationDAO = new KeyInformationDAO();

        string selectedSKey = string.Empty;

        string wrappedKey = string.Empty;

        #region DataContext
        private string loginTime = "10분 0초";
        public string LoginTime {
            get {
                return loginTime;
            }
            set {
                loginTime = value;
                onPropertyChanged("LoginTime");
            }
        }

        private double deleteKeyIconOpacity = 0.5;
        public double DeleteKeyIconOpacity {
            get {
                return deleteKeyIconOpacity;
            }
            set {
                deleteKeyIconOpacity = value;
                onPropertyChanged("DeleteKeyIconOpacity");
            }
        }
        private double reInjectionIconOpacity = 0.5;
        public double ReInjectionIconOpacity {
            get {
                return reInjectionIconOpacity;
            }
            set {
                reInjectionIconOpacity = value;
                onPropertyChanged("ReInjectionIconOpacity");
            }
        }

        private string todayDate = DateTime.Now.ToString("yyyy-MM-dd");
        public string TodayDate {
            get {
                return todayDate;
            }
            set {
                todayDate = value;
                onPropertyChanged("TodayDate");
            }
        }
        private ObservableCollection<KeyRelDAO> registeredDataRow = new ObservableCollection<KeyRelDAO>();
        public ObservableCollection<KeyRelDAO> RegisteredDataRow {
            get {
                return registeredDataRow;
            }
            set {
                registeredDataRow = value;
                onPropertyChanged("RegisteredDataRow");
            }
        }
        

        private string keyInformationText = string.Empty;
        public string KeyInformationText {
            get {
                return keyInformationText;
            }
            set {
                keyInformationText = value;
                onPropertyChanged("KeyInformationText");
            }
        }
        private Brush connectConfirmColor = Brushes.DarkGray;
        public Brush ConnectConfirmColor {
            get { return connectConfirmColor; }
            set {
                connectConfirmColor = value;
                onPropertyChanged("ConnectConfirmColor");
            }
        }
        private Brush keyBackground = Brushes.Transparent;
        public Brush KeyBackground {
            get { return keyBackground; }
            set {
                keyBackground = value;
                onPropertyChanged("KeyBackground");
            }
        }

        private ObservableCollection<string> expirationItems = new ObservableCollection<string> {
            "1개월","2개월","3개월","4개월","5개월","6개월","7개월","8개월","9개월","10개월","11개월","12개월"
        };
        public ObservableCollection<string> ExpirationItems {
            get {
                return expirationItems;
            }
            set {
                expirationItems = value;
                onPropertyChanged("ExpirationItems");
            }
        }
        private ObservableCollection<string> grpItems = new ObservableCollection<string> {
            "A부대", "B부대", "C부대", "D부대"
        };

        public ObservableCollection<string> GrpItems {
            get {
                return grpItems;
            }
            set {
                grpItems = value;
                onPropertyChanged("GrpItems");
            }
        }
        private ObservableCollection<string> purPoseItems = new ObservableCollection<string> {
            "DRONE", "CCTV", "ETC"
        };
        public ObservableCollection<string> PurPoseItems {
            get {
                return purPoseItems;
            }
            set {
                purPoseItems = value;
                onPropertyChanged("PurPoseItems");
            }
        }

        private double keyOpacity = 0.1;
        public double KeyOpacity {
            get {
                return keyOpacity;
            }
            set {
                keyOpacity = value;
                onPropertyChanged("KeyOpacity");
            }
        }


        #endregion
        public void SetObserver(dynamic info) {
            StaticAttribute.Function.expirationKeyTracker.Subscribe(info);

            //renewal
            StaticAttribute.Function.loginTimerTracker.Subscribe(this);
            StaticAttribute.Function.tcpIsConnectTracker.Subscribe(info);
            StaticAttribute.Function.tcpReceivedDataTracker2.Subscribe(info);
        }

        public void UnsetObserver(dynamic info) {
            StaticAttribute.Function.loginTimerTracker.UnSubscribe(this);
            StaticAttribute.Function.tcpIsConnectTracker.Unsubscribe(info);
            StaticAttribute.Function.tcpReceivedDataTracker2.Unsubscribe(info);
        }
        public void Init() {
            SetMilitaryUnitList();
            SetPurposeList();
            SetExpirationDate();
        }

        private void SetExpirationDate() {
            if(StaticAttribute.ConstAttribute.userInfo.uid.Equals("tngenKimsMaster")) {
                ExpirationItems.Add("무제한");
            }
            
        }

        private void SetMilitaryUnitList() {
            var milUnitInfoList = StaticAttribute.Function.selectMilUnitInfoAllUseCase.Execute();
            GrpItems.Clear();

            foreach(var milUnit in milUnitInfoList) {
                if (milUnit.stat.Equals("A")) {
                    GrpItems.Add(milUnit.unit);
                }
            }
        }

        private void SetPurposeList() {
            var pposeInfoList = StaticAttribute.Function.selectPposeInfoAllUseCase.Execute();
            PurPoseItems.Clear();

            foreach(var purpose in pposeInfoList) {
                if (purpose.stat.Equals("A")) {
                    PurPoseItems.Add(purpose.ppose);
                }
            }

        }

        public void ShowKeyGenerationPage() {
            KeyGenerationPage keyGenerationPage = new KeyGenerationPage();
            keyGenerationPage.ShowDialog();

            if(StaticAttribute.Function.keyGenerationPageState) {
                StaticAttribute.Function.loadingMessage.setMessage("키 생성 중..");
                GenerateKey();
            }

        }
        public void GenerateKey() {
            int rndType = -1;
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => {

                foreach (int i in Enum.GetValues(typeof(RndTypeEnum))) {
                    int checkAvailableHsm = StaticAttribute.Function.findAvailableRndTypeUseCase.Execute(i);
                    if (checkAvailableHsm == 0 && i != 0) {
                        rndType = i;
                        Console.WriteLine(Enum.GetName(typeof(RndTypeEnum), i) + "으로 키 생성을 시도합니다.");
                        break;
                    }
                }

                int keyLen = 16;
                byte[] key = new byte[keyLen];
                int result = StaticAttribute.Function.generateKeyUseCase.Execute(ref key, keyLen, rndType);
                Console.WriteLine(result);
                if (result == 0) { //성공
                    string createdKey = StaticAttribute.Function.hexAndByte.byteToHex(key);
                    //string createdKey = "09F54CBD1C929D53813A7A07D351C807"; //유콘, 억세스위
                    //string createdKey = "57E0A76ABA0AF7DFD1120C0183827E92"; //네스엔텍

                    wrappedKey = WrapKey(createdKey);
                    ConnectConfirmColor = System.Windows.Media.Brushes.Green;
                    KeyOpacity = 1.0;
                    KeyBackground = System.Windows.Media.Brushes.White;
                    KeyInformationText = wrappedKey.Substring(0, 6) + "*********";

                    InsertLog(LogEnum.INFO, "키 생성 성공");

                    StaticAttribute.Function.loadingMessage.loadingViewClose();
                    InformationMessage.InformationShowDialog("              키 생성이 완료되었습니다.\r\n" +
                                                                                     "부대 및 용도, 유효일을 설정하고 등록해주세요.");
                } else { //실패
                    StaticAttribute.Function.loadingMessage.loadingViewClose();
                    InformationMessage.InformationShowDialog("인식된 HSM이 존재하지 않습니다.");
                    KeyInformationText = "";
                    KeyOpacity = 0.1;
                    KeyBackground = Brushes.Transparent;
                    ConnectConfirmColor = Brushes.DarkGray;
                    wrappedKey = string.Empty;
                    InsertLog(LogEnum.ERROR, "키 생성 실패(인식된 HSM이 존재하지 않음)");
                }
            }));

        }
        public void ShowRegisteredData() {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => {
                RegisteredDataRow.Clear();
                AddColumnData();
                ShowOverExpData();
            }));
            
        }
        private void AddColumnData() {
            var keyRelList = StaticAttribute.Function.selectKeyRelAllUseCase.Execute();

            foreach (var keyRel in keyRelList) {
                string state = keyRel.stat.Equals("REG") ? "키 등록 완료" :
                               keyRel.stat.Equals("WAIT") ? "키 재배포 완료" :
                               keyRel.stat.Equals("DT") ? "키 배포 완료" : "키 삭제";
                if (!keyRel.stat.Equals("DEL")) {
                    RegisteredDataRow.Add(new KeyRelDAO {
                        Num = RegisteredDataRow.Count+1,
                        Timestamp = keyRel.timestamp.ToString("yyyy-MM-dd HH:mm:ss"),
                        StDate = keyRel.stdate.ToString("yyyy-MM-dd"),
                        ExpDate = keyRel.expdate.ToString("yyyy-MM-dd"),
                        Dpt = keyRel.dpt,
                        Ppose = keyRel.ppose,
                        Stat = state,
                        Wkey = keyRel.wkey,
                        Idx = keyRel.idx,
                        DelDate = keyRel.deldate == null ? "" : keyRel.deldate.ToString(),
                        DtDate = keyRel.dtdate == null ? "" : keyRel.dtdate.ToString(),
                        Ijidx = keyRel.ijidx,
                    });
                }
            }
        }
        public void ShowOverExpData() {

            int registeredDataRowCount = RegisteredDataRow.Count;

            List<OverExpKeyDAO> listNum=new List<OverExpKeyDAO>();
            for (int i = 0; i < registeredDataRowCount; i++) {
                TimeSpan timeSpanData = Convert.ToDateTime(RegisteredDataRow[i].ExpDate) - DateTime.Now.Date;

                if (CalculateTimeSpan(timeSpanData).Equals("Green")) {
                    listNum.Add(new OverExpKeyDAO {
                        num = i,
                        stat = "Green"
                    });
                } else if (CalculateTimeSpan(timeSpanData).Equals("Yellow")) {
                    listNum.Add(new OverExpKeyDAO {
                        num = i,
                        stat = "Yellow"
                    });
                } else if (CalculateTimeSpan(timeSpanData).Equals("Orange")) {
                    listNum.Add(new OverExpKeyDAO {
                        num = i,
                        stat = "Orange"
                    });
                } else if (CalculateTimeSpan(timeSpanData).Equals("Red")) {
                    listNum.Add(new OverExpKeyDAO {
                        num = i,
                        stat = "Red"
                    });
                }
            }
            StaticAttribute.Function.expirationKeyTracker.TrackExpirationNotify(listNum);
        }
        private string CalculateTimeSpan(TimeSpan timeSpanData) {
            string result = string.Empty;
            int days = timeSpanData.Days;
            if (days < 31) {
                result = "Green";
                if (days < 21) {
                    result = "Yellow";
                    if (days < 11) {
                        result = "Orange";
                        if (days < 0) {
                            result = "Red";
                        }
                    }
                }
            }
            return result;
        }
        
        internal void ShowKeyRegistrationPage(string dpt, string ppose, string usingData,string expirationData) { //키 생성 요청 창 띄우기
            usingData = usingData.Substring(0, 10);

            KeyRegistrationPage keyRegistrationPage = new KeyRegistrationPage(dpt, ppose, usingData, expirationData);
            keyRegistrationPage.ShowDialog();

            if(StaticAttribute.Function.keyRegistrationPageState) {
                RegistKey(dpt, ppose, usingData, expirationData);
            }

        }
        public void RegistKey(string dpt, string ppose, string usingDate, string expirationDate) { //키 등록 확인 버튼

            StaticAttribute.Function.logCommand.infoLog("[VM.KeyRegistrationManagementPage.User Regist Key]");
            InsertLog(LogEnum.INFO, "암호키 등록 확인 버튼 클릭.");
            bool state = StaticAttribute.Function.insertKeyRelItemUseCase.Execute(wrappedKey, dpt, ppose, usingDate, expirationDate);

            ShowRegisteredData();
            InitializeKeyInfo();

            InformationMessage.InformationShowDialog("암호키를 등록했습니다.");
        }
        internal void ShowKeySendingPage(SelectedKey selectedKey) {
            KeySendingPage keySendingPage = new KeySendingPage(selectedKey);
            keySendingPage.ShowDialog();

            if(StaticAttribute.Function.keySendingPageState) {
                SendKey(selectedKey);
            }
        }

        public void SendKey(SelectedKey selectedKey) {
            bool state = false;
            InsertLog(LogEnum.INFO, "암호키 출고 버튼 클릭");

            if (selectedKey.stat.Equals("DEL")) {
                InformationMessage.InformationShowDialog("삭제된 키는 출고 할 수 없습니다.");
                StaticAttribute.Function.logCommand.warnLog("[VM.KeyRegistrationManagementPage.Deleted Key Can Not be Sent]");
                InsertLog(LogEnum.WARN, "삭제된 키는 출고 할 수 없습니다.");
            } else {
                StaticAttribute.Function.loadingMessage.setMessage("암호키 출고 중..");
                InsertLog(LogEnum.INFO, "암호키 출고 메세지 송신");
                string unwrappedKey = UnwrapKey(selectedKey.wkey);
                string wrappedKeyUsingInjector = WrapKeyUsingInjector(unwrappedKey, StaticAttribute.Function.connectedInjectorDAO.id);
                if (StaticAttribute.Function.connectedInjectorDAO.name.Equals("KIS-100") && StaticAttribute.Function.tcpConnect == 1) {
                    state = TcpSendKey(selectedKey, wrappedKeyUsingInjector);
                } else if(StaticAttribute.Function.connectedInjectorDAO.name.Equals("KIS-200") && StaticAttribute.Function.tcpConnect == 1) {
                    state = TcpSendKey(selectedKey, wrappedKeyUsingInjector);
                }
                string dd = UnwrapKeyUsingInjector(wrappedKeyUsingInjector, StaticAttribute.Function.connectedInjectorDAO.id);
                if (state) {
                    StaticAttribute.Function.updateKeyRelItemUseCase.Execute(selectedKey.idx, StaticAttribute.Function.connectedInjectorDAO.id);
                    StaticAttribute.Function.logCommand.infoLog("[VM.KeyRegistrationManagementPage.Send Reissue Key Success]");
                    InsertLog(LogEnum.INFO, "암호키 출고 성공");
                } else {
                    StaticAttribute.Function.logCommand.warnLog("[VM.KeyRegistrationManagementPage.Send Reissue Key Failed]");
                    InsertLog(LogEnum.WARN, "암호키 출고 실패");
                }
            }
        }

        private bool TcpSendKey(SelectedKey selectedKey, string wrappedKeyUsingIJ) {
            bool state = StaticAttribute.Function.tcpSendUseCase.Execute(Core.StaticAttribute.Enum.typeEnum.REQ, Core.StaticAttribute.Enum.commandEnum.REK, new FromKISM3SendKey {
                grp = selectedKey.idx,
                ppo = selectedKey.ppose,
                st = selectedKey.stDate.ToString("yyyy-MM-dd"),
                exp = Convert.ToInt32((selectedKey.expDate - selectedKey.stDate).TotalDays),
                id = StaticAttribute.Function.connectedInjectorDAO.id,
                key = wrappedKeyUsingIJ
            });
            return state;
        }

        public void ShowKeyDeletionPage(DeleteKey deleteKey) {
            KeyDeletionPage keyDeletionPage = new KeyDeletionPage(deleteKey);
            keyDeletionPage.ShowDialog();

            if(StaticAttribute.Function.keyDeletionPageState) {
                StaticAttribute.Function.deleteKeyRelItemUseCase.Execute(deleteKey.idx);
                if(StaticAttribute.Function.tcpConnect == 1) {
                    string originalKey = UnwrapKey(deleteKey.wkey);
                    StaticAttribute.Function.loadingMessage.setMessage("암호키 삭제 중..");
                    StaticAttribute.Function.tcpSendUseCase.Execute(Core.StaticAttribute.Enum.typeEnum.REQ, Core.StaticAttribute.Enum.commandEnum.KDEL, new FromKISM3KeyDel {
                        skey = StaticAttribute.Function.encryptionCommand.SHA256Hash(originalKey)
                    });                    
                }else {
                    InformationMessage.InformationShowDialog("서버에 저장된 암호키를 삭제했습니다.");
                }
                ShowRegisteredData();
            }
        }
        public bool DuplicateCheck(string grp, string purpose) {
            bool state = true;
            var keyRelAll = StaticAttribute.Function.selectKeyRelAllUseCase.Execute();
            foreach(var keyRelItem in keyRelAll) {
                if(keyRelItem.dpt.Equals(grp) && keyRelItem.ppose.Equals(purpose) && !keyRelItem.stat.Equals("DEL")) {
                    state = false;
                    break;
                }
            }
            return state;
        }
        public void RequestKeyGen() {
            ConnectConfirmColor = Brushes.Orange;
            KeyBackground = Brushes.White;
            KeyOpacity = 1.0;
            InformationMessage.InformationShowDialogExpandSize("생성할 키의 부대와 용도를 선택하고\r\n" +
                                                                                              "       등록 버튼을 눌러주세요.");
        }
        public void OnError(Exception error) {
            throw new NotImplementedException();
        }

        public void OnCompleted() {

        }

        public void changeOpacity() {
            DeleteKeyIconOpacity = 1.0;
            ReInjectionIconOpacity = 1.0;
        }

        public void InitializeKeyInfo() {
            ConnectConfirmColor = Brushes.DarkGray;
            KeyBackground = Brushes.Transparent;
            KeyInformationText = string.Empty;
        }

        public void InsertLog(LogEnum logEnum, string message) {
            StaticAttribute.Function.insertLogInfoItemUseCase.Execute(logEnum, message);
        }

        public void OnNext(LoginTimerDAO value) {
            LoginTime = value.time;
        }
        private string WrapKey(string keyStr) {
            byte[] key = StaticAttribute.Function.hexAndByte.hexToByte(keyStr);
            int keySize = key.Length;
            byte[] macIdStr = StaticAttribute.Function.hexAndByte.StringToBytes(StaticAttribute.Function.getMacAddress.getMacAddress());

            int result = StaticAttribute.Function.wrapKeyUseCase.Execute(keySize, ref key, ref keySize, macIdStr, macIdStr, 0);
            return StaticAttribute.Function.hexAndByte.byteToHex(key);
        }
        private string WrapKeyUsingInjector(string keyStr, string deviceId) {
            byte[] key = StaticAttribute.Function.hexAndByte.hexToByte(keyStr);
            int keySize = key.Length;
            byte[] macIdStr = StaticAttribute.Function.hexAndByte.StringToBytes(StaticAttribute.Function.getMacAddress.getMacAddress());
            byte[] deviceIdByte = StaticAttribute.Function.hexAndByte.StringToBytes(deviceId);

            int result = StaticAttribute.Function.wrapKeyUseCase.Execute(keySize, ref key, ref keySize, macIdStr, deviceIdByte, 0);
            return StaticAttribute.Function.hexAndByte.byteToHex(key);
        }
        private string UnwrapKey(string wkey) {
            byte[] wrappedKey = StaticAttribute.Function.hexAndByte.hexToByte(wkey);
            byte[] rnd = new byte[16];
            rnd = wrappedKey;
            int keySize = rnd.Length;
            int size = keySize;
            byte[] macIdStr = StaticAttribute.Function.hexAndByte.StringToBytes(StaticAttribute.Function.getMacAddress.getMacAddress());

            int result = StaticAttribute.Function.unwrapKeyUseCase.Execute(keySize, ref rnd, ref size, macIdStr, macIdStr, 0);

            return StaticAttribute.Function.hexAndByte.byteToHex(rnd);
        }
        private string UnwrapKeyUsingInjector(string wkey, string deviceId) {
            byte[] wrappedKey = StaticAttribute.Function.hexAndByte.hexToByte(wkey);
            byte[] rnd = new byte[16];
            rnd = wrappedKey;
            int keySize = rnd.Length;
            int size = keySize;
            byte[] macIdStr = StaticAttribute.Function.hexAndByte.StringToBytes(StaticAttribute.Function.getMacAddress.getMacAddress());
            byte[] deviceIdByte = StaticAttribute.Function.hexAndByte.StringToBytes(deviceId);

            int result = StaticAttribute.Function.unwrapKeyUseCase.Execute(keySize, ref rnd, ref size, macIdStr, deviceIdByte, 0);
            return StaticAttribute.Function.hexAndByte.byteToHex(rnd);
        }
    }
}
