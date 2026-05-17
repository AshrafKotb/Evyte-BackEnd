namespace Eventa.ApplicationCore.Services.Maps
{
    public interface IGoogleMapsService
    {
        string ConvertToEmbedUrl(string locationUrl, string? fallbackAddress = null);
    }
}
