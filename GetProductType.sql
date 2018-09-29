CREATE PROC dbo.GetProductType
AS
     BEGIN
         WITH ProductTypeHierarchy(Id,
                                   ProductType,
                                   ParentId,
                                   ParentProductType,
                                   Sort,
                                   [Level])
              AS (
    -- anchor member
              SELECT ProductTypeID AS Id,
                     ProductType,
                     ParentProductTypeId AS ParentId,
                     CAST('' AS NVARCHAR(200)) AS ParentProductType,
                     CAST(ROW_NUMBER() OVER(PARTITION BY ParentProductTypeId ORDER BY ProductType) AS INT) AS Sort,
                     0 AS [Level]
              FROM dbo.[ProductTypeMaster]
              WHERE ParentProductTypeId IS NULL
                    AND IsActive = 1
              UNION ALL
     -- recursive members
              SELECT PT.ProductTypeID AS Id,
                     PT.ProductType,
                     PT.ParentProductTypeId AS ParentId,
                     CAST(ISNULL(Hierarchy.ProductType, '') AS NVARCHAR(200)) AS ParentProductType,
                     CAST(ROW_NUMBER() OVER(PARTITION BY Hierarchy.Id ORDER BY PT.ProductType) AS INT) AS Sort,
                     [Level] + 1 AS [Level]
              FROM dbo.[ProductTypeMaster] AS PT
                   INNER JOIN ProductTypeHierarchy Hierarchy ON PT.ParentProductTypeId = Hierarchy.Id)
              SELECT Id,
                     ProductType,
                     ParentId,
                     ParentProductType,
                     Sort,
                     [Level]
              FROM ProductTypeHierarchy
              ORDER BY [Level],
                       ParentProductType,
                       Sort;
     END;