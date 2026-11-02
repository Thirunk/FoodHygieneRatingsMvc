using System.Net.Http.Json;
using FoodHygieneRatingsMvc.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace FoodHygieneRatingsMvc.Services;

public sealed class FoodRatingsService(
    HttpClient client,
    IMemoryCache cache,
    IOptions<FoodRatingsApiOptions> options) : IFoodRatingsService
{
    private readonly FoodRatingsApiOptions _options = options.Value;

    public async Task<List<Authority>> GetAuthoritiesAsync(CancellationToken token)
    {
        return await GetOrFetchAsync("authorities", TimeSpan.FromHours(_options.AuthoritiesCacheHours), async () =>
        {
            var data = await client.GetFromJsonAsync<AuthorityResponse>("Authorities/basic", token);
            return data?.Authorities?.OrderBy(x => x.Name).ToList() ?? [];
        });
    }

    public async Task<List<Establishment>> GetEstablishmentsAsync(int authorityId, CancellationToken token)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(authorityId);

        return await GetOrFetchAsync(
            $"establishments-{authorityId}",
            TimeSpan.FromMinutes(_options.EstablishmentsCacheMinutes),
            () => FetchAllEstablishmentsAsync(authorityId, token));
    }

    // A zero duration disables caching; MemoryCache rejects a zero expiration.
    private async Task<List<T>> GetOrFetchAsync<T>(string key, TimeSpan duration, Func<Task<List<T>>> fetch)
    {
        if (duration <= TimeSpan.Zero)
            return await fetch();

        var items = await cache.GetOrCreateAsync(key, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = duration;
            return fetch();
        });

        return items ?? [];
    }

    private async Task<List<Establishment>> FetchAllEstablishmentsAsync(int authorityId, CancellationToken token)
    {
        var result = new List<Establishment>();

        for (var page = 1; page <= _options.MaxPages; page++)
        {
            var url = $"Establishments?localAuthorityId={authorityId}&pageNumber={page}&pageSize={_options.PageSize}";
            var data = await client.GetFromJsonAsync<EstablishmentResponse>(url, token)
                       ?? throw new HttpRequestException("Empty API response");

            var establishments = data.Establishments ?? [];
            var meta = data.Meta ?? new PageMeta();

            result.AddRange(establishments);

            var lastPage = meta.TotalPages > 0 && page >= meta.TotalPages;
            var allFetched = meta.TotalCount > 0 && result.Count >= meta.TotalCount;
            if (lastPage || allFetched || establishments.Count == 0)
                return result;
        }

        throw new InvalidOperationException("Pagination limit reached");
    }
}
