using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using BLL.DomainClasses;

namespace GoldStreamer.Helpers
{
    public class UsefulMethods
    {
        public static decimal? string2decimal(string incomingValue)
        {
            decimal val;
            if (!decimal.TryParse(incomingValue.Replace(",", "").Replace(".", ""), NumberStyles.Number, CultureInfo.InvariantCulture, out val))
                return null;
            return val / 100;
        }
        public static string ConvertToEasternArabicNumbers(string input)
        {
            System.Text.UTF8Encoding utf8Encoder = new UTF8Encoding();
            System.Text.Decoder utf8Decoder = utf8Encoder.GetDecoder();
            System.Text.StringBuilder convertedChars = new System.Text.StringBuilder();
            char[] convertedChar = new char[1];
            byte[] bytes = new byte[] { 217, 160 };
            char[] inputCharArray = input.ToCharArray();
            foreach (char c in inputCharArray)
            {
                if (char.IsDigit(c))
                {
                    bytes[1] = Convert.ToByte(160 + char.GetNumericValue(c));
                    utf8Decoder.GetChars(bytes, 0, 2, convertedChar, 0);
                    convertedChars.Append(convertedChar[0]);
                }
                else
                {
                    convertedChars.Append(c);
                }
            }
            return convertedChars.ToString();
        }
        public static string GenerateNewPassword()
        {
            string allowedChars = "";
            string password = System.Web.Security.Membership.GeneratePassword(12, 0);
            StringBuilder sb = new StringBuilder();
            Random r = new Random();

            allowedChars = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,";
            allowedChars += "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,";
            allowedChars += "1,2,3,4,5,6,7,8,9,0";
            char[] sep = { ',' };
            string[] arr = allowedChars.Split(sep);
            foreach (char c in password)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
                {
                    sb.Append(c);
                }
                else
                {

                    sb.Append(arr[r.Next(0, arr.Length - 1)]); ;
                }
            }
            return sb.ToString();
        }

        public static Task SendAsync(Mail message,bool isHtml=true)
        {

 
            // Plug in your email service here to send an email.
            // Credentials:
            var credentialUserName = "smartresp2017@gmail.com";
            var sentFrom = "smartresp2017@gmail.com";
            var pwd = "Tempura15";

            // Configure the client:
            System.Net.Mail.SmtpClient client =
                new System.Net.Mail.SmtpClient("smtp.mail.yahoo.com");

            client.Port = 587;
            client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;

            // Create the credentials:
            System.Net.NetworkCredential credentials =
                new System.Net.NetworkCredential(credentialUserName, pwd);

            client.EnableSsl = true;
            client.Credentials = credentials;

            // Create the message:
            var mail =
                new System.Net.Mail.MailMessage(sentFrom, message.Destination);
            mail.IsBodyHtml = isHtml;
            mail.Subject = message.Subject;
            mail.Body = message.Body;

            // Send:

            return client.SendMailAsync(mail);
            //return Task.FromResult(0);
        }
        public static string[] GetPagingOptions()
        {
            string pagingOptions = System.Configuration.ConfigurationManager.AppSettings["PagingOptions"] != null ? System.Configuration.ConfigurationManager.AppSettings["PagingOptions"].ToString() : "";
            if (!string.IsNullOrEmpty(pagingOptions))
            {
                return pagingOptions.Split(',');
            }
            else
            {
                return new string[0];
            }
        }
    }
}