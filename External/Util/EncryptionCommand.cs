using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace External.Util {
    public class EncryptionCommand {
        private const string logo = "TNGEN";
        internal string encBase64(string data) {
            byte[] bytes = Encoding.Unicode.GetBytes(data);
            return Convert.ToBase64String(bytes);
        }

        public string SHA256Hash(string data) {

            SHA256 sha = new SHA256Managed();
            byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes(data));
            StringBuilder stringBuilder = new StringBuilder();
            try {
                foreach (byte b in hash) {
                    stringBuilder.AppendFormat("{0:x2}", b);
                }
            } catch (Exception ) {
                
            }
            return stringBuilder.ToString();
        }
        public string dataHashing(string userId, string userPwd) {
            string id = SHA256Hash(userId);
            string password = SHA256Hash(userPwd);
            string salt = SHA256Hash(logo);                  //20200427 salt 추가
            string data = id + salt + password;
            string hashPassword = SHA256Hash(data);

            return hashPassword.ToUpper();
        }


        public string SHA1Hash(string data) {
            using (SHA1Managed sha1 = new SHA1Managed()) {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(data));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash) {
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString();
            }
        }
    }
}
