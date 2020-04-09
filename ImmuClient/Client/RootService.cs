using Immudb.Schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImmuClient.Client
{
    public class RootService
    {
        private readonly ImmuService.ImmuServiceClient immuC;
        private readonly RootCache cache;

        public RootService(ImmuService.ImmuServiceClient immuC)
        {
            this.immuC = immuC;
            this.cache = new RootCache();
        }

        public void Init()
        {
            Root root = immuC.CurrentRoot(new Google.Protobuf.WellKnownTypes.Empty());
            cache.Set(root);
        }

        public Root GetRoot()
        {
            try
            {
                return cache.Get();
            }
            catch (Exception)
            {
                Root root = immuC.CurrentRoot(new Google.Protobuf.WellKnownTypes.Empty());
                cache.Set(root);

                return root;
            }
        }

        public void SetRoot(Root root)
        {
            cache.Set(root);
        }
    }
}
