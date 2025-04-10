
namespace ET
{
    public partial class ProtoAddress
    {
        public Address ToAddress()
        {
            return new Address(this.Process, this.Fiber);
        }
        
    }

    public static class AddressHelper
    {
        public static ProtoAddress ToProto(this Address self)
        {
            return new ProtoAddress
            {
               Process = self.Process,
               Fiber = self.Fiber,
            };
            
        }
    }
    
    
    public partial class ProtoActorId
    {
        public ActorId ToActorId()
        {
            return new ActorId(this.Address.Process, this.Address.Fiber, this.InstanceId);
        }
    }
    
    public static class ActorIdHelper
    {
        public static ProtoActorId ToProto(this ActorId self)
        {
            return new ProtoActorId
            {
                Address = self.Address.ToProto(),
                InstanceId = self.InstanceId,
            };
            
        }
    }
    
    
}