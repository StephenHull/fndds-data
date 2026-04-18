CREATE TABLE [dbo].[FoodWeight]
(
    FoodCode INT,
    Subcode INT
        DEFAULT 0,
    SeqNum INT,
    PortionCode INT,
    VersionID INT,
    StartDT DATETIME2 NULL,
    EndDT DATETIME2 NULL,
    PortionWeight DECIMAL(8, 3) NOT NULL,
    ChangeType VARCHAR(1) NULL,
    CreateDT DATETIME2
        DEFAULT GETUTCDATE() NOT NULL,
    CONSTRAINT PK_FoodWeight
        PRIMARY KEY
        (
            FoodCode,
            Subcode,
            SeqNum,
            PortionCode,
            VersionID
        )
)
