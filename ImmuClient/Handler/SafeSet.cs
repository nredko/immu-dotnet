using Google.Protobuf;
using ImmuClient.Client;
using ImmuClient.Handler.Response;
using ImmuClient.Utils;
using Immudb.Schema;
using System;
using System.Linq;

namespace ImmuClient.Handler
{
    public class SafeSet
    {
        public static SafeSetResponse Call(ImmuService.ImmuServiceClient immuS, RootService rs, SafeSetOptions request)
        {
            var root = rs.GetRoot();

            var index = new Immudb.Schema.Index
            {
                Index_ = root.Index
            };

            var valueB = new byte[8 + request.Kv.Value.Length];
            var buffTimestamp = BitConverter.GetBytes(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
            if (BitConverter.IsLittleEndian)
                Array.Reverse(buffTimestamp);

            buffTimestamp.CopyTo(valueB, 0);
            request.Kv.Value.CopyTo(valueB, 8);

            var sso = new SafeSetOptions
            {
                Kv = new KeyValue
                {
                    Key = request.Kv.Key,
                    Value = ByteString.CopyFrom(valueB)
                },
                RootIndex = index
            };

            var msg = immuS.SafeSet(sso);

            var item = new Item
            {
                Key = sso.Kv.Key,
                Value = sso.Kv.Value,
                Index = msg.Index
            };

            if (!ItemUtils.GetHash(item).SequenceEqual(msg.Leaf.ToByteArray()))
                throw new Exception("Proof does not match the given item.");

            bool verified = Proofs.Verify(msg, msg.Leaf.ToByteArray(), root);

            if (verified)
            {
                var toCache = new Root
                {
                    Index = msg.Index,
                    Root_ = msg.Root
                };

                try
                {
                    rs.SetRoot(toCache);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            return new SafeSetResponse
            {
                Index = msg.Index,
                Leaf = msg.Leaf,
                Root = msg.Root,
                At = msg.At,
                InclusionPath = msg.InclusionPath,
                ConsistencyPath = msg.ConsistencyPath,
                Verified = verified
            };
        }
    }
}
