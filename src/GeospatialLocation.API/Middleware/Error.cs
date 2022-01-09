namespace GeospatialLocation.API.Extensions
{
    public class Error
    {
        public Error(string code, string message, params object[] values)
        {
            Code = code;
            Message = message;
            Values = values;
        }

        public string Code { get; }

        public string Message { get; }

        public object[] Values { get; set; }
    }
}