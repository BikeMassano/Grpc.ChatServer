using Grpc.Core;

namespace Grpc.ChatServer.Services
{
    // Наследуем ChatService от абстрактного класса Chat.ChatBase,
    // который был сгенерирован из содержания .proto файла автоматически
    public class ChatService : Chat.ChatBase
    {
        // Логгер для записи информации о событиях и ошибках
        private readonly ILogger<ChatService> _logger;
        // Сервис для управления потоковой передачей сообщений клиентам
        private readonly MessageStreamingService _streamingService;
        // Конструктор класса
        public ChatService(ILogger<ChatService> logger, MessageStreamingService streamingService)
        {
            _logger = logger;
            _streamingService = streamingService;
        }

        // Переопределение метода входа в чат из .proto файла
        // Методы и свойства из .proto файла генерируются в файл
        public override async Task EnterChat(EnterRequest request, IServerStreamWriter<ChatMessage> responseStream, ServerCallContext context)
        {
            try
            {
                // Логирование информации о подключении клиента
                _logger.LogInformation($"{request.Name} вошёл в чат.");
                // Подписка клиента на получение сообщений
                _streamingService.Subscribe(responseStream);
                // Оправка сообщения всем клиентам о подключении нового клиента
                await _streamingService.SendMessage(new ChatMessage { Message = $"Пользователь с именем {request.Name} присоединился к чату." });

                await WaitForMessagesAsync();
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Неудачная попытка входа: {request.Name}\n{e.Message}");
            }
        }

        // Переопределение метода отправки сообщения клиентам из .proto файла
        public override async Task<MessageResponse> SendMessage(ChatMessage request, ServerCallContext context)
        {
            try
            {
                // Логирование информации о рассылке сообщения
                _logger.LogInformation($"{request.Message} получено.");

                // Отправка сообщения всем клиентам
                await _streamingService.SendMessage(request);
                return new MessageResponse() { Ok = true };
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Неудачная отправка сообщения: {request.Message}\n{e.Message}");
                return new MessageResponse() { Ok = false };
            }
        }

        public async Task WaitForMessagesAsync()
        {
            while (true)
            {
                await Task.Delay(10000); // Асинхронная задержка без блокировки потока
            }
        }
    }
}
