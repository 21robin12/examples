namespace UnitTestExamples.Service
{
    using System.Net.Mail;

    public interface IEmailSender
    {
        void Send(MailAddress to, MailAddress from, Email email);
    }
}
