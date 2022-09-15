using Core.UseCase.TCP;
using KISM.IObservable.TCP;
using KISM.Util;
using Log.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KISM.IObservable.MainWindow;
using KISM.DAO.TCP;
using KISM.IObservable.KeyRegistrationManagementPage;
using KISM.IObservable;
using KISM.DAO.Serial;
using Core.UseCase.DB.comminfo;
using Core.UseCase.DB.injectorinfo;
using KISM.View.Loading;
using Core.UseCase.DB.injectormgr;
using Core.UseCase.DB.keyrel;
using Core.UseCase.DB.milunitinfo;
using Core.UseCase.DB.pposeinfo;
using Core.UseCase.TngLib;
using Core.UseCase.DB.dtinfo;
using Core.UseCase.DB.loginfo;
using Core.UseCase.DB.accountinfo;

namespace KISM.StaticAttribute {
    public static class Function {

        #region Usecase
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
        //renewal
        static public TcpReceivedUseCase tcpReceivedUseCase = new TcpReceivedUseCase();
        static public TcpConnectUseCase tcpConnectUseCase = new TcpConnectUseCase();
        static public TcpSendUseCase tcpSendUseCase = new TcpSendUseCase();

        //renewal DB
        static public SelectCommInfoAllUseCase selectCommInfoAllUseCase = new SelectCommInfoAllUseCase();
        static public InsertInjectorInfoItemUseCase insertInjectorInfoItemUseCase = new InsertInjectorInfoItemUseCase();
        static public InsertInjectorMgrItemUseCase insertInjectorMgrItemUseCase = new InsertInjectorMgrItemUseCase();
        static public SelectInjectorInfoItemUseCase selectInjectorInfoItemUseCase = new SelectInjectorInfoItemUseCase();
        static public SelectInjectorMgrListUseCase selectInjectorMgrListUseCase = new SelectInjectorMgrListUseCase();
        static public SelectKeyRelAllUseCase selectKeyRelAllUseCase = new SelectKeyRelAllUseCase();
        static public SelectMilUnitInfoAllUseCase selectMilUnitInfoAllUseCase = new SelectMilUnitInfoAllUseCase();
        static public SelectPposeInfoAllUseCase selectPposeInfoAllUseCase = new SelectPposeInfoAllUseCase();
        static public InsertKeyRelItemUseCase insertKeyRelItemUseCase = new InsertKeyRelItemUseCase();
        static public UpdateKeyRelItemUseCase updateKeyRelItemUseCase = new UpdateKeyRelItemUseCase();
        static public DeleteKeyRelItemUseCase deleteKeyRelItemUseCase = new DeleteKeyRelItemUseCase();
        static public SelectDtInfoAllUseCase selectDtInfoAllUseCase = new SelectDtInfoAllUseCase();
        static public InsertDtInfoListUseCase insertDtInfoListUseCase = new InsertDtInfoListUseCase();
        static public InsertMilUnitInfoItemUseCase insertMilUnitInfoItemUseCase = new InsertMilUnitInfoItemUseCase();
        static public DeleteMilUnitInfoItemUseCase deleteMilUnitInfoItemUseCase = new DeleteMilUnitInfoItemUseCase();
        static public UpdateMilUnitInfoItemUseCase updateMilUnitInfoItemUseCase = new UpdateMilUnitInfoItemUseCase();
        static public InsertPposeInfoItemUseCase insertPposeInfoItemUseCase = new InsertPposeInfoItemUseCase();
        static public UpdatePposeInfoItemUseCase updatePposeInfoItemUseCase = new UpdatePposeInfoItemUseCase();
        static public DeletePposeInfoItemUseCase deletePposeInfoItemUseCase = new DeletePposeInfoItemUseCase();
        static public InsertLogInfoItemUseCase insertLogInfoItemUseCase = new InsertLogInfoItemUseCase();
        static public SelectLogInfoAllUseCase selectLogInfoAllUseCase = new SelectLogInfoAllUseCase();
        static public InsertCommInfoItemUseCase insertCommInfoItemUseCase = new InsertCommInfoItemUseCase();
        static public SelectAccountInfoAllUseCase selectAccountInfoAllUseCase = new SelectAccountInfoAllUseCase();
        static public InsertAccountInfoItemUseCase InsertAccountInfoItemUseCase = new InsertAccountInfoItemUseCase();
        static public UpdateAccountInfoItemUseCase updatedAccountInfoItemUseCase = new UpdateAccountInfoItemUseCase();
        static public DeleteAccountInfoItemUseCase deleteAccountInfoItemUseCase = new DeleteAccountInfoItemUseCase();
        static public CheckedAccountInfoItemUseCase checkedAccountInfoItemUseCase = new CheckedAccountInfoItemUseCase();

        //renewal CoreEngine (TngLib.dll)
        static public FindAvailableRndTypeUseCase findAvailableRndTypeUseCase = new FindAvailableRndTypeUseCase();
        static public GenerateKeyUseCase generateKeyUseCase = new GenerateKeyUseCase();
        static public WrapKeyUseCase wrapKeyUseCase = new WrapKeyUseCase();
        static public UnwrapKeyUseCase unwrapKeyUseCase = new UnwrapKeyUseCase();

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------


        #endregion

        #region Observer
        //renewal
        static public ExpirationKeyTracker expirationKeyTracker = new ExpirationKeyTracker();
        static public LoginTimerTracker loginTimerTracker = new LoginTimerTracker();
        static public MovePageTracker movePageTracker = new MovePageTracker();

        static public TcpReceivedDataTracker tcpReceivedDataTracker2 = new TcpReceivedDataTracker();
        static public TcpIsConnectTracker tcpIsConnectTracker = new TcpIsConnectTracker();
        #endregion

        #region Util
        static public bool accountState = false;
        static public bool authState = false;

        static public bool movePageKeyGen = false; //키 생성 시 확인을 눌러 생성 페이지로 이동하면 true 아니면 false
        static public bool movePageHistorySave = false; //메인 페이지에서 다른 페이지로 이동하면 true 다시 메인페이지로 돌아오면 false

        static public bool serialConnect = false;
        static public int tcpConnect = 0;

        static public string injectorID = string.Empty;
        static public string injectorPW = string.Empty;

        static public bool keyGenerationPageState = false;
        static public bool keyRegistrationPageState = false;
        static public bool keySendingPageState = false;
        static public bool keyDeletionPageState = false;
        static public bool historyInitializePageState = false;
        static public bool deleteMilUnitInfoItemPageState = false;
        static public bool deletePposeInfoItemPageState = false;
        static public bool logoutPageState = false;
        static public bool tcpDisconnectPageState = false;
        static public bool deleteAccountRequestPageState = false;

        static public ConnectedInjectorDAO connectedInjectorDAO = new ConnectedInjectorDAO();
        static internal EncryptionCommand encryptionCommand = new EncryptionCommand();
        static internal ToastMessage toastMessage = new ToastMessage();
        static internal LogCommand logCommand = new LogCommandImpl();
        static internal HexAndByte hexAndByte = new HexAndByte();
        static internal GetMacAddress getMacAddress = new GetMacAddress();
        static public SerialInfoDAO serialInfoDAO = new SerialInfoDAO();
        static public ProcessingDBData processingDBData = new ProcessingDBData();

        static public LoadingWindow loadingWindow = new LoadingWindow();
        static public LoadingMessage loadingMessage = new LoadingMessage();

        #endregion
    }
}
