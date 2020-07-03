using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using Zeus.Lib.WebServices.Interfaces;
using Zeus.Lib.WebServices.Models.Response;

namespace WebService
{
    [ServiceContract]
    public interface ITestWebService : IWebService
    {
        [OperationContract]
        [WebInvoke(Method = "OPTIONS", UriTemplate = "/UploadText",
        RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void UploadTextOptions();

        [OperationContract]
        [WebInvoke(Method = "OPTIONS", UriTemplate = "/Upload",
        RequestFormat = WebMessageFormat.Xml, ResponseFormat = WebMessageFormat.Xml)]
        void UploadOptions();

        [OperationContract]
        [WebInvoke(Method = "OPTIONS", UriTemplate = "/Upload2",
        RequestFormat = WebMessageFormat.Xml, ResponseFormat = WebMessageFormat.Xml)]
        void Upload2Options();
        

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/Upload",
        RequestFormat = WebMessageFormat.Xml, ResponseFormat = WebMessageFormat.Xml)]
        WebServiceResponse Upload(XmlDocument request);


        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/UploadText",
        RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        WebServiceResponse UploadText(string request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/Upload2")]
        WebServiceResponse Upload2(HttpPostedFileBase file);



    }
}
