using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Security.Principal;
namespace CustomRoles
{
    public class CustomPrincipal : IPrincipal
    {
        public IIdentity Identity { get; private set; }
        public bool IsInRole(string role)
        {
            if (roles.Any(r => role.Contains(r)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public CustomPrincipal(string Username)
        {
            this.Identity = new GenericIdentity(Username);
        }

        public long AccountId { get; set; }
        public string Email { get; set; }
        // UsernameId = Email: quanghoahcm@gmail.com + Id: 15 = quanghoahcm15
        public string UserNameId { get; set; }       
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<string> roles { get; set; }
    }

    public class CustomPrincipalSerializeModel
    {

        public long AccountId { get; set; }
        public string Email { get; set; }
        // UsernameId = Email: quanghoahcm@gmail.com + Id: 15 = quanghoahcm15
        public string UserNameId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<string> roles { get; set; }
    }
}