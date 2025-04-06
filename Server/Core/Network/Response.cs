namespace ET
{
    [Message(ushort.MaxValue)]
    public partial class MessageResponse: MessageObject, IResponse
    {
        public int Error { get; set; }
        public string Message { get; set; }
    }
}