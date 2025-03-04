using OrderService.Data;
using OrderService.Models;

namespace OrderService.Seeders
{
    public static class SeedData
    {
        public static void Initialize(ErpDbContext context)
        {
            // Check if data already exists to avoid re-seeding
            if (context.Users.Any() || context.Orders.Any()) return;

            // Seed Users (10 records) without explicit Ids
            var users = new[]
            {
                new User { Username = "admin", PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"), Email = "admin@erp.com" },
                new User { Username = "manager1", PasswordHash = BCrypt.Net.BCrypt.HashPassword("pass123"), Email = "manager1@erp.com" },
                new User { Username = "manager2", PasswordHash = BCrypt.Net.BCrypt.HashPassword("pass123"), Email = "manager2@erp.com" },
                new User { Username = "employee1", PasswordHash = BCrypt.Net.BCrypt.HashPassword("emp123"), Email = "employee1@erp.com" },
                new User { Username = "employee2", PasswordHash = BCrypt.Net.BCrypt.HashPassword("emp123"), Email = "employee2@erp.com" },
                new User { Username = "employee3", PasswordHash = BCrypt.Net.BCrypt.HashPassword("emp123"), Email = "employee3@erp.com" },
                new User { Username = "customer1", PasswordHash = BCrypt.Net.BCrypt.HashPassword("cust123"), Email = "customer1@erp.com" },
                new User { Username = "customer2", PasswordHash = BCrypt.Net.BCrypt.HashPassword("cust123"), Email = "customer2@erp.com" },
                new User { Username = "customer3", PasswordHash = BCrypt.Net.BCrypt.HashPassword("cust123"), Email = "customer3@erp.com" },
                new User { Username = "guest", PasswordHash = BCrypt.Net.BCrypt.HashPassword("guest123"), Email = "guest@erp.com" }
            };
            context.Users.AddRange(users);
            context.SaveChanges(); // Save users first to generate their IDs

            // Seed Orders (10 records) without explicit Ids, linking to saved users
            var orders = new[]
            {
                new Order { OrderNumber = "ORD-001", CustomerName = "John Doe", TotalAmount = 150.00M, CreatedAt = DateTime.UtcNow, UserId = users[0].Id },
                new Order { OrderNumber = "ORD-002", CustomerName = "Jane Smith", TotalAmount = 200.50M, CreatedAt = DateTime.UtcNow, UserId = users[1].Id },
                new Order { OrderNumber = "ORD-003", CustomerName = "Alice Brown", TotalAmount = 75.25M, CreatedAt = DateTime.UtcNow, UserId = users[2].Id },
                new Order { OrderNumber = "ORD-004", CustomerName = "Bob Johnson", TotalAmount = 300.00M, CreatedAt = DateTime.UtcNow, UserId = users[3].Id },
                new Order { OrderNumber = "ORD-005", CustomerName = "Charlie Lee", TotalAmount = 50.75M, CreatedAt = DateTime.UtcNow, UserId = users[4].Id },
                new Order { OrderNumber = "ORD-006", CustomerName = "David Kim", TotalAmount = 120.00M, CreatedAt = DateTime.UtcNow, UserId = users[5].Id },
                new Order { OrderNumber = "ORD-007", CustomerName = "Eve White", TotalAmount = 90.30M, CreatedAt = DateTime.UtcNow, UserId = users[6].Id },
                new Order { OrderNumber = "ORD-008", CustomerName = "Frank Green", TotalAmount = 250.00M, CreatedAt = DateTime.UtcNow, UserId = users[7].Id },
                new Order { OrderNumber = "ORD-009", CustomerName = "Grace Taylor", TotalAmount = 180.90M, CreatedAt = DateTime.UtcNow, UserId = users[8].Id },
                new Order { OrderNumber = "ORD-010", CustomerName = "Hank Miller", TotalAmount = 99.99M, CreatedAt = DateTime.UtcNow, UserId = users[9].Id }
            };
            context.Orders.AddRange(orders);

            context.SaveChanges();
        }
    }
}