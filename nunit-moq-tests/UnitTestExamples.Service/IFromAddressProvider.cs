namespace UnitTestExamples.Service
{
    using System.Net.Mail;

    public interface IFromAddressProvider
    {
        MailAddress GetFromAddress();
    }
}
