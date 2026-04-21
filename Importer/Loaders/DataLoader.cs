using System.Data.OleDb;
using FoodAndNutrientData.Base.Interfaces;
using FoodAndNutrientData.Base.Services;
using FoodAndNutrientData.Importer.Contexts;
using FoodAndNutrientData.Importer.Entities;

namespace FoodAndNutrientData.Importer.Loaders;

public abstract class DataLoader : DataLoaderBase
{
    /// <summary>
    /// Constructs a new DataLoader object.
    /// </summary>
    /// <param name="version">The FNDDS version.</param>
    /// <param name="connection">The connection to the source database.</param>
    /// <param name="context">The destination database context.</param>
    protected DataLoader(FnddsVersion version, OleDbConnection connection, FnddsDbContext context)
    {
        FnddsVersion = version;
        Connection = connection;
        Context = context;
    }

    /// <summary>
    /// The source database connection.
    /// </summary>
    public sealed override OleDbConnection Connection { get; set; }

    /// <summary>
    /// The destination database context.
    /// </summary>
    public FnddsDbContext Context { get; }

    /// <summary>
    /// The FNDDS version.
    /// </summary>
    public sealed override IFnddsVersion FnddsVersion { get; set; }
}
