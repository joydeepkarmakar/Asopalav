--ALTER TABLE YourTable ADD CONSTRAINT DF_YourTable DEFAULT GETDATE() FOR YourColumn

CREATE TABLE [dbo].[OccasionMaster](
	[OccasionId] [int] IDENTITY(1,1) NOT NULL,
	[Occasion] [nvarchar](100) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedOn] [DateTime] NULL,
	[ModifiedOn] [DateTime] NULL,
 CONSTRAINT [PK_OccasionMaster] PRIMARY KEY CLUSTERED 
(
	[OccasionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[GemVariant](
	[GemVariantId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](30) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[Carat] [int] NULL,
	[Weight] [decimal](9, 2) NULL,
	[Price] [decimal](18, 2) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedOn] [DateTime] NULL,
	[ModifiedOn] [DateTime] NULL,
 CONSTRAINT [PK_GemVariant] PRIMARY KEY CLUSTERED 
(
	[GemVariantId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[GemVariant] ADD CONSTRAINT DF_GemVariant_CreatedOn DEFAULT GETDATE() FOR CreatedOn
GO

CREATE TABLE [dbo].[MetalVariant](
	[MetalVariantId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](30) NOT NULL,
	[Specification] [nvarchar](30) NOT NULL,
	[Carat] [int] NOT NULL,
	[UnitValueInGms] [decimal](18, 2) NOT NULL,
	[UnitSellPrice] [decimal](18, 2) NOT NULL,
	[UnitBuyPrice] [decimal](18, 2) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedOn] [DateTime] NULL,
	[ModifiedOn] [DateTime] NULL,
 CONSTRAINT [PK_MetalVariant] PRIMARY KEY CLUSTERED 
(
	[MetalVariantId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[MetalVariant] ADD CONSTRAINT DF_MetalVariant_CreatedOn DEFAULT GETDATE() FOR CreatedOn
GO

ALTER TABLE ProductMaster
ADD 
OccasionId [int] NULL,
OfferStartDate DATETIME NULL,
OfferEndDate DATETIME NULL,
MakingChargePercentage [decimal](18, 2) NULL,
MakingCharge [decimal](18, 2) NULL,
IsMakingChargePercentage [bit] NOT NULL CONSTRAINT [DF_ProductMaster_IsMakingChargePercentage]  DEFAULT ((0)),
MetalVariantId INT NULL,
GemVariantId INT NULL
GO

ALTER TABLE [dbo].[ProductMaster]  WITH CHECK ADD  CONSTRAINT [FK_ProductMaster_MetalVariant] FOREIGN KEY([MetalVariantId])
REFERENCES [dbo].[MetalVariant] ([MetalVariantId])
GO

ALTER TABLE [dbo].[ProductMaster] CHECK CONSTRAINT [FK_ProductMaster_MetalVariant]
GO

ALTER TABLE [dbo].[ProductMaster]  WITH CHECK ADD  CONSTRAINT [FK_ProductMaster_GemVariant] FOREIGN KEY([GemVariantId])
REFERENCES [dbo].[GemVariant] ([GemVariantId])
GO

ALTER TABLE [dbo].[ProductMaster] CHECK CONSTRAINT [FK_ProductMaster_GemVariant]
GO

ALTER TABLE [dbo].[ProductMaster]  WITH CHECK ADD  CONSTRAINT [FK_ProductMaster_OccasionMaster] FOREIGN KEY([OccasionId])
REFERENCES [dbo].[OccasionMaster] ([OccasionId])
GO

ALTER TABLE [dbo].[ProductMaster] CHECK CONSTRAINT [FK_ProductMaster_OccasionMaster]
GO

ALTER TABLE OccasionMaster ADD CONSTRAINT DF_OccasionMaster DEFAULT GETDATE() FOR CreatedOn
GO

INSERT INTO OccasionMaster(Occasion,IsActive)
VALUES ('Makar Sankranti',1),('Navratri',1),('Rath Yatra',1),('Janmashtami',1)
GO

INSERT INTO MetalVariant(Name,Specification,Carat,UnitValueInGms,UnitSellPrice,UnitBuyPrice,IsActive) VALUES
('Gold','22 Carat',22,'1.00','2930.00','2800.00',1),
('Silver','Pure Silver',22,'1.00','120.00','100.00',1)
GO

INSERT INTO GemVariant(Name,[Weight],Price,IsActive) VALUES
('Diamond','1.00','5000.00',1),
('Ruby','1.00','4000.00',1),
('Pearl','1.00','500.00',1),
('Emerald','1.00','3000.00',1)
GO

CREATE TYPE [dbo].[ImgTable]
AS TABLE
(
	[ImageID] [int] NOT NULL,
	[ImageName] [varchar](500) NOT NULL,
	[ImagePath] [varchar](max) NOT NULL,
	[ProductID] [bigint] NULL
)
GO

ALTER PROCEDURE [dbo].[AddUpdateProduct] @ProductID                BIGINT,
                                         @ProductCode              VARCHAR(20),
                                         @ProductName              VARCHAR(100),
                                         @ProductTypeID            INT,
                                         @WeightInGms              DECIMAL(18, 3),
                                         @HeightInInch             VARCHAR(15),
                                         @WidthInInch              VARCHAR(15),
                                         @Price                    DECIMAL(18, 2),
                                         @IsOffer                  BIT,
                                         @OfferPrice               DECIMAL(18, 2),
                                         @IsActive                 BIT,
                                         @Description              VARCHAR(MAX),
                                         @OccasionId               INT,
                                         @OfferStartDate           DATETIME,
                                         @OfferEndDate             DATETIME,
                                         @MakingChargePercentage   DECIMAL(18, 2),
                                         @MakingCharge             DECIMAL(18, 2),
                                         @IsMakingChargePercentage BIT,
                                         @MetalVariantId           INT,
                                         @GemVariantId             INT,
                                         @ImgDetails               [dbo].[ImgTable] READONLY
AS
     BEGIN
         SET NOCOUNT ON;
         DECLARE @TranCount INT, @ErrorMessage VARCHAR(4000);
         SET @TranCount = @@trancount;
         DECLARE @OutputTableVar AS TABLE(OutputProductID INT);
         DECLARE @varImgDetails [dbo].[ImgTable];
         INSERT INTO @varImgDetails
                SELECT ImageID,
                       ImageName,
                       ImagePath,
                       ProductID
                FROM @ImgDetails;
         BEGIN TRY
             IF @TranCount = 0
             BEGIN TRANSACTION;
                 ELSE
             SAVE TRANSACTION AddUpdateProduct;
             IF EXISTS
				(
					SELECT 1
					FROM [dbo].[ProductMaster]
					WHERE ProductCode = @ProductCode
				)
                 BEGIN
                     UPDATE dbo.ProductMaster
                       SET
                           IsActive = 0,
                           ModifyDate = GETDATE()
                     WHERE ProductID = @ProductID
                           AND ProductCode = @ProductCode;
                     IF(@IsActive = 1)
                         BEGIN
                             INSERT INTO [dbo].[ProductMaster]
												(ProductCode,
												 ProductName,
												 ProductTypeID,
												 WeightInGms,
												 HeightInInch,
												 WidthInInch,
												 Price,
												 IsOffer,
												 OfferPrice,
												 IsActive,
												 [Description],
												 OccasionId,
												 OfferStartDate,
												 OfferEndDate,
												 MakingChargePercentage,
												 MakingCharge,
												 IsMakingChargePercentage,
												 MetalVariantId,
												 GemVariantId
												)
                             OUTPUT INSERTED.ProductID
                                    INTO @OutputTableVar
                                    SELECT ProductCode,
                                           CASE
                                               WHEN LOWER(RTRIM(LTRIM(ProductName))) = LOWER(RTRIM(LTRIM(@ProductName)))
                                               THEN ProductName
                                               ELSE @ProductName
                                           END AS ProductName,
                                           CASE
                                               WHEN ProductTypeID = @ProductTypeID
                                               THEN ProductTypeID
                                               ELSE @ProductTypeID
                                           END AS ProductTypeID,
                                           CASE
                                               WHEN WeightInGms = @WeightInGms
                                               THEN WeightInGms
                                               ELSE @WeightInGms
                                           END AS WeightInGms,
                                           CASE
                                               WHEN HeightInInch = @HeightInInch
                                               THEN HeightInInch
                                               ELSE @HeightInInch
                                           END AS HeightInInch,
                                           CASE
                                               WHEN WidthInInch = @WidthInInch
                                               THEN WidthInInch
                                               ELSE @WidthInInch
                                           END AS WidthInInch,
                                           CASE
                                               WHEN Price = @Price
                                               THEN Price
                                               ELSE @Price
                                           END AS Price,
                                           CASE
                                               WHEN IsOffer = @IsOffer
                                               THEN IsOffer
                                               ELSE @IsOffer
                                           END AS IsOffer,
                                           CASE
                                               WHEN OfferPrice = @OfferPrice
                                               THEN OfferPrice
                                               ELSE @OfferPrice
                                           END AS OfferPrice,
                                           1 AS IsActive,
                                           CASE
                                               WHEN LOWER(RTRIM(LTRIM([Description]))) = LOWER(RTRIM(LTRIM(@Description)))
                                               THEN [Description]
                                               ELSE @Description
                                           END AS Description,
                                           CASE
                                               WHEN OccasionId = @OccasionId
                                               THEN OccasionId
                                               ELSE @OccasionId
                                           END AS OccasionId,
                                           CASE
                                               WHEN OfferStartDate = @OfferStartDate
                                               THEN OfferStartDate
                                               ELSE @OfferStartDate
                                           END AS OfferStartDate,
                                           CASE
                                               WHEN OfferEndDate = @OfferEndDate
                                               THEN OfferEndDate
                                               ELSE @OfferEndDate
                                           END AS OfferEndDate,
                                           CASE
                                               WHEN MakingChargePercentage = @MakingChargePercentage
                                               THEN MakingChargePercentage
                                               ELSE @MakingChargePercentage
                                           END AS MakingChargePercentage,
                                           CASE
                                               WHEN MakingCharge = @MakingCharge
                                               THEN MakingCharge
                                               ELSE @MakingCharge
                                           END AS MakingCharge,
                                           CASE
                                               WHEN IsMakingChargePercentage = @IsMakingChargePercentage
                                               THEN IsMakingChargePercentage
                                               ELSE @IsMakingChargePercentage
                                           END AS IsMakingChargePercentage,
                                           CASE
                                               WHEN MetalVariantId = @MetalVariantId
                                               THEN MetalVariantId
                                               ELSE @MetalVariantId
                                           END AS MetalVariantId,
                                           CASE
                                               WHEN GemVariantId = @GemVariantId
                                               THEN GemVariantId
                                               ELSE @GemVariantId
                                           END AS GemVariantId
                                    FROM dbo.ProductMaster
                                    WHERE ProductCode = @ProductCode;
                         END;
                     UPDATE @varImgDetails
                       SET
                           ProductID =
										(
											SELECT OutputProductID
											FROM @OutputTableVar
										);
                     INSERT INTO dbo.Images
								(ImageName,
									ImagePath,
									ProductID
								)
                            SELECT ImageName,
                                   ImagePath,
                                   ProductID
                            FROM @varImgDetails;
                 END;
             lbexit:
             IF @@TRANCOUNT = 0
                 COMMIT;
         END TRY
         BEGIN CATCH
             DECLARE @error INT, @xstate INT;
             SELECT @error = ERROR_NUMBER(),
                    @ErrorMessage = ERROR_MESSAGE(),
                    @xstate = XACT_STATE();
             IF @xstate = -1
                 ROLLBACK;
             IF @xstate = 1
                AND @trancount = 0
                 ROLLBACK;
             IF @xstate = 1
                AND @trancount > 0
                 ROLLBACK TRANSACTION AddUpdateProduct;
             RAISERROR('AddUpdateProduct: %d: %s', 16, 1, @error, @ErrorMessage);
         END CATCH;
     END;
GO;
/*******************************************************EXECUTED ON PRODUCTION 20181018***********************************************************/
CREATE PROCEDURE [dbo].[GetLatestOfferProducts] @CurrentCurrency CHAR(5)     = NULL,
                                                @ConversionRate  VARCHAR(50) = NULL
AS
     BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
         SET NOCOUNT ON;
         SELECT TOP 10 P.ProductID,
                       P.ProductCode,
                       P.ProductName,
                       P.ProductTypeID,
                       P.WeightInGms,
                       P.HeightInInch,
                       P.WidthInInch,
                       CASE
                           WHEN 'INR' = @CurrentCurrency
                           THEN ROUND((P.Price * @ConversionRate), 2)
                           ELSE ROUND(P.Price, 2)
                       END AS Price,
                       P.IsOffer,
                       CASE
                           WHEN 'INR' = @CurrentCurrency
                           THEN ROUND((P.OfferPrice * @ConversionRate), 2)
                           ELSE ROUND(P.OfferPrice, 2)
                       END AS OfferPrice,
                       P.IsActive,
                       P.[Description],
                       P.CreationDate,
					   P.OfferStartDate,
                       P.OfferEndDate,
                       P.IsMakingChargePercentage,
                       P.MakingChargePercentage,
                       P.MakingCharge,
                       I.ImageName,
                       I.ImagePath
         FROM ProductMaster P
              INNER JOIN
						(
							SELECT ImageName,
								   ImagePath,
								   ProductID
							FROM
						(
							SELECT ROW_NUMBER() OVER(PARTITION BY ProductID ORDER BY ImageID) AS RN,
								   ImageName,
								   ImagePath,
								   ProductID
							FROM Images
						) T
							WHERE T.RN = 1
						) I ON P.ProductID = I.ProductID 
							AND P.IsActive = 1 
							AND P.IsOffer = 1 
							AND GETDATE() BETWEEN P.OfferStartDate AND P.OfferEndDate
							ORDER BY CreationDate DESC;
     END;

GO
/*******************************************************EXECUTED ON PRODUCTION 20181019***********************************************************/