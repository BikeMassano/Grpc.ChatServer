using Grpc.Core;

namespace Grpc.ChatServer.Services
{
    public class MessageStreamingService
    {
        private readonly ICollection<IServerStreamWriter<ChatMessage>> _streams;

        public MessageStreamingService()
        {
            _streams = new List<IServerStreamWriter<ChatMessage>>();
        }

        public void Subscribe(IServerStreamWriter<ChatMessage> stream)
        {
            _streams.Add(stream);
        }

        public async Task SendMessage(ChatMessage message)
        {
            await Parallel.ForEachAsync(_streams, async (stream, ctx) =>
            {
                await stream.WriteAsync(message);

            });
        }
    }
}
