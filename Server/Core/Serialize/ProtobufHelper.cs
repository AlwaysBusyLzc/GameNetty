using System;
using System.Buffers;
using System.ComponentModel;
using Google.Protobuf;

namespace ET
{
    public static class ProtobufHelper
    {
        public static byte[] Serialize(object message)
        {
            if (message is ISupportInitialize supportInitialize)
            {
                supportInitialize.BeginInit();
            }
            return (message as Google.Protobuf.IMessage).ToByteArray();
        }

        public static void Serialize(object message, MemoryBuffer stream)
        {
            if (message is ISupportInitialize supportInitialize)
            {
                supportInitialize.BeginInit();
            }

            (message as Google.Protobuf.IMessage).WriteTo(stream as IBufferWriter<byte>);
        }

        public static object Deserialize(Type type, byte[] bytes, int index, int count)
        {
            var msg = Activator.CreateInstance(type);
            (msg as Google.Protobuf.IMessage).MergeFrom(bytes.AsSpan(index, count));
            
            
            if (msg is ISupportInitialize supportInitialize)
            {
                supportInitialize.EndInit();
            }
            return msg;
        }

        public static object Deserialize(Type type, byte[] bytes, int index, int count, ref object o)
        {
            (o as Google.Protobuf.IMessage).MergeFrom(bytes.AsSpan(index, count));
            if (o is ISupportInitialize supportInitialize)
            {
                supportInitialize.EndInit();
            }
            return o;
        }

        public static object Deserialize(Type type, MemoryBuffer stream)
        {
            var msg = Activator.CreateInstance(type);
            (msg as Google.Protobuf.IMessage).MergeFrom(stream.GetSpan());
            
            if (msg is ISupportInitialize supportInitialize)
            {
                supportInitialize.EndInit();
            }
            return msg;
        }

        public static object Deserialize(Type type, MemoryBuffer stream, ref object o)
        {
            (o as Google.Protobuf.IMessage).MergeFrom(stream.GetSpan());
            
            if (o is ISupportInitialize supportInitialize)
            {
                supportInitialize.EndInit();
            }
            return o;
        }
    }
}