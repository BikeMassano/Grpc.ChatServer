using Grpc.ChatServer.Services;

// Инициализация билдера для конфигурации приложения
var builder = WebApplication.CreateBuilder(args);

// Добавляем сервисы в контейнер IServiceCollection
builder.Services.AddGrpc(); // регистрация gRPC сервиса
builder.Services.AddSingleton<MessageStreamingService>(); // регистрация сервиса как Singleton
// Вызываем метод Build для конфигурации основных сервисов приложений, после вызова этого метода изменить конфигурацию нельзя.
var app = builder.Build();
// Настройка конвеера, порядок элементов(Промежуточного ПО) в конвеере важен
// Если приложение не находится в разработке, то выводится меньшее кол-во об ошибке.
if (app.Environment.IsDevelopment())
{
    // Метод расширения WebApplication, добавляющий ExceptionHandlerMiddleware в конвеер
    // для вывода информации без передачи конфиденциальных данных
    app.UseExceptionHandler("/error");
}
// Расширение для прикольной страницы на эндпоинте "/"
app.UseWelcomePage("/");

app.MapGrpcService<ChatService>();
// Конечная точка ошибки, выполняющаяся при обработке исключения
app.MapGet("/error", () => "Sorry, an error occurred");
app.Run();
