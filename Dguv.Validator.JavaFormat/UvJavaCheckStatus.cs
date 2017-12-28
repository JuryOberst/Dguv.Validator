namespace Dguv.Validator.JavaFormat
{
    public class UvJavaCheckStatus : IStatus
    {
        private readonly string _message;

        public UvJavaCheckStatus(int code, string message)
        {
            _message = message;
            Code = code;
        }

        public int Code { get; }

        public bool IsSuccessful => Code == 0;

        public string GetStatusText() => _message;
    }
}
