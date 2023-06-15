namespace AirQualityApi.Middlewares
{
    public class ArgumentNullException : Exception
    {
        public ArgumentNullException(string message) : base(message)
        {
        }
    }
}
