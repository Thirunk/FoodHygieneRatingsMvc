using System.ComponentModel.DataAnnotations;

namespace FoodHygieneRatingsMvc.Services;

public sealed class FoodRatingsApiOptions
{
    public const string SectionName = "FoodRatingsApi";

    [Required, Url]
    public string BaseUrl { get; set; } = "https://api.ratings.food.gov.uk/";

    [Required]
    public string ApiVersion { get; set; } = "2";

    [Range(1, 600)]
    public int TimeoutSeconds { get; set; } = 60;

    [Range(1, 5000)]
    public int PageSize { get; set; } = 500;

    [Range(1, 1000)]
    public int MaxPages { get; set; } = 200;

    [Range(0, 168)]
    public int AuthoritiesCacheHours { get; set; } = 12;

    [Range(0, 1440)]
    public int EstablishmentsCacheMinutes { get; set; } = 30;
}
