using FoodHygieneRatingsMvc.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddMemoryCache();

builder.Services
    .AddOptions<FoodRatingsApiOptions>()
    .Bind(builder.Configuration.GetSection(FoodRatingsApiOptions.SectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddHttpClient<IFoodRatingsService, FoodRatingsService>((provider, client) =>
{
    var api = provider.GetRequiredService<IOptions<FoodRatingsApiOptions>>().Value;

    client.BaseAddress = new Uri(api.BaseUrl);
    client.DefaultRequestHeaders.Add("x-api-version", api.ApiVersion);
    client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
    client.Timeout = TimeSpan.FromSeconds(api.TimeoutSeconds);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Ratings/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute("default", "{controller=Ratings}/{action=Index}/{id?}");

app.Run();
