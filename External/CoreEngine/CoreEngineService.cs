using External.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace External.CoreEngine {
    public class CoreEngineService : Service {
        readonly public static int SUCCESS = 0;
        public CoreEngineService() {
            CoreEngineCommand.tngInitCore();

            //tnglib.dll Version 확인
            IntPtr ptr = CoreEngineCommand.tngGetLibVersion();
            string ver = Marshal.PtrToStringAnsi(ptr);
            Console.WriteLine("tnglib.dll Version : " + ver);
        }
        public int tngGenMasterKey(int keyIndex, int bits) {
            return CoreEngineCommand.tngGenMasterKey(keyIndex, bits);
        }

        //public int tngGenMasterKeyNonHsm(ref string priKey, ref string pubKey) {
        //    StringBuilder priKeyBuilder = new StringBuilder(2048);
        //    StringBuilder pubKeyBuilder = new StringBuilder(2048);
        //    int code = CoreEngineCommand.tngGenMasterKeyNonHsm(priKeyBuilder, pubKeyBuilder);
        //    priKey = priKeyBuilder.ToString();
        //    pubKey = pubKeyBuilder.ToString();
        //    return code;
        //}
        public int tngGenMasterKeyNonHsm(ref byte[] priKey, ref byte[] pubKey, int bits) {
            int priKeySize = Marshal.SizeOf(priKey[0]) * priKey.Length;
            IntPtr priKeyData = Marshal.AllocHGlobal(priKeySize);
            Marshal.Copy(priKey, 0, priKeyData, priKeySize);
            int pubKeySize = Marshal.SizeOf(pubKey[0]) * pubKey.Length;
            IntPtr pubKeyData = Marshal.AllocHGlobal(pubKeySize);
            Marshal.Copy(pubKey, 0, pubKeyData, pubKeySize);
            int result = CoreEngineCommand.tngGenMasterKeyNonHsm(out priKeyData, out pubKeyData, bits);
            if (result == SUCCESS) {
                Array.Clear(priKey, 0, priKey.Length);
                Array.Clear(pubKey, 0, pubKey.Length);

                int priLen = PtrLength(priKeyData);
                Marshal.Copy(priKeyData, priKey, 0, priLen);

                int pubLen = PtrLength(pubKeyData);
                Marshal.Copy(pubKeyData, pubKey, 0, pubLen);
            }
            return result;
        }
        public int tngGenRootCA(byte[] path, int keyIndex, byte[] country, byte[] org, byte[] cn, int bits, int days) {
            return CoreEngineCommand.tngGenRootCA(path, path.Length, keyIndex, country, org, cn, bits, days);
        }
        public int tngGenRootCANonHsm(byte[] path, byte[] priKey, byte[] pubKey, byte[] country, byte[] org, byte[] cn, int days) {
            return CoreEngineCommand.tngGenRootCANonHsm(path, path.Length, priKey, pubKey, country, org, cn, days);
        }

        public int tngGenSymmKey(uint keyLen, uint keyIndex) {
            return CoreEngineCommand.tngGenSymmKey(keyLen, keyIndex);
        }

        public int tngWrapKey(byte[] crtStr, int crtStrLen, ref byte[] wrappedKey, ref int wrappedKeyLen, int keyIndex, int sKeySize, int mKeyIndex) {
            IntPtr wrappedKeyData = Marshal.AllocHGlobal(wrappedKeyLen);
            int result = CoreEngineCommand.tngWrapKey(crtStr, crtStrLen, out wrappedKeyData, ref wrappedKeyLen, keyIndex, sKeySize, mKeyIndex);
            if (result == SUCCESS) {
                // Array.Clear(plainTxt, 0, plainTxt.Length);
                Marshal.Copy(wrappedKeyData, wrappedKey, 0, wrappedKeyLen);
            }
            return result;
        }
        public int TngIsAvailableRandomType(int rndType) {
            return CoreEngineCommand.tngIsAvailableRandomType(rndType);
        }


        public int tngGenSymmKeyTest(uint keyLen, uint keyIndex) {
            //return CoreEngineCommand.tngGenSymmKeyTest(keyLen, keyIndex);
            return 0;
        }

        public int tngGenRsaKeyTest(uint keyIndex) {
            //return CoreEngineCommand.tngGenRsaKeyTest(keyIndex);
            return 0;
        }

        public byte[] tngQrngTest(int size) {
            byte[] qrngData = new byte[size];
            for (int index = 0; index < size; index++) {
                //int code = CoreEngineCommand.tngGenRandom(ref qrngData[index], 1);
            }
            return qrngData;

        }

        //public int tngGenRootCA(string path, uint keyIndex, string country, string org, string cn) {
        //    return CoreEngineCommand.tngGenRootCA(path, keyIndex, country, org, cn);
        //}


        private int PtrLength(IntPtr ptr) {
            if (ptr == IntPtr.Zero) return 0;
            int len = 0;
            while (Marshal.ReadByte(ptr, len) != 0) len++;
            return len;
        }

        public string hexString(byte[] bin, int size, ref string buf, ref int len) {
            CoreEngineCommand.tngBinToHex(bin, (uint)size, ref buf, ref len);
            return buf;
        }

        public int TngWrappedSecretKey(int keySize, ref byte[] buf, ref int bufLen, byte[] idStr, byte[] moduleIdStr, int doPadding) {
            int msize = Marshal.SizeOf(buf[0]) * buf.Length;
            IntPtr data = Marshal.AllocHGlobal(msize);
            Marshal.Copy(buf, 0, data, bufLen);
            int result = CoreEngineCommand.tngWrappedSecretKey(keySize, out data, ref bufLen, idStr, idStr.Length, moduleIdStr, moduleIdStr.Length, doPadding);
            Array.Clear(buf, 0, buf.Length);
            if (result == SUCCESS) Marshal.Copy(data, buf, 0, bufLen);
            return result;
        }

        public int TngUnwrappedSecretKey(int keySize, ref byte[] buf, ref int bufLen, byte[] idStr, byte[] moduleIdStr, int doPadding) {
            int msize = Marshal.SizeOf(buf[0]) * buf.Length;
            IntPtr data = Marshal.AllocHGlobal(msize);
            Marshal.Copy(buf, 0, data, bufLen);
            int result = CoreEngineCommand.tngUnWrappedSecretKey(keySize, out data, ref bufLen, idStr, idStr.Length, moduleIdStr, moduleIdStr.Length, doPadding);
            Array.Clear(buf, 0, buf.Length);
            if (result == SUCCESS) Marshal.Copy(data, buf, 0, bufLen);
            return result;
        }

        public int TngGenerateRandomNumber(ref byte[] buf, int size, int rndType) {
            int msize = Marshal.SizeOf(buf[0]) * buf.Length;
            IntPtr data = Marshal.AllocHGlobal(msize);
            int result = CoreEngineCommand.tngGenerateRandomNumber(out data, size, rndType);
            Array.Clear(buf, 0, size);
            if (result == SUCCESS) Marshal.Copy(data, buf, 0, size);
            return result;
        }
    }
}
