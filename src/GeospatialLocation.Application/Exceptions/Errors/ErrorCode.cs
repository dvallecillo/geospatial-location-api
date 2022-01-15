namespace GeospatialLocation.Application.Exceptions.Errors
{
    public class ErrorCode
    {
        public ErrorCode(string code, string message)
        {
            Code = code;
            Message = message;
        }

        public string Code { get; set; }
        public string Message { get; set; }
    }
}