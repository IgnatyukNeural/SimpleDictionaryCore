using System.Threading.Tasks;

namespace SimpleDictionary.Services
{
    // Implement this interface to be able to send emails to users 
    // Default mail provider: MailKit
    public interface IMailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
