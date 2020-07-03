using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Zeus.Lib.WebServices.Models.ServiceSettings;

namespace Zeus.Lib.WebServices.Models.Response
{
    public class AuthenticationResponse : WebServiceResponse
    {
        public string Token { get; set; }
        public string UserID { get; set; }
    }

    //[KnownType(typeof(DynamicSerializable))]
    //[KnownType(typeof(WebServiceStatus))]
    public class WebServiceResponse //: ISerializable
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public object Item { get; set; }
        public object[] Items { get; set; }

        /*
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new System.ArgumentNullException("info");
            info.AddValue("AltName", "XXX");
            info.AddValue("AltID", 9999);
        }//*/
    }

    /*
    [Serializable]
    public class DynamicSerializable : DynamicObject, ISerializable
    {
        private readonly Dictionary<string, object> dictionary = new Dictionary<string, object>();

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            dictionary[binder.Name] = value;
            return true;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            foreach (var kvp in dictionary)
            {
                info.AddValue(kvp.Key, kvp.Value);
            }
        }
    }//*/
}
