SELECT
        
            p.Name,
            p.Price,
            c.FirstName + ' ' + c.LastName AS FullName
            FROM Product p 
            INNER JOIN OrderProducts op 
              ON p.IdProduct = op.IdProduct 
            INNER JOIN CustomerOrder co
              ON op.IdOrder = co.IdOrder 
            INNER JOIN Customer c
                ON co.IdCustomer = c.IdCustomer
            WHERE c.IdCustomer =2
              
           