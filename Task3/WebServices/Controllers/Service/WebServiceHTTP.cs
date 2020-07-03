using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using Zeus.Lib.WebServices.Interfaces;
using Zeus.Lib.WebServices.Models.Authorization;

namespace Zeus.Lib.WebServices.Controllers.Service
{
    [ServiceBehavior(InstanceContextMode =
      InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class WebServiceHTTP : IWebServiceHTTP
    {
        public static string WebServiceBaseAddress = "";
        public static string AssemblyWebsiteDir = "web";

        public bool SetupResponseFormat(string extension)
        {
            extension = extension.ToLower();
            switch (extension)
            {
                //http://www.w3schools.com/media/media_mimeref.asp
                case "htm":
                case "html":
                    //if ((String.Compare(resource, "index") != 0) && (String.Compare(resource, "login") != 0))
                    //{
                    //    if (!Authenticate()) return null;
                    //}
                    WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
                    break;
                case "js":
                    WebOperationContext.Current.OutgoingResponse.ContentType = "text/javascript";
                    break;
                case "css":
                    WebOperationContext.Current.OutgoingResponse.ContentType = "text/css";
                    break;
                case "png":
                    WebOperationContext.Current.OutgoingResponse.ContentType = "image/png";
                    break;
                case "ico":
                    WebOperationContext.Current.OutgoingResponse.ContentType = "image/x-icon";
                    break;
                default:
                    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.UnsupportedMediaType;
                    return false;
            }
            return true;
        }

        public Stream Files(string resource, string extension)
        {
            bool success = SetupResponseFormat(extension);
            if (!success)
                return new MemoryStream(Encoding.ASCII.GetBytes("File type not supported"), false);

            if (!WebServiceAuthorization.isAuthorized())
            {
                if (resource.ToLower().Equals("login"))
                {
                    // continue
                }
                else
                {
                    switch (extension)
                    {
                        case "htm":
                        case "html":
                            WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Redirect;
                            WebOperationContext.Current.OutgoingResponse.Headers.Add("Location", "Login.html");
                            return new MemoryStream(Encoding.ASCII.GetBytes("Redirecting to login"), false);
                    }
                }
            }

            Stream resStream = GetFileStream(resource, extension);
            if (resStream == null)
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.NotFound;
                return new MemoryStream(Encoding.ASCII.GetBytes("File not found."), false);
            }
            else if (extension.ToLower().Equals("js"))
            {
                // Inject webservice base url in all javascript files
                // so that we know what to call
                resStream = AddBaseURL(resStream);
            }
            return resStream;
        }

        private Stream AddBaseURL(Stream resStream)
        {
            StreamReader sr = new StreamReader(resStream);
            string jsFile = sr.ReadToEnd();
            byte[] bytes = Encoding.ASCII.GetBytes(jsFile);
            byte[] baseURL = Encoding.ASCII.GetBytes("var WebServiceBaseAddress = '" + WebServiceBaseAddress + "';\r\n");
            resStream = new MemoryStream();
            resStream.Write(baseURL, 0, baseURL.Count());
            resStream.Write(bytes, 0, bytes.Count());
            resStream.Seek(0, SeekOrigin.Begin);
            return resStream;
        }

        private Stream GetFileStream(string resource, string extension)
        {
            string fileName = "";
            Stream resStream = null;
            // PRIORITY
            // 1. File System
            // 2. Entry Assembly eg: OMSServiceWindows.exe
            // 3. Executing assembly eg: Zeus.Lib.WebServices.dll


            Assembly ass = Assembly.GetEntryAssembly();
            // 1. Check outside first
            string webPath = Path.GetDirectoryName(ass.Location) + "\\" + AssemblyWebsiteDir + "\\";
            fileName = webPath + resource + "." + extension;

            if (File.Exists(fileName))
            {
                resStream = new FileStream(fileName, FileMode.Open);
            }
            else
            {
                // 2. Get the file from inside the start binary
                resStream = GetAssemblyFileStream(ass, resource, extension);
                // 3. Get the file from inside excuting binary
                if (null == resStream)
                {
                    ass = Assembly.GetExecutingAssembly();
                    resStream = GetAssemblyFileStream(ass, resource, extension);
                }
            }
            return resStream;
        }

        private Stream GetAssemblyFileStream(Assembly ass, string resource, string extension)
        {
            return GetAssemblyFileStream(ass, AssemblyWebsiteDir, resource, extension);
        }

        private Stream GetAssemblyFileStream(Assembly ass, string path, string resource, string extension)
        {
            string fileName = "";
            Stream resStream = null;
            string assNamespace = ass.FullName.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)[0];

            /*
            Type innerType = ass.ExportedTypes.FirstOrDefault();
            if (innerType != null)
                assNamespace = innerType.Namespace;
            //*/

            fileName = String.Format(assNamespace + ".{0}.{1}.{2}", path, resource, extension);
            resStream = ass.GetManifestResourceStream(fileName);
            return resStream;
        }

        public Stream Handle404()
        {
            WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Redirect;
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Location", "Login.html");
            return new MemoryStream(Encoding.ASCII.GetBytes("Redirecting to login"), false);
        }
    }

}
