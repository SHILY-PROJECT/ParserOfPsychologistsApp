using System.Drawing;

namespace ParserOfPsychologists.Application.Models;

public class CaptchaModel
{
    public string Id { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public Image Img { get; set; } = null!;
    public string InputText { get; set; } = string.Empty;
}