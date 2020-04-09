using Google.Protobuf;
using ImmuClient.Client;
using ImmuClient.Handler.Response;
using ImmuClient.Utils;
using Immudb.Schema;
using System;
using System.Linq;

namespace ImmuClient.Handler
{
    public class SafeGet
    {
        public static SafeGetResponse Call(ImmuService.ImmuServiceClient immuS, RootService rs, SafeGetOptions request)
        {
            var root = rs.GetRoot();
            var index = new Immudb.Schema.Index
            {
                Index_ = root.Index
            };

            var protoReq = new SafeGetOptions(request)
            {
                RootIndex = index
            };

            var msg = immuS.SafeGet(protoReq);
            var verified = Proofs.Verify(msg.Proof, ItemUtils.GetHash(msg.Item), root);

            if (verified)
            {
                var toCache = new Root
                {
                    Index = msg.Proof.At,
                    Root_ = msg.Proof.Root
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

            var i = msg.Item;

            var timestampBytes = i.Value.Take(8).ToArray();
            if (BitConverter.IsLittleEndian)
                Array.Reverse(timestampBytes);

            return new SafeGetResponse
            {
                Index = i.Index,
                Key = i.Key,
                Value = ByteString.CopyFrom(i.Value.Skip(8).ToArray()),
                Timestamp = BitConverter.ToInt64(timestampBytes),
                Verified = verified
            };
        }
    }
}
