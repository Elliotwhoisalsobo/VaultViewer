INSERT INTO department (DepartmentID, Name, Description) VALUES
(1, 'Sales', 'Responsible for customer relationships'),
(2, 'IT', 'Handles infrastructure and development'),
(3, 'Finance', 'Manages company finances');


INSERT INTO role (RoleID, Name, Description, DepartmentID) VALUES
(1, 'Sales Rep', 'Handles sales calls', 1),
(2, 'Developer', 'Builds software', 2),
(3, 'Accountant', 'Manages invoices', 3);


INSERT INTO employee (EmployeeID, FirstName, LastName, DateOfBirth, AddressLine1, AddressLine2, PostalCode, PostalCity, Country, EmploymentDate, CurrentMonthlySalary) VALUES
(1, 'Alice', 'Smith', '1990-05-12', 'Main St 1', '', '1000', 'Brussels', 'BE', '2020-01-15', 3200.00),
(2, 'Bob', 'Jones', '1985-09-23', 'Elm St 5', 'Apt 2', '2000', 'Antwerp', 'BE', '2019-06-01', 3500.00),
(3, 'Charlie', 'Brown', '1992-11-02', 'Oak St 7', NULL, '3000', 'Ghent', 'BE', '2021-03-10', 2900.00);


INSERT INTO employeeLogin (UserName, PasswordHash, EmployeeID) VALUES
('alice.smith', 'hashedpassword1', 1),
('bob.jones', 'hashedpassword2', 2),
('charlie.brown', 'hashedpassword3', 3);

INSERT INTO employeeRole (EmployeeID, RoleID) VALUES
(1, 1),  -- Alice is Sales Rep
(2, 2),  -- Bob is Developer
(3, 3);  -- Charlie is Accountant


INSERT INTO customer (CustomerID, Name, AddressLine1, AddressLine2, PostalCode, PostalCity, Country, ContactPerson, VATNumber, EmailAddress, PhoneNumber, IsCompany, IsActive) VALUES
(1, 'TechWorld', 'Tech Street 10', '', '1010', 'Brussels', 'BE', 'Sarah Tech', 'BE0123456789', 'contact@techworld.be', '0123456789', 1, 1),
(2, 'GreenFarm', 'Nature Lane 22', NULL, '2020', 'Antwerp', 'BE', 'Mark Green', 'BE0987654321', 'info@greenfarm.be', '0987654321', 1, 1),
(3, 'John Doe', 'Private Rd 4', NULL, '3030', 'Leuven', 'BE', 'John Doe', NULL, 'john.doe@example.com', '0478123456', 0, 1);


INSERT INTO product (ProductID, Name, Description, Price) VALUES
(1, 'Laptop', '15-inch business laptop', 999.99),
(2, 'Mouse', 'Wireless mouse', 29.95),
(3, 'Monitor', '24-inch HD monitor', 179.99);


INSERT INTO invoice (InvoiceID, EmployeeID, CustomerID, OurReference, YourReference, InvoiceDate, PaymentDueDate) VALUES
(1, 1, 1, 'AL-001', 'PO-789', '2025-04-01', '2025-04-30'),
(2, 3, 2, 'CH-001', 'PO-234', '2025-03-20', '2025-04-20'),
(3, 1, 3, 'AL-002', 'PO-999', '2025-04-03', '2025-04-15');


INSERT INTO invoiceLine (InvoiceLineID, InvoiceID, ProductID, Amount) VALUES
(1, 1, 1, 1), -- 1 laptop
(2, 1, 2, 2), -- 2 mice
(3, 2, 3, 1), -- 1 monitor
(4, 3, 2, 1); -- 1 mouse
