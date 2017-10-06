namespace UnitTestExamples.Service
{
    public class Email
    {
        public Email()
        {
            IsValid = true;
        }

        public string Message { get; set; }

        public bool IsSent { get; set; }

        public bool IsValid { get; set; }
    }
}
