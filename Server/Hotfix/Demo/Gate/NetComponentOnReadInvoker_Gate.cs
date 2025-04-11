using System;

namespace ET.Server
{
    [Invoke((long)SceneType.Gate)]
    public class NetComponentOnReadInvoker_Gate: AInvokeHandler<NetComponentOnRead>
    {
        public override void Handle(NetComponentOnRead args)
        {
            HandleAsync(args).Coroutine();
        }

        private async ETTask HandleAsync(NetComponentOnRead args)
        {
            Session session = args.Session;
            object message = args.Message;
            Scene root = args.Session.Root();

            // 如果注册了处理，则由本服务处理，不再作为actor消息处理
            if (MessageSessionDispatcher.Instance.CanHandleMessage(message.GetType()))
            { 
                MessageSessionDispatcher.Instance.Handle(session, message);
                return;
            }
            
            // 根据消息接口判断是不是Actor消息，不同的接口做不同的处理,比如需要转发给Chat Scene，可以做一个IChatMessage接口
            switch (message)
            {
                case IRequest actorLocationRequest: // gate session收到actor rpc消息，先向actor 发送rpc请求，再将请求结果返回客户端
                {
                    // Type reqType = actorLocationRequest.GetType();
                    // Type resType = OpcodeType.Instance.GetResponseType(reqType);
                    
                    long unitId = session.GetComponent<SessionPlayerComponent>().Player.Id;
                    int rpcId = actorLocationRequest.RpcId; // 这里要保存客户端的rpcId
                    long instanceId = session.InstanceId;
                    IResponse iResponse = await root.GetComponent<MessageLocationSenderComponent>().Get(LocationType.Unit).Call(unitId, actorLocationRequest);
                    iResponse.RpcId = rpcId;
                    // session可能已经断开了，所以这里需要判断
                    if (session.InstanceId == instanceId)
                    {
                        session.Send(iResponse);
                    }
                    break;
                }
                case IMessage actorLocationMessage:
                {
                    throw new Exception($"client send to server must req, {message.GetType().FullName}");
                    // long unitId = session.GetComponent<SessionPlayerComponent>().Player.Id;
                    // root.GetComponent<MessageLocationSenderComponent>().Get(LocationType.Unit).Send(unitId, actorLocationMessage);
                    // break;
                }
            
                default:
                {
                    throw new Exception($"not found handler: {message}");
                }
            }
        }
    }
}