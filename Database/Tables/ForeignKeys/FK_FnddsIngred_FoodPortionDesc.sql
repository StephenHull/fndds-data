ALTER TABLE [dbo].FnddsIngred
ADD CONSTRAINT FK_FnddsIngred_FoodPortionDesc
    FOREIGN KEY
    (
        PortionCode,
        VersionID
    )
    REFERENCES [dbo].FoodPortionDesc
    (
        PortionCode,
        VersionID
    )