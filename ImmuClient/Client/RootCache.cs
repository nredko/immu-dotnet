using Google.Protobuf;
using Immudb.Schema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ImmuClient.Client
{
    public class RootCache
    {
        private const string ROOT_FN = ".root";

        public Root Get()
        {
            if (!File.Exists(ROOT_FN))
                throw new Exception("Cache file not found.");

            using (var input = File.OpenRead(ROOT_FN))
            {
                return Root.Parser.ParseFrom(input);
            }
        }

        public void Set(Root root)
        {
            using (var output = File.Create(ROOT_FN))
            {
                root.WriteTo(output);
            }
        }
    }
}
