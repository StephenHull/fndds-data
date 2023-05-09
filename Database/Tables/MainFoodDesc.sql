CREATE TABLE [dbo].[MainFoodDesc]
(
	FoodCode INT,
	[Version] INT,
	StartDate DATETIME NOT NULL,
	EndDate DATETIME NOT NULL,
	MainFoodDescription VARCHAR(200) NOT NULL,
	AbbreviatedMainFoodDescription VARCHAR(60) NULL,
	FortificationIdentifier VARCHAR(2)  NULL,
	CategoryNumber INT NULL,
	CategoryDescription VARCHAR(80) NULL,
	Created DATETIME DEFAULT CURRENT_TIMESTAMP NOT NULL,
	CONSTRAINT PK_MainFoodDesc PRIMARY KEY (FoodCode, [Version])
)