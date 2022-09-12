using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CustomRoles;
using Newtonsoft.Json;
namespace Web.Common
{
    public class Functions : Controller
    {
        public static string generateAlias(string content)
        {
             Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = content.Normalize(NormalizationForm.FormD).Trim();

            string kq= regex.Replace(temp, String.Empty)
                        .Replace('\u0111', 'd')
                        .Replace('\u0110', 'D')
                        .Replace(",", "-")
                        .Replace(".", "-")
                        .Replace("!", "")
                        .Replace("(", "")
                        .Replace(")", "")
                        .Replace(";", "-")
                        .Replace("/", "-")
                        .Replace("%", "ptram")
                        .Replace("&", "va")
                        .Replace("?", "")
                        .Replace('"', '-')
                        .Replace(' ', '-');
            while (kq.Contains("--"))
                kq = kq.Replace("--", "-");
            while (kq.Contains("  "))
                kq = kq.Replace("  ", " ");
            return kq;
        }
        public static bool SendEmail(string userName, string Password, string host, int port, string subject, string body, string email)
        {
            try
            {
                using (var smtpclient = new SmtpClient())
                {
                    smtpclient.EnableSsl = true;
                    smtpclient.Host = host;
                    smtpclient.Port = port;
                    smtpclient.UseDefaultCredentials = true;
                    smtpclient.Credentials = new NetworkCredential(userName, Password);
                    var msg = new MailMessage
                    {
                        IsBodyHtml = true,
                        BodyEncoding = Encoding.UTF8,
                        From = new MailAddress(userName),
                        Subject = subject,
                        Body = body,
                        Priority = MailPriority.Normal,
                    };

                    msg.To.Add(email);

                    smtpclient.Send(msg);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        public bool MakeSessionFromCookie(HttpRequestBase Request, HttpResponseBase Response, HttpContextBase HttpContext)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                CustomPrincipalSerializeModel serializeModel = JsonConvert.DeserializeObject<CustomPrincipalSerializeModel>(authTicket.UserData);
                CustomPrincipal newUser = new CustomPrincipal(authTicket.Name);
                newUser.AccountId = serializeModel.AccountId;
                newUser.FirstName = serializeModel.FirstName;
                newUser.LastName = serializeModel.LastName;
                newUser.Email = serializeModel.Email;
                newUser.roles = serializeModel.roles;
                // create new 15 minutes for cookie
                string userData = JsonConvert.SerializeObject(serializeModel);
                FormsAuthenticationTicket authTicket2 = new FormsAuthenticationTicket(
                           1,
                          newUser.Email,
                           DateTime.Now,
                           DateTime.Now.AddMinutes(15),
                           false,
                           userData);
                string encTicket = FormsAuthentication.Encrypt(authTicket);
                HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                Response.Cookies.Add(faCookie);
                HttpContext.Session["bds_Acc_id"] = newUser.AccountId;
                return true;
            }
            else
            {
                return false;
            }
        }
        //public long getIDSession(HttpContextBase HttpContext)
        //{
        //    var id_session = Convert.ToInt32(HttpContext.Session["bds_Acc_id"]);
        //    if (id_session == null || id_session == 0)
        //    {
        //        return -1;
        //    }
        //    return id_session;
        //}
    }

}