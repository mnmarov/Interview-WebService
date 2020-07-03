using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using Zeus.Lib.WebServices.Models.Response;

namespace WebService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class TestWebService : Zeus.Lib.WebServices.Controllers.Service.WebService, ITestWebService
    {
        public void UploadOptions()
        {
        }

        public void Upload2Options()
        {
        }

        public void UploadTextOptions()
        {
        }


        public WebServiceResponse Upload2(HttpPostedFileBase file)
        {
            return null;
        }

        public WebServiceResponse Upload(XmlDocument request)
        {
            var result = new WebServiceResponse();
            result.Status = 0;
            var commandNodes = request.SelectNodes("//DeclarationList/Declaration");
            bool command_found = false;
            bool dub_found = false;
            foreach (XmlElement node in commandNodes)
            {
                var command = node.GetAttribute("Command");
                // Process commands here
                if (command.Trim().ToUpper().Equals("DEFAULT"))
                {
                    command_found = true;
                    var SiteIDnode = node.SelectSingleNode("DeclarationHeader/SiteID");
                    if (SiteIDnode != null)
                    {
                        if (SiteIDnode.InnerText.Trim().ToUpper().Equals("DUB"))
                        {
                            dub_found = true;
                        }
                    }
                }
            }
            if (!command_found)
            {
                result.Status = -1;
                result.Message = "Invalid command specified.";
            }
            else if (!dub_found)
            {
                result.Status = -2;
                result.Message = "Invalid Site specified.";
            }

            return result;
        }

        public WebServiceResponse UploadText(string request)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.LoadXml(request);
            }
            catch(Exception ex)
            {
                return new WebServiceResponse()
                {
                    Status = -3,
                    Message = "Invalid XML",
                };
            }
            return Upload(doc);
        }

    }
}
