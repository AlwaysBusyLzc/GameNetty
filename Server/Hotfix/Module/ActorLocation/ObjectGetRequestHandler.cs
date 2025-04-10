using System;

namespace ET.Server
{
    [MessageHandler(SceneType.Location)]
    public class ObjectGetRequestHandler: MessageHandler<Scene, ObjectGetRequest, ObjectGetResponse>
    {
        protected override async ETTask Run(Scene scene, ObjectGetRequest request, ObjectGetResponse response)
        {
            ActorId actorId = await scene.GetComponent<LocationManagerComoponent>().Get(request.Type).Get(request.Key);
            response.ActorId = actorId.ToProto();
        }
    }
}