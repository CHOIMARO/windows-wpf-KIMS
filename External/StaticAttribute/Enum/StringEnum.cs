using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace External.StaticAttribute.Enum {
    public static class StringEnum {
        public static string GetStringValue(dynamic value) {
            string output = null;
            Type type = value.GetType();
            FieldInfo fi = type.GetField(value.ToString());
            StringValue[] attrs = fi.GetCustomAttributes(typeof(StringValue), false) as StringValue[];
            if (attrs.Length > 0) {
                output = attrs[0].Value;
            }
            return output;
        }
    }

    public class StringValue : Attribute {
        private string value;
        public StringValue(string value) {
            this.value = value;
        }
        public string Value {
            get {
                return value;
            }
        }
    }
}
