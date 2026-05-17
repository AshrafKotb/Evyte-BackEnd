using System.Text.RegularExpressions;
using System.Web;

namespace Eventa.ApplicationCore.Services.Maps
{
    public class GoogleMapsService : IGoogleMapsService
    {
        public string ConvertToEmbedUrl(string locationUrl, string? fallbackAddress = null)
        {
            if (string.IsNullOrWhiteSpace(locationUrl))
            {
                if (!string.IsNullOrWhiteSpace(fallbackAddress))
                {
                    return BuildEmbedFromAddress(fallbackAddress);
                }
                return string.Empty;
            }

            // Try to extract coordinates from Google Maps URL
            var coordinates = ExtractCoordinates(locationUrl);
            if (coordinates != null)
            {
                return $"https://maps.google.com/maps?q={coordinates}&output=embed&z=15";
            }

            // Try to extract place query from URL
            var placeQuery = ExtractPlaceQuery(locationUrl);
            if (!string.IsNullOrEmpty(placeQuery))
            {
                return $"https://maps.google.com/maps?q={HttpUtility.UrlEncode(placeQuery)}&output=embed&z=15";
            }

            // Fallback: use the address text
            if (!string.IsNullOrWhiteSpace(fallbackAddress))
            {
                return BuildEmbedFromAddress(fallbackAddress);
            }

            // Last resort: encode the original URL as a query
            return $"https://maps.google.com/maps?q={HttpUtility.UrlEncode(locationUrl)}&output=embed&z=15";
        }

        private string? ExtractCoordinates(string url)
        {
            // Match patterns like @30.0444,31.2357 or /30.0444,31.2357
            var match = Regex.Match(url, @"@(-?\d+\.?\d*),(-?\d+\.?\d*)");
            if (match.Success)
            {
                return $"{match.Groups[1].Value},{match.Groups[2].Value}";
            }

            // Match patterns like ?q=30.0444,31.2357
            match = Regex.Match(url, @"[?&]q=(-?\d+\.?\d*),(-?\d+\.?\d*)");
            if (match.Success)
            {
                return $"{match.Groups[1].Value},{match.Groups[2].Value}";
            }

            // Match patterns like /place/.../@30.0444,31.2357
            match = Regex.Match(url, @"/place/[^/]+/@(-?\d+\.?\d*),(-?\d+\.?\d*)");
            if (match.Success)
            {
                return $"{match.Groups[1].Value},{match.Groups[2].Value}";
            }

            return null;
        }

        private string? ExtractPlaceQuery(string url)
        {
            // Match /place/Place+Name/ pattern
            var match = Regex.Match(url, @"/place/([^/@]+)");
            if (match.Success)
            {
                return HttpUtility.UrlDecode(match.Groups[1].Value.Replace("+", " "));
            }

            // Match ?q=place+name pattern
            match = Regex.Match(url, @"[?&]q=([^&]+)");
            if (match.Success)
            {
                var value = match.Groups[1].Value;
                // Skip if it's coordinates
                if (!Regex.IsMatch(value, @"^-?\d+\.?\d*,-?\d+\.?\d*$"))
                {
                    return HttpUtility.UrlDecode(value);
                }
            }

            return null;
        }

        private string BuildEmbedFromAddress(string address)
        {
            return $"https://maps.google.com/maps?q={HttpUtility.UrlEncode(address)}&output=embed&z=15";
        }
    }
}
