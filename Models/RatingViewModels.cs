namespace FoodHygieneRatingsMvc.Models;
public sealed record RatingResult(string Rating,int Count,decimal Percentage);
public sealed class RatingsViewModel{
public int? AuthorityId{get;set;}public string? AuthorityName{get;set;}
public List<Authority> Authorities{get;set;}=[];
public string? Scheme{get;set;}public int RatedCount{get;set;}
public int ExcludedCount{get;set;}public int TotalFetched{get;set;}
public IReadOnlyList<RatingResult> Results{get;set;}=[];
public string? Error{get;set;}
}