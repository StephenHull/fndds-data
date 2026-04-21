using FoodAndNutrientData.Importer.Entities;

namespace FoodAndNutrientData.Importer.Models;

public class LoaderArguments
{
    public FnddsVersion FnddsVersion { get; set; } = default!;

    public string FnddsConnectionString { get; set; } = default!;

    public string FpedConnectionString { get; set; } = default!;

    public string FpidConnectionString { get; set; } = default!;
}
