using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Immudb.Schema;

namespace ImmuClient.Utils
{
    public class ItemUtils
    {
        public static byte[] GetHash(Item item)
        {
            if (item == null)
            {
                return null;
            }

            return Digest.Calc(
                    item.Index,
                    item.Key.ToByteArray(),
                    item.Value.ToByteArray()
            );
        }
    }
}
