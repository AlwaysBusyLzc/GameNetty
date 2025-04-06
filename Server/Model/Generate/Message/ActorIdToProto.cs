namespace ET
{
    public static class AddressHelper
    {
        public static ProtoAddress ToProto(this Address self)
        {
            return new ProtoAddress()
            {
                Process = self.Process,
                Fiber = self.Fiber
            };
        }

        public static Address ToAddress(this ProtoAddress self)
        {
            return new Address()
            {
                Process = self.Process,
                Fiber = self.Fiber
            };

        }
    }


    public static class ActorIdHelper
    {
        public static ProtoActorId ToProto(this ActorId self)
        {
            return new ProtoActorId()
            {
                Address = self.Address.ToProto(),
                InstanceId = self.InstanceId
            };
        }

        public static ActorId ToActorId(this ProtoActorId self)
        {
            return new ActorId()
            {
                Address = self.Address.ToAddress(),
                InstanceId = self.InstanceId
            };
        }
    }
}
