using FoodHygieneRatingsMvc.Models;
using FoodHygieneRatingsMvc.Services;
using Microsoft.AspNetCore.Mvc;
namespace FoodHygieneRatingsMvc.Controllers;
public sealed class RatingsController(IFoodRatingsService ratings,ILogger<RatingsController> logger):Controller{
[HttpGet]public async Task<IActionResult> Index(int? authorityId,CancellationToken token){
var model=new RatingsViewModel{AuthorityId=authorityId};
try{
model.Authorities=await ratings.GetAuthoritiesAsync(token);
if(!authorityId.HasValue)return View(model);
var authority=model.Authorities.FirstOrDefault(x=>x.Id==authorityId);
if(authority is null){model.Error="Please choose a valid authority.";return View(model);}
model.AuthorityName=authority.Name;
var establishments=await ratings.GetEstablishmentsAsync(authority.Id,token);
model.TotalFetched=establishments.Count;
var summary=RatingCalculator.Calculate(establishments);
model.Scheme=summary.Scheme;model.Results=summary.Results;model.ExcludedCount=summary.Excluded;
model.RatedCount=model.Results.Sum(x=>x.Count);
}catch(Exception ex)when(ex is HttpRequestException or TaskCanceledException or InvalidOperationException){
logger.LogError(ex,"Failed to load ratings for {AuthorityId}",authorityId);
model.Error="Unable to load food ratings. Please try again later.";
}
return View(model);
}
[HttpGet]public IActionResult Error()=>View();
}