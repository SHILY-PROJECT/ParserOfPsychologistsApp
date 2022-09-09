using System.Text;
using System.Text.Json.Serialization;
using System.Web;

namespace ParserOfPsychologists.Application.Models.ResponseBackend;

public class ContactsBackendModel
{
    private string _htmlWithKontaktOne = string.Empty;

    [JsonPropertyName("ok")]
    public string Ok { get; set; } = string.Empty;

    [JsonPropertyName("kontakt")]
    public string HtmlWithKontaktOne { get => _htmlWithKontaktOne; set => _htmlWithKontaktOne = HttpUtility.UrlDecode(value, Encoding.UTF8); }

    [JsonPropertyName("kontakt2")]
    public string HtmlWithKontaktTwo { get; set; } = string.Empty;
}