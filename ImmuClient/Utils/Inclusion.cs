using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Google.Protobuf;
using Immudb.Schema;

namespace ImmuClient.Utils
{
    public static class Inclusion
    {
        const byte NODE_PREFIX = 1;

        public static bool Verify(InclusionProof inclusionProof, ulong index, ByteString leaf)
        {
            if (inclusionProof.Index != index || inclusionProof.Leaf != leaf)
                return false;

            return VerifyPath(
                inclusionProof.Path.Select(p => p.ToByteArray()).ToList(),
                inclusionProof.At,
                inclusionProof.Index,
                inclusionProof.Root.ToByteArray(),
                leaf.ToByteArray()
            );
        }

        public static bool VerifyPath(List<byte[]> path, ulong at, ulong i, byte[] root, byte[] leaf)
        {
            if (i > at || (at > 0 && path.Count == 0))
                return false;

            var h = leaf;
            foreach (var v in path)
            {
                var c = new List<byte>();
                c.Add(NODE_PREFIX);

                if (i % 2 == 0 && i != at)
                {
                    c.AddRange(h);
                    c.AddRange(v);
                }
                else
                {
                    c.AddRange(v);
                    c.AddRange(h);
                }
                h = SHA256.Create().ComputeHash(c.ToArray());
                i /= 2;
                at /= 2;
            }

            return at == i && h.SequenceEqual(root);
        }
    }
}
