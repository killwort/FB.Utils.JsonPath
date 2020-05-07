namespace FB.Utils.JsonPath.Language
{
    public class Error
    {
        public Error(int line, int column, string message, ErrorType type)
        {
            Line = line;
            Column = column;
            Message = message;
            Type = type;
        }

        public int Line { get; }
        public int Column { get; }
        public string Message { get; }
        public ErrorType Type { get; }
    }
}