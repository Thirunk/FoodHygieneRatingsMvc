using System.Text.Json.Serialization;
namespace FoodHygieneRatingsMvc.Models;
public sealed class AuthorityResponse{[JsonPropertyName("authorities")] public List<Authority> Authorities{get;set;}=[];}
public sealed class Authority{[JsonPropertyName("LocalAuthorityId")] public int Id{get;set;} [JsonPropertyName("Name")] public string Name{get;set;}="";}
public sealed class EstablishmentResponse{[JsonPropertyName("establishments")] public List<Establishment> Establishments{get;set;}=[];[JsonPropertyName("meta")] public PageMeta Meta{get;set;}=new();}
public sealed class Establishment{[JsonPropertyName("RatingValue")] public string RatingValue{get;set;}="";[JsonPropertyName("SchemeType")] public string SchemeType{get;set;}="";}
public sealed class PageMeta{[JsonPropertyName("totalPages")] public int TotalPages{get;set;}[JsonPropertyName("totalCount")] public int TotalCount{get;set;}}
