namespace ET
{
    // 不需要返回消息
    public interface IMessage
    {
        int RpcId
        {
            get;
            set;
        }
    }

    public interface IRequest: IMessage
    {

    }

    public interface IResponse: IMessage
    {
        int Error
        {
            get;
            set;
        }

        string Message
        {
            get;
            set;
        }
    }
}