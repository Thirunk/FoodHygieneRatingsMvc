# Food Hygiene Ratings MVC
ASP.NET Core 8 MVC app to retrieve real UK Food Hygiene Rating API establishments by local authority, calculate percentages and display them in fixed scheme order.

## Run
Install .NET 8 SDK and execute:
```bash
dotnet restore
dotnet build
dotnet run
```

Select a local authority from the dropdown. Country selection is not required. The API is called with header `x-api-version: 2`.

## Rating order
- FHRS: 5, 4, 3, 2, 1, 0, Exempt
- FHIS: Pass and Eat Safe, Pass, Improvement Required

The percentage denominator is the sum of establishments belonging to the fixed rating categories; statuses such as Awaiting Inspection are excluded. Confirm this assumption with the product owner.

## Status
Initial source implementation; compilation, unit tests, and live API integration have not yet been verified in a .NET runtime.
