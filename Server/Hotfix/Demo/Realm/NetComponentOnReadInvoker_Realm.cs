using System;

namespace ET.Server
{
    [Invoke((long)SceneType.Realm)]
    public class NetComponentOnReadInvoker_Realm: AInvokeHandler<NetComponentOnRead>
    {
        public override void Handle(NetComponentOnRead args)
        {
            Session session = args.Session;
            object message = args.Message;
            
            // 如果注册了处理，则由本服务处理，不再作为actor消息处理
            if (MessageSessionDispatcher.Instance.CanHandleMessage(message.GetType()))
            { 
                MessageSessionDispatcher.Instance.Handle(session, message);
            }
            else
            {
                throw new Exception($"not found handler: {message}");
            }
            
            // 根据消息接口判断是不是Actor消息，不同的接口做不同的处理,比如需要转发给Chat Scene，可以做一个IChatMessage接口
            // switch (message)
            // {
            //     case ISessionMessage:
            //     {
            //         MessageSessionDispatcher.Instance.Handle(session, message);
            //         break;
            //     }
            //     default:
            //     {
            //         throw new Exception($"not found handler: {message}");
            //     }
            // }
        }
    }
}