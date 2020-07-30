using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Threading.Tasks;

namespace BookStore.IdentityProvider
{
    public class CustomEmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            //sending email logic here
            using (System.IO.StreamWriter file = new System.IO.StreamWriter($"identityEmail-{DateTime.UtcNow.Millisecond}.txt"))
            {
                await file.WriteAsync(htmlMessage);
            }
        }
    }
}
