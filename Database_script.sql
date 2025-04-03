-- CREATE DATABASE VaultViewer;
USE VaultViewer;

CREATE TABLE Employee (
    EmployeeID INT PRIMARY KEY,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    DateOfBirth DATE NOT NULL,
    AddressLine1 VARCHAR(150) NOT NULL,
    AddressLine2 VARCHAR(150),
    PostalCode VARCHAR(20) NOT NULL,
    PostalCity VARCHAR(50) NOT NULL,
    Country CHAR(2) NOT NULL,
    EmploymentDate DATE NOT NULL,
    CurrentMonthlySalary DECIMAL(10,2) NOT NULL
);

CREATE TABLE EmployeeLogin (
    UserName VARCHAR(50) PRIMARY KEY,
    PasswordHash VARCHAR(512) NOT NULL,
    EmployeeID INT NOT NULL,
    FOREIGN KEY (EmployeeID) REFERENCES Employee(EmployeeID)
);

CREATE TABLE Customer (
    CustomerID INT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    AddressLine1 VARCHAR(100) NOT NULL,
    AddressLine2 VARCHAR(100),
    PostalCode VARCHAR(20) NOT NULL,
    PostalCity VARCHAR(50) NOT NULL,
    Country CHAR(2) NOT NULL,
    ContactPerson VARCHAR(100) NOT NULL,
    VATNumber VARCHAR(20),
    EmailAddress VARCHAR(125) NOT NULL,
    PhoneNumber VARCHAR(20) NOT NULL,
    IsCompany BOOLEAN NOT NULL,
    IsActive BOOLEAN NOT NULL
);

CREATE TABLE Invoice (
    InvoiceID INT PRIMARY KEY,
    EmployeeID INT NOT NULL,
    CustomerID INT NOT NULL,
    OurReference VARCHAR(70) NOT NULL,
    YourReference VARCHAR(70),
    InvoiceDate DATETIME NOT NULL,
    PaymentDueDate DATE NOT NULL,
    FOREIGN KEY (EmployeeID) REFERENCES Employee(EmployeeID),
    FOREIGN KEY (CustomerID) REFERENCES Customer(CustomerID)
);

CREATE TABLE Product (
    ProductID INT PRIMARY KEY,
    Name VARCHAR(150) NOT NULL,
    Description VARCHAR(100),
    Price DECIMAL(10,2) NOT NULL
);

CREATE TABLE InvoiceLine (
    InvoiceLineID INT PRIMARY KEY,
    InvoiceID INT NOT NULL,
    ProductID INT NOT NULL,
    Amount DECIMAL(10,2) NOT NULL, # 8 chars
    FOREIGN KEY (InvoiceID) REFERENCES Invoice(InvoiceID),
    FOREIGN KEY (ProductID) REFERENCES Product(ProductID)
);

CREATE TABLE Department (
    DepartmentID INT PRIMARY KEY,
    Name VARCHAR(25) NOT NULL,
    Description VARCHAR(100)
);

CREATE TABLE Role (
    RoleID INT PRIMARY KEY,
    Name VARCHAR(20) NOT NULL,
    Description VARCHAR(100),
    DepartmentID INT NOT NULL,
    FOREIGN KEY (DepartmentID) REFERENCES Department(DepartmentID)
);

CREATE TABLE EmployeeRole (
    EmployeeID INT NOT NULL,
    RoleID INT NOT NULL,
    PRIMARY KEY (EmployeeID, RoleID),
    FOREIGN KEY (EmployeeID) REFERENCES Employee(EmployeeID),
    FOREIGN KEY (RoleID) REFERENCES Role(RoleID)
);
