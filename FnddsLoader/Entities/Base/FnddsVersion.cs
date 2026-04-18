using FnddsData.Fndds.Interfaces;

namespace FnddsData.FnddsLoader.Entities;

public partial class FnddsVersion : IFnddsVersion
{
    public static bool TryParse(string value, out FnddsVersion? version)
    {
        if (!int.TryParse(value, out var id))
        {
            version = null;

            return false;
        }

        version = id switch
        {
            1 => new FnddsVersion
            {
                Id = id,
                BeginYear = 2001,
                EndYear = 2002,
                Major = 1,
                Minor = 0
            },
            2 => new FnddsVersion
            {
                Id = id,
                BeginYear = 2003,
                EndYear = 2004,
                Major = 2,
                Minor = 0
            },
            4 => new FnddsVersion
            {
                Id = id,
                BeginYear = 2005,
                EndYear = 2006,
                Major = 3,
                Minor = 0
            },
            8 => new FnddsVersion
            {
                Id = id,
                BeginYear = 2007,
                EndYear = 2008,
                Major = 4,
                Minor = 1
            },
            16 => new FnddsVersion
            {
                Id = id,
                BeginYear = 2009,
                EndYear = 2010,
                Major = 5,
                Minor = 0
            },
            32 => new FnddsVersion
            {
                Id = id,
                BeginYear = 2011,
                EndYear = 2012,
                Major = 6,
                Minor = 0
            },
            64 => new FnddsVersion
            {
                Id = id,
                BeginYear = 2013,
                EndYear = 2014,
                Major = 7,
                Minor = 0
            },
            128 => new FnddsVersion
            {
                Id = id,
                BeginYear = 2015,
                EndYear = 2016,
                Major = 8,
                Minor = 0
            },
            256 => new FnddsVersion
            {
                Id = id,
                BeginYear = 2017,
                EndYear = 2018,
                Major = 9,
                Minor = 0
            },
            512 => new FnddsVersion
            {
                Id = id,
                BeginYear = 2019,
                EndYear = 2020,
                Major = 10,
                Minor = 0
            },
            1024 => new FnddsVersion
            {
                Id = id,
                BeginYear = 2021,
                EndYear = 2023,
                Major = 11,
                Minor = 0
            },
            _ => default,
        };

        return (version != null);
    }
}
