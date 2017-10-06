namespace UnitTestExamples.Service
{
    using System.Net.Mail;

    public class EmailService
    {
        private readonly IEmailSender _emailSender;
        private readonly IFromAddressProvider _fromAddressProvider;
        private readonly IEmailValidator _emailValidator;

        public EmailService(IEmailSender emailSender,
            IFromAddressProvider fromAddressProvider,
            IEmailValidator emailValidator)
        {
            _emailSender = emailSender;
            _fromAddressProvider = fromAddressProvider;
            _emailValidator = emailValidator;
        }

        public void SendEmail(string toAddress, string toName, Email email)
        {
            _emailValidator.EnsureEmailIsValid(email);
            if (email.IsValid)
            {
                var to = new MailAddress(toAddress, toName);
                var from = _fromAddressProvider.GetFromAddress();

                _emailSender.Send(to, from, email);

                email.IsSent = true;
            }
        }
    }
}
