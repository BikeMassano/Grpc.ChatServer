using Grpc.Core;

namespace Grpc.ChatServer.Services
{
    public class MessageStreamingService
    {
        // Коллекция потоков для подключения клиентов
        private readonly ICollection<IServerStreamWriter<ChatMessage>> _streams;

        // Конструктор класса по умолчанию
        public MessageStreamingService()
        {
            _streams = new List<IServerStreamWriter<ChatMessage>>();
        }
        // Подписка клиента на получение сообщений
        public void Subscribe(IServerStreamWriter<ChatMessage> stream)
        {
            _streams.Add(stream);
        }

        // Рассылка сообщения всем клиентам, подключенным к сервису
        public async Task SendMessage(ChatMessage message)
        {
            // Параллельная рассылка сообщения всем клиентам
            await Parallel.ForEachAsync(_streams, async (stream, ctx) =>
            {
                // Отправка сообщения в поток
                await stream.WriteAsync(message);
            });
        }
    }
}
