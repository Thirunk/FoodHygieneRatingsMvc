using FoodHygieneRatingsMvc.Models;
namespace FoodHygieneRatingsMvc.Services;
public static class RatingCalculator{
public static readonly string[] FhrsOrder=["5","4","3","2","1","0","Exempt"];
public static readonly string[] FhisOrder=["Pass and Eat Safe","Pass","Improvement Required"];
public static (string Scheme,IReadOnlyList<RatingResult> Results,int Excluded) Calculate(IReadOnlyList<Establishment> items){
if(items.Count==0)return("FHRS",Build(FhrsOrder,new Dictionary<string,int>()),0);
var schemes=items.Select(x=>(x.SchemeType??"").Trim()).Where(x=>x.Equals("FHIS",StringComparison.OrdinalIgnoreCase)||x.Equals("FHRS",StringComparison.OrdinalIgnoreCase)).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
if(schemes.Length>1)throw new InvalidOperationException("Mixed rating schemes");
var fhis=schemes.Length==1?schemes[0].Equals("FHIS",StringComparison.OrdinalIgnoreCase):items.Any(x=>FhisOrder.Any(r=>r.Equals((x.RatingValue??"").Trim(),StringComparison.OrdinalIgnoreCase)));
var order=fhis?FhisOrder:FhrsOrder;
var counts=new Dictionary<string,int>(StringComparer.OrdinalIgnoreCase);
foreach(var record in items){
var value=(record.RatingValue??"").Trim();
var key=order.FirstOrDefault(k=>k.Equals(value,StringComparison.OrdinalIgnoreCase)||(k=="Pass and Eat Safe"&&value.Equals("Pass and EatSafe",StringComparison.OrdinalIgnoreCase)));
if(key is not null)counts[key]=counts.GetValueOrDefault(key)+1;
}
return(fhis?"FHIS":"FHRS",Build(order,counts),items.Count-counts.Values.Sum());
}
private static IReadOnlyList<RatingResult> Build(string[] order,IReadOnlyDictionary<string,int> counts){
var total=counts.Values.Sum();
return order.Select(k=>new RatingResult(k,counts.GetValueOrDefault(k),total==0?0m:Math.Round(100m*counts.GetValueOrDefault(k)/total,2))).ToArray();
}}