using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Common.Infrastructure.Http
{
    public class IgnoreSSLValidateDelegatingHandler : DelegatingHandler
    {
        // this commented block could be useful if you want to allow a defined
        // group of certicates, rather than just anything out in the wild.
        // -------------------------------------------------------------------
        //private readonly X509CertificateCollection _certificates;
        //public IgnoreSSLValidateDelegatingHandler()
        //{
        //  _certificates = new X509CertificateCollection();
        //}

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            System.Threading.CancellationToken cancellationToken)
        {
            HttpMessageHandler handler = this.InnerHandler;

            while (handler is DelegatingHandler)
            {
                handler = ((DelegatingHandler)handler).InnerHandler;
            }

            if (handler is HttpClientHandler httpClientHandler
                && httpClientHandler.ServerCertificateCustomValidationCallback == null)
            {
                httpClientHandler
                    .ServerCertificateCustomValidationCallback =
                        (message, cert, chain, errors) =>
                        {
                            // It is possible inpect the certificate provided by server
                            Console.WriteLine($"Requested URI: {message.RequestUri}");
                            Console.WriteLine($"Effective date: {cert.GetEffectiveDateString()}");
                            Console.WriteLine($"Exp date: {cert.GetExpirationDateString()}");
                            Console.WriteLine($"Issuer: {cert.Issuer}");
                            Console.WriteLine($"Subject: {cert.Subject}");

                            // Based on the custom logic it is possible to decide whether the client considers certificate valid or not
                            Console.WriteLine($"Errors: {errors}");
                            return true;

                        };
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}
