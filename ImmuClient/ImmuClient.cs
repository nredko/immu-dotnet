using Grpc.Net.Client;
using Immudb.Schema;
using System;

namespace ImmuClient
{
    public class ImmuClient
    {
        private readonly ImmuService.ImmuServiceClient instance;

        public ImmuClient(string serverUrl)
        {
            var channel = GrpcChannel.ForAddress(serverUrl);
            instance = new ImmuService.ImmuServiceClient(channel);
        }

        public ImmuService.ImmuServiceClient GetInstance()
        {
            return instance;
        }
    }
}
