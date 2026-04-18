namespace FnddsData.Fndds.Interfaces;

public interface IFnddsVersion
{
    int Id { get; set; }

    int BeginYear { get; set; }

    int EndYear { get; set; }

    int? Major { get; set; }

    int? Minor { get; set; }

    DateTime CreateDt { get; set; }
}
