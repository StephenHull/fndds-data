CREATE TABLE [dbo].[FnddsVersion]
(
    ID INT,
    BeginYear INT NOT NULL,
    EndYear INT NOT NULL,
    Major INT NULL,
    Minor INT NULL,
    CreateDT DATETIME2
        DEFAULT GETUTCDATE() NOT NULL,
    CONSTRAINT PK_FnddsVersion
        PRIMARY KEY (ID)
)
