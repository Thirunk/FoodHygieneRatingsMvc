using FoodHygieneRatingsMvc.Models;
namespace FoodHygieneRatingsMvc.Services;
public interface IFoodRatingsService{
Task<List<Authority>> GetAuthoritiesAsync(CancellationToken token);
Task<List<Establishment>> GetEstablishmentsAsync(int authorityId,CancellationToken token);
}