using Grpc.Net.Client;
using ImmuClient.Client;
using ImmuClient.Handler.Response;
using Immudb.Schema;
using System;

namespace ImmuClient
{
    public class ImmuClient
    {
        private readonly ImmuService.ImmuServiceClient instance;
        private readonly GrpcChannel channel;
        private readonly RootService rootService;

        public ImmuClient(string immudbUrl)
        {
            this.channel = GrpcChannel.ForAddress(immudbUrl);
            this.instance = new ImmuService.ImmuServiceClient(channel);
            this.rootService = new RootService(this.instance);
            
            this.rootService.Init();
        }

        public ImmuService.ImmuServiceClient GetInstance()
        {
            return instance;
        }

        public void Shutdown()
        {
            this.channel.ShutdownAsync().Wait();
        }

        public SafeGetResponse SafeGet(SafeGetOptions request)
        {
            return Handler.SafeGet.Call(this.instance, this.rootService, request);
        }

        public SafeSetResponse SafeSet(SafeSetOptions request)
        {
            return Handler.SafeSet.Call(this.instance, this.rootService, request);
        }
    }
}
