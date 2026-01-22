using SupportCenter.Classes;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;

public class ChatService
{
    private readonly HttpClient _httpClient;

    public ChatService()
    {
        // Берём адрес API из App.xaml
        var baseAddress = Application.Current.Resources["ApiBaseAddress"]?.ToString()
                          ?? throw new Exception("ApiBaseAddress не найден в App.xaml");

        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(baseAddress)
        };
    }

    // Получение сообщений
    public async Task<List<ChatMessageDto>> GetMessagesAsync(int typeAppeal, int appealId)
    {
        var messages = await _httpClient.GetFromJsonAsync<List<ChatMessageDto>>(
            $"api/chat/{typeAppeal}/{appealId}");
        return messages ?? new List<ChatMessageDto>();
    }
    public async Task SendMessageAsync(CreateChatMessageDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/chat", dto);
        response.EnsureSuccessStatusCode();
    }
}