namespace UnitTestExamples.Service.Tests
{
    using System.Net.Mail;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class EmailServiceTests
    {
        private MailAddress _fromAddress;
        private const string Message = "this is my message";
        private const string ToEmail = "othertest@othertest.com";
        private const string ToName = "Mr Other Tester";

        private Mock<IEmailSender> _emailSender;
        private Mock<IFromAddressProvider> _fromAddressProvider;
        private Mock<IEmailValidator> _emailValidator;
        private Mock<Email> _email;

        private EmailService _emailService;

        [SetUp]
        public void Setup()
        {
            _fromAddress = new MailAddress("test@test.com", "Mr Test Test");

            _emailSender = new Mock<IEmailSender>();
            _fromAddressProvider = new Mock<IFromAddressProvider>();
            _emailValidator = new Mock<IEmailValidator>();

            _email = new Mock<Email>();
            _email.Object.Message = Message;

            // Setup return methods like this
            _fromAddressProvider.Setup(x => x.GetFromAddress()).Returns(_fromAddress);

            // Setup void methods like this
            _emailValidator.Setup(x => x.EnsureEmailIsValid(_email.Object)).Callback(() =>
            {
                if (string.IsNullOrEmpty(_email.Object.Message))
                {
                    _email.Object.IsValid = false;
                }
            });

            _emailService = new EmailService(_emailSender.Object, _fromAddressProvider.Object, _emailValidator.Object);
        }

        [Test]
        public void SendEmail_AddressAndNameAndMessage_EmailSent()
        {
            _emailService.SendEmail(ToEmail, ToName, _email.Object);

            // If we want to do a complex comparison like for MailAddress, we can use It.Is
            // However, if we just want to check that a value was passed, we can just pass the value itself (like Message)
            _emailSender.Verify(x => x.Send(It.Is<MailAddress>(m => m.Address == ToEmail && m.DisplayName == ToName),
                _fromAddress,
                _email.Object), Times.Once);

            // Verify is used to check that a method has been called a specified number of times.
            // Assert is used to check the value of a property on an object. 
            Assert.IsTrue(_email.Object.IsSent);
        }

        [Test]
        public void SendEmail_AddressAndNameAndEmptyMessage_EmailNotSent()
        {
            _email.Object.Message = string.Empty;

            _emailService.SendEmail(ToEmail, ToName, _email.Object);

            _emailSender.Verify(x => x.Send(It.IsAny<MailAddress>(), It.IsAny<MailAddress>(), It.IsAny<Email>()), Times.Never);

            Assert.IsFalse(_email.Object.IsSent);
        }
    }
}
