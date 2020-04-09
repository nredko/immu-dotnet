using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ImmuClient.Utils
{
    public static class Digest
    {
        private const byte LEAF_PREFIX = 0;

        public static byte[] Calc(ulong index, byte[] key, byte[] value)
        {
            var c = new byte[1 + 8 + 8 + key.Length + value.Length];

            var buffIndex = BitConverter.GetBytes((UInt64)index);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(buffIndex);

            var buffKey = BitConverter.GetBytes((UInt64)key.Length);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(buffKey);

            c[0] = LEAF_PREFIX;
            buffIndex.CopyTo(c, 1);
            buffKey.CopyTo(c, 1 + 8);
            key.CopyTo(c, 1 + 8 + 8);
            value.CopyTo(c, 1 + 8 + 8 + key.Length);

            return SHA256.Create().ComputeHash(c);
        }
    }
}
