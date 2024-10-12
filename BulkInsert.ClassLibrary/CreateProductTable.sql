CREATE TABLE dbo.Products
(
    Id INT PRIMARY KEY IDENTITY(1,1),  -- Assuming Id is auto-generated
    Name NVARCHAR(255),                -- String field for the product name
    ImageUrl NVARCHAR(500),            -- String field for the image URL
    Description NVARCHAR(MAX),         -- String field for the description (use MAX for longer descriptions)
    ExternalId INT NOT NULL,           -- Integer field for the external identifier
    Amount FLOAT                       -- Float field for the amount
);
