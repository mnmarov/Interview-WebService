using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace Zeus.Lib.WebServices.Interfaces
{
    [ServiceContract]
    public interface IWebServiceHTTP
    {
        [OperationContract, WebGet(UriTemplate = "/{resource}.{extension}")]
        Stream Files(string resource, string extension);

        [OperationContract, WebGet(UriTemplate = "*")]
        Stream Handle404();
    }
}
