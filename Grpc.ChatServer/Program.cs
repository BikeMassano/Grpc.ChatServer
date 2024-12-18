using Grpc.ChatServer.Services;

// ������������� ������� ��� ������������ ����������
var builder = WebApplication.CreateBuilder(args);

// ��������� ������� � ��������� IServiceCollection
builder.Services.AddGrpc(); // ����������� gRPC �������
builder.Services.AddSingleton<MessageStreamingService>(); // ����������� ������� ��� Singleton
// �������� ����� Build ��� ������������ �������� �������� ����������, ����� ������ ����� ������ �������� ������������ ������.
var app = builder.Build();
// ��������� ��������, ������� ���������(�������������� ��) � �������� �����
// ���� ���������� �� ��������� � ����������, �� ��������� ������� ���-�� �� ������.
if (app.Environment.IsDevelopment())
{
    // ����� ���������� WebApplication, ����������� ExceptionHandlerMiddleware � �������
    // ��� ������ ���������� ��� �������� ���������������� ������
    app.UseExceptionHandler("/error");
}
// ���������� ��� ���������� �������� �� ��������� "/"
app.UseWelcomePage("/");

app.MapGrpcService<ChatService>();
// �������� ����� ������, ������������� ��� ��������� ����������
app.MapGet("/error", () => "Sorry, an error occurred");
app.Run();
