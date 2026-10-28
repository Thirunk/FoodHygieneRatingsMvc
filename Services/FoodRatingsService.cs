using FoodHygieneRatingsMvc.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Json;
namespace FoodHygieneRatingsMvc.Services;
public sealed class FoodRatingsService(HttpClient client,IMemoryCache cache):IFoodRatingsService{
public async Task<List<Authority>> GetAuthoritiesAsync(CancellationToken token)=>
(await cache.GetOrCreateAsync("authorities-v1",async entry=>{
entry.AbsoluteExpirationRelativeToNow=TimeSpan.FromHours(12);
var data=await client.GetFromJsonAsync<AuthorityResponse>("Authorities/basic",token);
return data?.Authorities.OrderBy(x=>x.Name).ToList()??[];
}))??[];
public async Task<List<Establishment>> GetEstablishmentsAsync(int authorityId,CancellationToken token){
if(authorityId<=0)throw new ArgumentOutOfRangeException(nameof(authorityId));
var result=new List<Establishment>();
for(int page=1;page<=200;page++){
var data=await client.GetFromJsonAsync<EstablishmentResponse>($"Establishments?localAuthorityId={authorityId}&pageNumber={page}&pageSize=500",token)??throw new HttpRequestException("Empty API response");
result.AddRange(data.Establishments);
if(data.Meta.TotalPages>0&&page>=data.Meta.TotalPages)break;
if(data.Establishments.Count==0)break;
if(data.Meta.TotalCount>0&&result.Count>=data.Meta.TotalCount)break;
if(page==200)throw new InvalidOperationException("Pagination limit reached");
}
return result;
}}