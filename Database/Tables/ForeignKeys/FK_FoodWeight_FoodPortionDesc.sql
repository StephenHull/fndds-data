ALTER TABLE [dbo].FoodWeight
ADD CONSTRAINT FK_FoodWeight_FoodPortionDesc
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
