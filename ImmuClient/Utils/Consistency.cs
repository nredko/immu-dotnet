using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Immudb.Schema;

namespace ImmuClient.Utils
{
    public static class Consistency
    {
        const byte NODE_PREFIX = 1;

        public static bool Verify(ConsistencyProof proof, Root prevRoot)
        {
            if (proof.First != prevRoot.Index)
                return false;

            var verified = VerifyPath(
                proof.Path.Select(p => p.ToByteArray()).ToList(),
                proof.Second,
                proof.First,
                proof.SecondRoot.ToByteArray(),
                prevRoot.Root_.ToByteArray()
            );

            if (verified)
            {
                proof.FirstRoot = prevRoot.Root_;
                return true;
            }

            return false;
        }
        public static bool VerifyPath(IEnumerable<byte[]> path, ulong second, ulong first, byte[] secondHash, byte[] firstHash)
        {
            var l = path.Count();
            if (first == second && firstHash.SequenceEqual(secondHash) && l == 0)
                return true;

            if (!(first < second) || l == 0)
                return false;

            var pp = new List<byte[]>();
            if (IsPowerOfTwo(first + 1))
            {
                pp.Add(firstHash);
            }
            pp.AddRange(path);

            var fn = first;
            var sn = second;

            while ((fn % 2) == 1)
            {
                fn >>= 1;
                sn >>= 1;
            }

            var fr = pp[0];
            var sr = pp[0];

            var isFirst = true;
            foreach (var c in pp)
            {
                if (isFirst)
                {
                    isFirst = false;
                    continue;
                }

                if (sn == 0)
                    return false;

                if (fn % 2 == 1 || fn == sn)
                {
                    var tmp = new List<byte>();
                    tmp.Add(NODE_PREFIX);
                    tmp.AddRange(c);
                    tmp.AddRange(fr);
                    fr = SHA256.Create().ComputeHash(tmp.ToArray());

                    tmp.Clear();
                    tmp.Add(NODE_PREFIX);
                    tmp.AddRange(c);
                    tmp.AddRange(sr);
                    sr = SHA256.Create().ComputeHash(tmp.ToArray());

                    while ((fn % 2) == 0 && fn != 0)
                    {
                        fn >>= 1;
                        sn >>= 1;
                    }
                }
                else
                {
                    var tmp = new List<byte>();
                    tmp.Add(NODE_PREFIX);
                    tmp.AddRange(sr);
                    tmp.AddRange(c);
                    sr = SHA256.Create().ComputeHash(tmp.ToArray());
                }

                fn >>= 1;
                sn >>= 1;
            }

            return fr.SequenceEqual(firstHash) && sr.SequenceEqual(secondHash) && sn == 0;
        }

        private static bool IsPowerOfTwo(ulong x)
        {
            return x != 0 && (x & (x - 1)) == 0;
        }
    }
}
