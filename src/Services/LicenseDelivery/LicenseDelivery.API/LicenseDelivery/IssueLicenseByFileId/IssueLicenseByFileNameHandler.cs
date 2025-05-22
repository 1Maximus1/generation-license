namespace LicenseDelivery.API.LicenseDelivery.GetLicense
{
    public record IssueLicenseByFileNameCommand(string FileName) : ICommand<IssueLicenseByFileNameResult>;
    public record IssueLicenseByFileNameResult(LicenseKey Key);
    public class IssueLicenseByFileNameCommandValidator : AbstractValidator<IssueLicenseByFileNameCommand>
    {
        public IssueLicenseByFileNameCommandValidator()
        {
            RuleFor(x => x.FileName).NotEmpty().WithMessage("FileId is required");
        }
    }

    public class IssueLicenseByFileNameCommandHandler(GoogleDriveClient googleDriveClient) : ICommandHandler<IssueLicenseByFileNameCommand, IssueLicenseByFileNameResult>
    {
        public async Task<IssueLicenseByFileNameResult> Handle(IssueLicenseByFileNameCommand request, CancellationToken cancellationToken)
        {
            var license = await googleDriveClient.GetFileByName(request.FileName);

            return new IssueLicenseByFileNameResult(new LicenseKey()
            {
                FileContent = new byte[10],
                FileName = license
            });
        }
    }
}
