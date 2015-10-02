/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OpenIdConnect.Server
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Http;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace AspNet.Security.OpenIdConnect.Server {
    /// <summary>
    /// Provides context information used when issuing an authorization code.
    /// </summary>
    public sealed class CreateAuthorizationCodeContext : BaseControlContext<OpenIdConnectServerOptions> {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateAuthorizationCodeContext"/> class
        /// </summary>
        /// <param name="context"></param>
        /// <param name="options"></param>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <param name="ticket"></param>
        internal CreateAuthorizationCodeContext(
            HttpContext context,
            OpenIdConnectServerOptions options,
            OpenIdConnectMessage request,
            OpenIdConnectMessage response,
            AuthenticationTicket ticket)
            : base(context, options) {
            Request = request;
            Response = response;
            AuthenticationTicket = ticket;
        }

        /// <summary>
        /// Gets the authorization request.
        /// </summary>
        public new OpenIdConnectMessage Request { get; }

        /// <summary>
        /// Gets the authorization response.
        /// </summary>
        public new OpenIdConnectMessage Response { get; }

        /// <summary>
        /// Gets or sets the serializer used to forge the authorization code.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Func<AuthenticationTicket, Task<string>> Serializer { get; set; }

        /// <summary>
        /// Gets or sets the data format used to serialize the authentication ticket.
        /// </summary>
        public ISecureDataFormat<AuthenticationTicket> DataFormat { get; set; }

        /// <summary>
        /// Gets or sets the authorization code returned to the client application.
        /// </summary>
        public string AuthorizationCode { get; set; }

        /// <summary>
        /// Serialize and sign the authentication ticket using <see cref="DataFormat"/>.
        /// Note: the <see cref="AuthorizationCode"/> property
        /// is automatically set when this method completes.
        /// </summary>
        /// <returns>The serialized and signed ticket.</returns>
        public Task<string> SerializeTicketAsync() => SerializeTicketAsync(AuthenticationTicket);

        /// <summary>
        /// Serialize and sign the authentication ticket using <see cref="DataFormat"/>.
        /// Note: the <see cref="AuthorizationCode"/> property
        /// is automatically set when this method completes.
        /// </summary>
        /// <param name="ticket">The authentication ticket to serialize.</param>
        /// <returns>The serialized and signed ticket.</returns>
        public async Task<string> SerializeTicketAsync(AuthenticationTicket ticket) {
            return AuthorizationCode = await Serializer(ticket);
        }
    }
}
