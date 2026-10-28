using FoodHygieneRatingsMvc.Services;
var builder=WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient<IFoodRatingsService,FoodRatingsService>(c=>{c.BaseAddress=new Uri("https://api.ratings.food.gov.uk/");c.DefaultRequestHeaders.Add("x-api-version","2");c.DefaultRequestHeaders.Accept.ParseAdd("application/json");c.Timeout=TimeSpan.FromSeconds(60);});
var app=builder.Build();
if(!app.Environment.IsDevelopment()){app.UseExceptionHandler("/Ratings/Error");app.UseHsts();}
app.UseHttpsRedirection();app.UseStaticFiles();app.UseRouting();app.MapControllerRoute("default","{controller=Ratings}/{action=Index}/{id?}");app.Run();