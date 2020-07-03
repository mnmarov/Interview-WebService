using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Zeus.Lib.WebServices.Models.ServiceSettings;

namespace Zeus.Lib.WebServices.Controllers.Service
{
    public  static class WebServiceKnownTypesProvider
    {
        public static List<Type> KnownTypes = new List<Type>();
        public static IEnumerable<Type> GetKnownTypes(ICustomAttributeProvider provider)
        {
            return KnownTypes;
        }
    }
}
