using FnddsData.FnddsLoader.Entities;

namespace FnddsData.FnddsLoader.Models;

public class LoaderArguments
{
    public FnddsVersion FnddsVersion { get; set; } = default!;

    public string FnddsConnectionString { get; set; } = default!;

    public string FpedConnectionString { get; set; } = default!;

    public string FpidConnectionString { get; set; } = default!;
}
