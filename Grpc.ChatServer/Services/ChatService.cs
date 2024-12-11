using Grpc.Core;

namespace Grpc.ChatServer.Services
{
    public class ChatService : Chat.ChatBase
    {
        private readonly ILogger<ChatService> _logger;
        private readonly MessageStreamingService _streamingService;
        public ChatService(ILogger<ChatService> logger, MessageStreamingService streamingService)
        {
            _logger = logger;
            _streamingService = streamingService;
        }

        public override async Task EnterChat(EnterRequest request, IServerStreamWriter<ChatMessage> responseStream, ServerCallContext context)
        {
            _logger.LogInformation($"{request.Name} is entered the chat.");

            _streamingService.Subscribe(responseStream);

            await _streamingService.SendMessage(new ChatMessage { Message = $"User with name {request.Name} entered the chat." });

            WaitForMessages();
        }

        public override async Task<MessageResponse> SendMessage(ChatMessage request, ServerCallContext context)
        {
            _logger.LogInformation($"{request.Message} is received.");

            await _streamingService.SendMessage(request);
            return new MessageResponse() { Ok = true };
        }

        public void WaitForMessages()
        {
            while (true)
            {
                Thread.Sleep(10000);
            }
        }
    }
}
