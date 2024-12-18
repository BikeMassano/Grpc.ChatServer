using Grpc.Core;
using System.Collections.Concurrent;

namespace Grpc.ChatServer.Services
{
    public class MessageStreamingService
    {
        // Потокобезопасная коллекция для подключения клиентов
        // Их работа не требует доп. синхронизации в коде.
        private readonly ConcurrentBag<IServerStreamWriter<ChatMessage>> _streams;

        // Конструктор класса по умолчанию
        public MessageStreamingService()
        {
            _streams = new ConcurrentBag<IServerStreamWriter<ChatMessage>>();
        }
        // Подписка клиента на получение сообщений
        public void Subscribe(IServerStreamWriter<ChatMessage> stream)
        {
            _streams.Add(stream);
        }

        // Рассылка сообщения всем клиентам, подключенным к сервису
        public async Task SendMessage(ChatMessage message)
        {
            try
            {
            // Параллельная рассылка сообщения всем клиентам
            await Parallel.ForEachAsync(_streams, async (stream, ctx) =>
            {
                // Отправка сообщения в поток
                await stream.WriteAsync(message);
            });
            }
            catch
            {

            }
        }
    }
}
