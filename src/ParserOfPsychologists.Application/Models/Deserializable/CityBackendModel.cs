namespace ParserOfPsychologists.Application.Models.Deserializable;

public class CityBackendModel
{
    private string _body = string.Empty;

    [JsonPropertyName("body")]
    public string Body { get => _body; set => _body = HttpUtility.UrlDecode(value, Encoding.UTF8); }

    [JsonPropertyName("enc")]
    public string Enc { get; set; } = string.Empty;
}