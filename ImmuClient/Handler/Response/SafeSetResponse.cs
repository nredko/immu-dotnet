using Google.Protobuf;
using Google.Protobuf.Collections;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImmuClient.Handler.Response
{
    public class SafeSetResponse
    {
        public ulong Index { get; set; }
        public ByteString Root { get; set; }
        public ByteString Leaf { get; set; }
        public ulong At { get; set; }
        public RepeatedField<ByteString> InclusionPath { get; set; }
        public RepeatedField<ByteString> ConsistencyPath { get; set; }
        public bool Verified { get; set; }

        public SafeSetResponse() { }
    }
}
