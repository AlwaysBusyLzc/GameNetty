using System;

namespace ET.Server
{
    [MessageHandler(SceneType.Location)]
    public class ObjectUnLockRequestHandler: MessageHandler<Scene, ObjectUnLockRequest, ObjectUnLockResponse>
    {
        protected override async ETTask Run(Scene scene, ObjectUnLockRequest request, ObjectUnLockResponse response)
        {
            var oldActorId = new ActorId(request.OldActorId.Address.Process, request.OldActorId.Address.Fiber, request.OldActorId.InstanceId);
            var newActorId = new ActorId(request.NewActorId.Address.Process, request.NewActorId.Address.Fiber, request.NewActorId.InstanceId);
            scene.GetComponent<LocationManagerComoponent>().Get(request.Type).UnLock(request.Key, oldActorId, newActorId);

            await ETTask.CompletedTask;
        }
    }
}