using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace External.Util {
    static internal class CoreEngineCommand {
        [DllImport("tnglib.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern internal IntPtr tngBinToHex(byte[] bin, uint size, ref string buf, ref int len);

        [DllImport("tnglib.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern internal int tngInitCore();

        [DllImport("tnglib.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern internal IntPtr tngGetJniVersion();

        [DllImport("tnglib.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern internal IntPtr tngGetLibVersion();

        [DllImport("tnglib.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern internal int tngGetKeyMax();

        [DllImport("tnglib.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern internal int tngIsAvailableKeyIndex(int keyIndex);


        [DllImport("tnglib.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern internal int tngIsAvailableRandomType(int rndType);

        [DllImport("tnglib.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern internal int tngGenerateRandomNumber(out IntPtr buf, int size, int rndType);

        //[DllImport("libtngcore.dll", CallingConvention = CallingConvention.Cdecl)]
        //static extern internal int tngGenerateRandomNumber(out IntPtr buf, int size, int rndType);

        [DllImport("tnglib.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern internal int tngWrappedSecretKey(int keySize, out IntPtr buf, ref int bufLen, byte[] idStr, int idStrLen, byte[] moduleIdStr, int moduleIdStrLen, int doPadding);

        [DllImport("tnglib.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern internal int tngUnWrappedSecretKey(int keySize, out IntPtr buf, ref int bufLen, byte[] idStr, int idStrLen, byte[] moduleIdStr, int moduleIdStrLen, int doPadding);

        [DllImport("tnglib.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern internal int tngGenSymmKey(uint keyLen, uint keyIndex);

        [DllImport("tnglib.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern internal int tngGenAsymKey(uint keyIndex);


        [DllImport("tnglib.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern internal int tngGenMasterKey(int keyIndex, int bits);

        [DllImport("tnglib.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern internal int tngGenMasterKeyNonHsm(out IntPtr priKey, out IntPtr pubKey, int bits);

        [DllImport("tnglib.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern internal int tngGenRootCA(byte[] path, int pathLen, int keyIndex, byte[] country, byte[] org, byte[] cn, int bits, int days);

        [DllImport("tnglib.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern internal int tngGenRootCANonHsm(byte[] path, int pathLen, byte[] priKey, byte[] pubKey, byte[] country, byte[] org, byte[] cn, int days);

        [DllImport("tnglib.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern internal int tngGenSubCA(byte[] path, int pathLen, int keyIndex, byte[] country, byte[] org, byte[] cn, int bits, int days, int mKeyIndex, byte[] rootCaPath, int rootCaPathLen);


        [DllImport("tnglib.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern internal int tngGenCSR(out IntPtr outCsr, ref int outCsrLen, int keyIndex);

        [DllImport("tnglib.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern internal int tngGenCRT(byte[] rootCaPath, int rootCaPathLen, out IntPtr crtStr, ref int crtLen, byte[] csrStr, int days);



        [DllImport("tnglib.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern internal int tngImportSymmKey(byte[] key, int keyLen, int keyIndex);

        [DllImport("tnglib.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern internal int tngImportAsymRsaKey(byte[] priKey, int priKeyLen, byte[] pubKey, int pubKeyLen, int keyIndex);

        [DllImport("tnglib.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern internal int tngImportAsymEccKey(byte[] priKey, int priKeyLen, byte[] pubKeyX, int pubKeyXLen, byte[] pubKeyY, int pubKeyYLen, int keyIndex); // 구현 아직 안됨




        [DllImport("tnglib.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern internal int tngEncAsym(byte[] plainTxt, int plainTxtLen, out IntPtr cipherTxt, ref int cipherTxtLen, int keyType, int keyIndex);

        [DllImport("tnglib.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern internal int tngDecAsym(byte[] cipherTxt, int cipherTxtLen, out IntPtr plainTxt, ref int plainTxtLen, int keyType, int keyIndex);


        [DllImport("tnglib.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern internal int tngPEMEncAsym(byte[] plainTxt, int plainTxtLen, out IntPtr cipherTxt, ref int cipherTxtLen, int keyType, byte[] pem);

        [DllImport("tnglib.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern internal int tngPEMDecAsym(byte[] cipherTxt, int cipherTxtLen, out IntPtr plainTxt, ref int plainTxtLen, int keyType, byte[] pem);







        [DllImport("tnglib.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern internal int tngHmacSha(int shaType, byte[] msg, int msgLen, out IntPtr digest, ref int digestLen);

        [DllImport("tnglib.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern internal int tngSha(int shaType, byte[] msg, int msgLen, out IntPtr digest, ref int digestLen);


        [DllImport("tnglib.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern internal int tngSign(byte[] msg, int msgLen, out IntPtr encMsg, ref int encMsgLen, int keyIndex, int shaType);

        [DllImport("tnglib.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern internal int tngVerify(byte[] encMsg, int encMsgLen, byte[] msg, int msgLen, int keyIndex, int shaType);


        [DllImport("tnglib.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern internal int tngPEMSign(byte[] msg, int msgLen, out IntPtr encMsg, ref int encMsgLen, byte[] pem, int shaType);

        [DllImport("tnglib.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern internal int tngPEMVerify(byte[] encMsg, int encMsgLen, byte[] msg, int msgLen, byte[] pem, int shaType);

        [DllImport("tnglib.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern internal int tngCrtVerify(byte[] encMsg, int encMsgLen, byte[] msg, int msgLen, byte[] caCrtStr, int caCrtStrLen, int shaType);


        [DllImport("tnglib.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern internal int tngEncSymm(byte[] plainTxt, int plainTxtLen, out IntPtr cipherTxt, ref int cipherTxtLen, int keyIndex, int doPadding);

        [DllImport("tnglib.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern internal int tngDecSymm(byte[] cipherTxt, int cipherTxtLen, out IntPtr plainTxt, ref int plainTxtLen, int keyIndex, int doPadding);


        [DllImport("tnglib.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern internal int tngWrapKey(byte[] crtStr, int crtStrLen, out IntPtr wrappedKey, ref int wrappedKeyLen, int keyIndex, int sKeySize, int mKeyIndex);


        [DllImport("tnglib.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern internal int tngUnwrapKey(byte[] wrappedKey, int wrappedKeyLen, out IntPtr unWrappedKey, ref int unWrappedKeyLen, int sKeySize, int keyIndex, byte[] caCrtPath);



        [DllImport("tnglib.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern internal void tngInitDebug(int size);

        [DllImport("tnglib.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern internal void tngReleaseDebug();

        [DllImport("tnglib.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern internal void tngPrintDebug(string log);

        [DllImport("tnglib.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern internal void tngGetDebug(out IntPtr buff);
    }
}
