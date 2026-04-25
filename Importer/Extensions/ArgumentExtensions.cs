using System.Text.RegularExpressions;
using FoodAndNutrientData.Importer.Entities;
using FoodAndNutrientData.Importer.Models;

namespace FoodAndNutrientData.Importer.Extensions;

public static partial class ArgumentExtensions
{
    [GeneratedRegex("Microsoft\\.ACE\\.OLEDB\\.16\\.0;Data Source=.+\\.mdb;Persist Security Info=False;")]
    private static partial Regex ConnectionStringRegex();

    public static ImporterArguments GetArguments(this string[] args)
    {
        if (args.Length < 1)
        {
            Console.WriteLine("Error! No command-line arguments were specified.");
            Console.WriteLine("Required arguments:");
            Console.WriteLine("    FnddsVersion: <int> - The version of FNDDS to import.");
            Console.WriteLine("           1 = FNDDS 1.0 (2001-2002)");
            Console.WriteLine("           2 = FNDDS 2.0 (2003-2004)");
            Console.WriteLine("           4 = FNDDS 3.0 (2005-2006)");
            Console.WriteLine("           8 = FNDDS 4.1 (2007-2008)");
            Console.WriteLine("          16 = FNDDS 5.0 (2009-2010)");
            Console.WriteLine("          32 = FNDDS 2011-2012");
            Console.WriteLine("          64 = FNDDS 2013-2014");
            Console.WriteLine("         128 = FNDDS 2015-2016");
            Console.WriteLine("         256 = FNDDS 2017-2018");
            Console.WriteLine("         512 = FNDDS 2019-2020");
            Console.WriteLine("        1024 = FNDDS 2021-2023");
            Console.WriteLine("    ConnectionString: <string> - The Access database connection string.");
            Console.WriteLine("        Example: Provider=Microsoft.ACE.OLEDB.16.0;Data Source=c:/FNDDS.mdb;Persist Security Info=False;");
            Console.WriteLine("Usage: FnddsData.Importer <FnddsVersion> <ConnectionString>");

            throw new ArgumentException("No command-line arguments were specified.");
        }

        if (!FnddsVersion.TryParse(args[0], out var version) || version == null)
        {
            Console.WriteLine("Error! The FNDDS Version was invalid.");

            throw new ArgumentException("The FNDDS Version was invalid.");
        }

        if (args.Length < 2)
        {
            Console.WriteLine("Error! The FNDDS Connection String is required.");

            throw new ArgumentException("The FNDDS Connection String is required.");
        }

        if (!ConnectionStringRegex().IsMatch(args[1]))
        {
            Console.WriteLine("Error! The FNDDS Connection String was invalid.");

            throw new ArgumentException("The FNDDS Connection String was invalid.");
        }

        var importerArguments = new ImporterArguments
        {
            FnddsVersion = version,
            FnddsConnectionString = args[1]
        };

        if (args.Length > 2)
        {
            if (args.Length < 4)
            {
                Console.WriteLine("Error! The FPED and FPID Connection Strings are required.");

                throw new ArgumentException("The FPED and FPID Connection Strings are required.");
            }

            if (!ConnectionStringRegex().IsMatch(args[2]))
            {
                Console.WriteLine("Error! The FPED Connection String was invalid.");

                throw new ArgumentException("The FPED Connection String was invalid.");
            }

            importerArguments.FpedConnectionString = args[2];

            if (!ConnectionStringRegex().IsMatch(args[3]))
            {
                Console.WriteLine("Error! The FPID Connection String was invalid.");

                throw new ArgumentException("The FPID Connection String was invalid.");
            }

            importerArguments.FpidConnectionString = args[3];
        }

        return importerArguments;
    }
}
