using Unity.Mathematics;

namespace ET
{
    public partial class ProtoFloat3
    {
        public float3 ToFloat3()
        {
            return new float3 { x = this.X, y = this.Y, z = this.Z};
        }
    }

    public static class Float3Helper
    {
        public static ProtoFloat3 ToProto(this float3 self)
        {
            return new ProtoFloat3 { X = self.x, Y = self.y, Z = self.z };
        }
    }

    public partial class ProtoQuaternion
    {
        public quaternion ToQuaternion()
        {
            return new quaternion(this.X, this.Y, this.Z, this.W);
        }
    }

    public static class QuaternionHelper
    {
        public static ProtoQuaternion ToProto(this quaternion self)
        {
            return new ProtoQuaternion { X = self.value.x, Y = self.value.y, Z = self.value.z, W = self.value.w };
        }
    }
}