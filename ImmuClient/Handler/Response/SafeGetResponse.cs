using Google.Protobuf;
using Immudb.Schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImmuClient.Handler.Response
{
    public class SafeGetResponse
    {
        public ulong Index { get; set; }
        public ByteString Key { get; set; }

        public long Timestamp { get; set; }
        public ByteString Value { get; set; }
        public bool Verified { get; set; }

        public SafeGetResponse() { }
    }
}
