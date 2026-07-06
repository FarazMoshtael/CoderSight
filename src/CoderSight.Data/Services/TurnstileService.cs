using System.Net.Http.Json;
using CoderSight.Core.Entities;
using CoderSight.Core.Services;
using Microsoft.Extensions.Logging;

namespace CoderSight.Data.Services;

public class TurnstileService : ITurnstileService
{
    private readonly ISiteSettingsService _settingsService;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<TurnstileService> _logger;

    private SiteSettings? _cachedSettings;

    public TurnstileService(ISiteSettingsService settingsService, IHttpClientFactory httpClientFactory, ILogger<TurnstileService> logger)
    {
        _settingsService = settingsService;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public bool IsEnabled => !string.IsNullOrEmpty(SiteKey) && !string.IsNullOrEmpty(GetSettings().TurnstileSecretKey);

    public string? SiteKey => GetSettings().TurnstileSiteKey;

    public async Task<bool> VerifyAsync(string? token, string? remoteIp)
    {
        var settings = await _settingsService.GetAsync();
        var secretKey = settings.TurnstileSecretKey;

        if (string.IsNullOrEmpty(settings.TurnstileSiteKey) || string.IsNullOrEmpty(secretKey))
            return true;

        if (string.IsNullOrEmpty(token))
            return false;

        try
        {
            var client = _httpClientFactory.CreateClient();
            var payload = new Dictionary<string, string>
            {
                ["secret"] = secretKey,
                ["response"] = token
            };
            if (!string.IsNullOrEmpty(remoteIp))
                payload["remoteip"] = remoteIp;

            var response = await client.PostAsync(
                "https://challenges.cloudflare.com/turnstile/v0/siteverify",
                new FormUrlEncodedContent(payload));

            var result = await response.Content.ReadFromJsonAsync<TurnstileResponse>();
            if (result?.Success != true)
                _logger.LogWarning("Turnstile verification failed: {Errors}", string.Join(", ", result?.ErrorCodes ?? []));

            return result?.Success == true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Turnstile verification request failed");
            return false;
        }
    }

    private SiteSettings GetSettings()
    {
        if (_cachedSettings is null)
        {
            _cachedSettings = _settingsService.GetAsync().GetAwaiter().GetResult();
        }
        return _cachedSettings;
    }

    private class TurnstileResponse
    {
        public bool Success { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("error-codes")]
        public string[]? ErrorCodes { get; set; }
    }
}
