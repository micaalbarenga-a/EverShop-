
--ELIMINA, CREA Y UTILIZA BD
USE master;
GO
IF EXISTS (SELECT * FROM sys.databases WHERE name = 'EverShop')
BEGIN	
	DROP DATABASE EverShop;
END

CREATE DATABASE EverShop;
GO
USE EverShop;
GO

--Tabla para manejo de productos
CREATE TABLE Products (
	ProId INT IDENTITY(1,1) PRIMARY KEY,
	ProName NVARCHAR(500) NOT NULL,
	ProCode NVARCHAR(100) NOT NULL,
	ProPrice float
);

--Tabla para manejo de usuarios (y customers)
CREATE TABLE Users (
	UseId INT IDENTITY(1,1) PRIMARY KEY,
	UseName NVARCHAR(80) NOT NULL,
	UseMail NVARCHAR(120) NOT NULL,	
	UseMobile NVARCHAR(40) NOT NULL,
	UsePassword NVARCHAR(30) NOT NULL,
	UseAdmin bit NOT NULL DEFAULT(0)
);


--tabla para manejo de órdenes
CREATE TABLE Orders 
(
	OrdId INT IDENTITY(1,1) PRIMARY KEY,
	OrdUser INT NOT NULL,
	OrdProduct INT NOT NULL,
	OrdStatus NVARCHAR(20) NOT NULL CHECK(OrdStatus IN ('CREATED', 'PAYED', 'REJECTED')),
	OrdCreatedAt DATETIME NOT NULL DEFAULT(GETDATE()),
	OrdUpdatedAt DATETIME,
	
	FOREIGN KEY (OrdProduct) REFERENCES Products(ProId),
	FOREIGN KEY (OrdUser) REFERENCES Users(UseId)
);


--Registros para pruebas [Products]
INSERT INTO Products (ProName, ProCode, ProPrice) VALUES ('Product 1', '1234', 1100)
INSERT INTO Products (ProName, ProCode, ProPrice) VALUES ('Product 2', '1221', 1500)
INSERT INTO Products (ProName, ProCode, ProPrice) VALUES ('Product 3', '1222', 1000)

--Registros para pruebas[Users]
INSERT INTO Users (UseName, UseMail, UseMobile, UsePassword, UseAdmin) VALUES ('Admin', 'admin@mail.com', '099999999', '1234', 1)
INSERT INTO Users (UseName, UseMail, UseMobile, UsePassword) VALUES ('Customer 2','Customer2@mail.com', '098888888', '1221')
INSERT INTO Users (UseName, UseMail, UseMobile, UsePassword) VALUES ('Customer 3', 'Customer3@mail.com', '095555555','1221')



--Registros para pruebas [Orders]
INSERT INTO Orders (OrdProduct, OrdUser, OrdStatus, OrdCreatedAt) VALUES (1,1, 'CREATED', '2021-05-22')
INSERT INTO Orders (OrdProduct, OrdUser, OrdStatus, OrdCreatedAt) VALUES (2,2, 'PAYED', '2021-02-15')
INSERT INTO Orders (OrdProduct, OrdUser, OrdStatus, OrdCreatedAt) VALUES (3, 2, 'REJECTED', '2021-05-23')
INSERT INTO Orders (OrdProduct, OrdUser, OrdStatus, OrdCreatedAt) VALUES (3, 3, 'PAYED', GETDATE())

