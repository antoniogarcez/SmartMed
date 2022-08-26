namespace SmartMed.API.Utils
{
    public class HttpResponseMessage
    {
        private string _message;

        public HttpResponseMessage(string message)
        {
            _message = message;
        }

        public string Message { get { return _message; } }
    }
}
