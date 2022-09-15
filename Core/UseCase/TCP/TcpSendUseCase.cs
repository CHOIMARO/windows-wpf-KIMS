using Core.DAO.JSON;
using Core.StaticAttribute.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UseCase.TCP {
    public class TcpSendUseCase : UseCase{
        private readonly string sig = "TnG";
        private readonly int ver = 1;

        public bool Execute(typeEnum messageType, commandEnum messageCmd, dynamic message) {
            string sendMessage = SetMessage(messageType, messageCmd, message);
            return StaticAttribute.Function.tcpSendService.Send(sendMessage);
        }

        private string SetMessage(typeEnum messageType, commandEnum messageCmd, dynamic message) {

            string messageTypeString = StringEnum.GetStringValue(messageType);
            string messageCmdString = StringEnum.GetStringValue(messageCmd);

            SendToKISDAO dataObj = new SendToKISDAO {
                sig = sig,
                ver = ver,
                type = messageTypeString,
                cmd = messageCmdString,
                msg = message
            };

            var json = JsonConvert.SerializeObject(dataObj);
            return json;
        }
    }
}
