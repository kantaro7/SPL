namespace SPL.Security.Application.Commands.Security
{
    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Reports;
    using SPL.Domain.SPL.Reports.CGD;
    using SPL.Domain.SPL.Reports.FPB;
    using SPL.Domain.SPL.Reports.FPC;
    using SPL.Domain.SPL.Security;

    using System.Collections.Generic;
    using System.Security;

    public class LoginUserCommand : IRequest<ApiResponse<long>>
    {
        public LoginUserCommand(IEnumerable<string> pScopes, string pUsername, SecureString pPassword)
        {
            this.Scopes = pScopes;
            this.Password = pPassword;
            this.Username = pUsername;
        }
        #region Constructor
        public IEnumerable<string> Scopes { get; }
        public SecureString Password { get; }
        public string Username { get; }
        #endregion
    }
}
