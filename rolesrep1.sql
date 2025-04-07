set @username = "alice.smith";
set @password = "hashedpassword1";
SELECT r.Name
FROM
    EmployeeLogin l JOIN
    Employee e ON l.EmployeeId = e.EmployeeId JOIN
    EmployeeRole er ON e.EmployeeId = er.EmployeeId JOIN
    Role r ON er.RoleId = r.RoleId
WHERE
    l.UserName = @UserName AND
    l.PasswordHash = @Password; 