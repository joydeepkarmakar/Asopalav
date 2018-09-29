CREATE PROC [dbo].[SearchProducts] @SearchText      NVARCHAR(500),
                                  @CurrentCurrency CHAR(5)       = NULL,
                                  @ConversionRate  VARCHAR(50)   = NULL
AS
     BEGIN
         SELECT P.ProductID,
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
              INNER JOIN ProductTypeMaster PT ON P.ProductTypeID = PT.ProductTypeID
         WHERE PT.ProductType LIKE '%'+@SearchText+'%'
               OR P.ProductName LIKE '%'+@SearchText+'%'
               OR P.ProductCode LIKE '%'+@SearchText+'%';
     END;