using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace Zeus.Lib.WebServices.Controllers
{
    public class WebHttpBehaviorWithErrors : WebHttpBehavior
    {
        internal sealed class WebHttpErrorHandler : IErrorHandler
        {
            public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
            {
                var exception = new FaultException("Web Server error encountered. All details have been logged.");
                var messageFault = exception.CreateMessageFault();
                //fault = Message.CreateMessage(version, messageFault, exception.Action);
            }

            public bool HandleError(Exception error)
            {
                return !(error is FaultException);
            }
        }

        protected override void AddServerErrorHandlers(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            base.AddServerErrorHandlers(endpoint, endpointDispatcher);
            endpointDispatcher.DispatchRuntime.ChannelDispatcher.ErrorHandlers.Add(new WebHttpErrorHandler());
        }
    }
}
