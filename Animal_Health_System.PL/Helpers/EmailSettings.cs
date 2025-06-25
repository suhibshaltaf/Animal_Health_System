using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Net;
using System.Net.Mail;

namespace Animal_Health_System.PL.Helpers
{
    public class EmailSettings
    {
        public static void SendEmail(DAL.Models.Email email)
        {
            try
            {
                using (var client = new SmtpClient("smtp.gmail.com", 587))
                {
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential("sohaibshaltafaltrefe@gmail.com", "euby mgzq ywdv vjwb");  // تأكد من استخدام كلمة مرور التطبيق هنا
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress("sohaibshaltafaltrefe@gmail.com"),
                        Subject = email.Subject,
                        Body = email.Body,
                        IsBodyHtml = true
                    };
                    mailMessage.To.Add(email.Recivers);

                    // إرسال البريد الإلكتروني
                    client.Send(mailMessage);
                }
            }
            catch (Exception ex)
            {
                // إضافة سجل الخطأ عند فشل إرسال البريد
                Console.WriteLine($"Error sending email: {ex.Message}");
                // أو استخدام خدمة تسجيل الأخطاء (Log4Net, NLog, إلخ)
            }
        }
    }
}
