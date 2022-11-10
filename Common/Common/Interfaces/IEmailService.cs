namespace Common.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string subject, string address, string name, string content);
    }
}
