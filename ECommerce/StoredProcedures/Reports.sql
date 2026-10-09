CREATE OR ALTER PROCEDURE sp_GetTotalProducts
AS
BEGIN
    SELECT COUNT(*) AS TotalProducts
    FROM Products;
END;
GO


CREATE OR ALTER PROCEDURE sp_GetTotalCustomers
AS
BEGIN
    SELECT COUNT(*) AS TotalCustomers
    FROM Customers;
END;
GO


CREATE OR ALTER PROCEDURE sp_GetTotalOrders
AS
BEGIN
    SELECT COUNT(*) AS TotalOrders
    FROM Orders;
END;
GO


CREATE OR ALTER PROCEDURE sp_GetTotalRevenue
AS
BEGIN
    SELECT ISNULL(SUM(TotalAmount), 0) AS TotalRevenue
    FROM Orders
    WHERE Status <> 2;
END;
GO


CREATE OR ALTER PROCEDURE sp_GetTopSellingProduct
AS
BEGIN
    SELECT TOP 1
        p.ProductName,
        SUM(oi.Quantity) AS UnitsSold
    FROM OrderItems oi
    INNER JOIN Products p
        ON oi.ProductId = p.ProductId
    INNER JOIN Orders o
        ON oi.OrderId = o.OrderId
    WHERE o.Status <> 2
    GROUP BY
        p.ProductId,
        p.ProductName
    ORDER BY
        UnitsSold DESC;
END;
GO


CREATE OR ALTER PROCEDURE sp_GetAverageOrderValue
AS
BEGIN
    SELECT
        ISNULL(AVG(TotalAmount), 0) AS AverageOrderValue
    FROM Orders
    WHERE Status <> 2;
END;
GO


CREATE OR ALTER PROCEDURE sp_GetCategoryWiseProductCount
AS
BEGIN
    SELECT
        c.CategoryName,
        COUNT(p.ProductId) AS ProductCount
    FROM Categories c
    LEFT JOIN Products p
        ON c.CategoryId = p.CategoryId
    GROUP BY
        c.CategoryId,
        c.CategoryName
    ORDER BY
        ProductCount DESC;
END;
GO