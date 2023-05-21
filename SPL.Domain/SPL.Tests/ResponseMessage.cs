namespace SPL.Domain.SPL.Tests
{
    public class ResponseMessage
    {
        #region Properties

        public MessageType Type { get; set; }

        public string Message { get; set; }

        #endregion

        #region Constructor

        public ResponseMessage(MessageType type, string message)
        {
            Type = type;
            Message = message;
        }

        #endregion
    }

    public enum MessageType
    {
        Information = 0,
        Warning = 1,
        Error = 2,
    }
}