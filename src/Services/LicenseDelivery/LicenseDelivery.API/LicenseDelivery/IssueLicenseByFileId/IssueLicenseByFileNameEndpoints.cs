namespace LicenseDelivery.API.LicenseDelivery.GetLicense
{
    public record IssueLicenseByFileNameRequest(string FileName);
    public record IssueLicenseByFileNameResponse(string Key);

    public class IssueLicenseByFileNameEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/licenses/issue", async (IssueLicenseByFileNameRequest request, ISender sender) =>
            {
                var command = request.Adapt<IssueLicenseByFileNameCommand>();
                
                var result = await sender.Send(command);

                var response = result.Adapt<IssueLicenseByFileNameResponse>();

                return Results.Ok(result);
            }).WithName("IssueLicenseByFileName")
              .Produces<IssueLicenseByFileNameResponse>(StatusCodes.Status202Accepted)
              .ProducesProblem(StatusCodes.Status400BadRequest)
              .WithSummary("Issue License By File Name")
              .WithDescription("Issue license key");
        }
    }
}
