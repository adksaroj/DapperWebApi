namespace DapperWebApi
{
    public class CustomException : Exception
    {
        public CustomException(string message) : base(message)
        {

        }

        // We can add some properties here for Customized Messages

    }
}
