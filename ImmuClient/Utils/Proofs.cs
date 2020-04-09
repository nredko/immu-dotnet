using Google.Protobuf;
using Google.Protobuf.Collections;
using Immudb.Schema;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ImmuClient.Utils
{
    public class Proofs
    {
        //private static const int SHA256_SIZE = 32;

        // FromSlice sets _Path_ from the give _slice_.
        private static IEnumerable<byte[]> FromSlice(RepeatedField<ByteString> slice)
        {
            return slice.Select(s => s.ToByteArray()).ToList();
        }

        public static bool Verify(Proof proof, byte[] leaf, Root prevRoot)
        {
            if (proof == null || !leaf.SequenceEqual(proof.Leaf.ToByteArray()))
            {
                return false;
            }

            var path = FromSlice(proof.InclusionPath);
            var rt = proof.Root.ToByteArray();
            var lf = proof.Leaf.ToByteArray();

            bool verifiedInclusion = Inclusion.VerifyPath(
                    path,
                    proof.At,
                    proof.Index,
                    rt,
                    lf
            );

            if (!verifiedInclusion)
            {
                return false;
            }

            // we cannot check consistency when the previous root is not provided
            if (prevRoot.Index == 0 && prevRoot.Root_.Length == 0)
            {
                return true;
            }

            path = FromSlice(proof.ConsistencyPath);

            var firstRoot = prevRoot.Root_.ToByteArray();
            var secondRoot = proof.Root.ToByteArray();

            return Consistency.VerifyPath(path, proof.At, prevRoot.Index, secondRoot, firstRoot);
        }
    }
}
