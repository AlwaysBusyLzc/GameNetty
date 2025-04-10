using System.Collections.Generic;
using Google.Protobuf;
using MongoDB.Bson;

namespace ET.Server
{
    public static partial class TransferHelper
    {
        public static async ETTask TransferAtFrameFinish(Unit unit, ActorId sceneInstanceId, string sceneName)
        {
            await unit.Fiber().WaitFrameFinish();

            await TransferHelper.Transfer(unit, sceneInstanceId, sceneName);
        }
        

        public static async ETTask Transfer(Unit unit, ActorId sceneInstanceId, string sceneName)
        {
            Scene root = unit.Root();
            
            // location加锁
            long unitId = unit.Id;
            
            M2M_UnitTransferRequest request = new();
            request.OldActorId = unit.GetActorId().ToProto();
            request.Unit = ByteString.CopyFrom(unit.ToBson());
            foreach (Entity entity in unit.Components.Values)
            {
                if (entity is ITransfer)
                {
                    request.Entitys.Add(ByteString.CopyFrom(entity.ToBson()));
                }
            }
            unit.Dispose();
            
            await root.GetComponent<LocationProxyComponent>().Lock(LocationType.Unit, unitId, request.OldActorId.ToActorId());
            await root.GetComponent<MessageSender>().Call(sceneInstanceId, request);
        }
    }
}