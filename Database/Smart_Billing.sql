
CREATE DATABASE SmartBillingDB
use SmartBillingDB

CREATE TABLE Categories (
    CategoryId INT PRIMARY KEY IDENTITY(1,1),
    CategoryName NVARCHAR(100) NOT NULL
);

CREATE TABLE Products (
    ProductId INT PRIMARY KEY IDENTITY(1,1),
    ProductName NVARCHAR(150) NOT NULL,
    Barcode NVARCHAR(50) UNIQUE NOT NULL,
    Price DECIMAL(10,2) NOT NULL,
    Stock INT NOT NULL,
    CategoryId INT FOREIGN KEY REFERENCES Categories(CategoryId),
    CreatedDate DATETIME DEFAULT GETDATE()
);

CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(100) NOT NULL,
    Password NVARCHAR(100) NOT NULL,
    Role NVARCHAR(50) NOT NULL
);

CREATE TABLE Bills (
    BillId INT PRIMARY KEY IDENTITY(1,1),
    BillNumber NVARCHAR(50) NOT NULL,
    BillDate DATETIME DEFAULT GETDATE(),
    TotalAmount DECIMAL(12,2) NOT NULL,
    CreatedBy INT FOREIGN KEY REFERENCES Users(UserId)
);

CREATE TABLE BillItems (
    BillItemId INT PRIMARY KEY IDENTITY(1,1),
    BillId INT FOREIGN KEY REFERENCES Bills(BillId),
    ProductId INT FOREIGN KEY REFERENCES Products(ProductId),
    Quantity INT NOT NULL,
    Price DECIMAL(10,2) NOT NULL,
    Total DECIMAL(12,2) NOT NULL
);


CREATE TABLE InventoryLogs
(
    LogId INT IDENTITY(1,1) PRIMARY KEY,
    
    ProductId INT NOT NULL,
    
    ChangeType NVARCHAR(50) NOT NULL, 
    -- 'SALE', 'PURCHASE', 'ADJUSTMENT', 'RETURN'
    
    QuantityChanged INT NOT NULL,
    
    PreviousStock INT NOT NULL,
    
    NewStock INT NOT NULL,
    
    ReferenceId INT NULL, 
    -- BillId or OrderId (for tracking source)
    
    Remarks NVARCHAR(250) NULL,
    
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_InventoryLogs_Products 
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
);

ALTER TABLE InventoryLogs
ADD CONSTRAINT PK_InventoryLogs PRIMARY KEY (LogId);

------------------------------------------------------------------------


select * from BillItems
EXEC sp_rename 'OrderItems', 'BillItems';
---------------------------------------------------------------------------


INSERT INTO Categories (CategoryName)
VALUES ('Snacks'), ('Beverages'), ('Groceries');

INSERT INTO Products (ProductName, Barcode, Price, Stock, CategoryId)
VALUES 
('Coca Cola 500ml', '8901234567890', 40, 100, 2),
('Lays Chips', '8909876543210', 20, 150, 1),
('Rice 1KG', '8901112223334', 60, 80, 3),
('Dairy Milk', '8904445556667', 50, 120, 1);

INSERT INTO Products (ProductName, Barcode, Price, Stock, CategoryId)
VALUES 
('Pepsi 500ml', '8901112223334', 38, 90, 2),
('Maggi Noodles', '8902223334445', 15, 200, 1),
('Sunlight Detergent', '8905556667778', 55, 60, 4),
('Oreo Biscuits', '8907778889990', 30, 110, 1),
('Amul Butter 100g', '8908889990001', 58, 45, 3),
('Nescafe Coffee 50g', '8909990001112', 150, 40, 2);


-- This will create CategoryId 4
INSERT INTO Categories (CategoryName) VALUES ('Household');

-- NOW you can run your product insert for Sunlight Detergent
INSERT INTO Products (ProductName, Barcode, Price, Stock, CategoryId)
VALUES ('Sunlight Detergent', '8905556667778', 55, 60, 4);


select * from Bills
select * from products
select * from BillItems
select * from Categories
select * from Users
select * from InventoryLogs


SELECT * FROM Bills ORDER BY BillId DESC;
SELECT * FROM BillItems ORDER BY BillItemId DESC;
SELECT ProductName, Stock FROM Products;
SELECT * FROM InventoryLogs ORDER BY LogId DESC;

================================================


INSERT INTO Users (Username, Password, Role)
VALUES ('admin', 'admin123', 'Admin');
INSERT INTO Users (Username, Password, Role)
VALUES ('cashier1', '123', 'Cashier');

INSERT INTO Users (Username, Password, Role)
VALUES ('cashier2', '123', 'Cashier');


SELECT Barcode FROM Products
