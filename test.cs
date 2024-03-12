class Program {
    private static readonly CustomerDbContext Db = new CustomerDbContext();

    private static IQueryable<Customer> GetCustomers() {
        // first query potentially executed and cached
        return Db.Customers;
    }

    private static IQueryable<Sale> GetSales() {
        // second query potentially executed and cached
        return from s in Db.Sales 
                where s.IsPaid
                select s;
    }

    static void Main(string[] args) {
        var totals = (from s in GetSales() // invoke second query
                        join c in GetCustomers() on s.CustomerId equals c.CustomerId // invoke first query
                        group s.Amount by c into g 
                        select new {
                            g.Key.CustomerId,
                            g.Key.FirstName,
                            g.Key.LastName,
                            TotalAmount = g.Sum(a => a)
                        }
                    ).ToList(); // third query invocation
    }

    // so the anwer would be 3 Queries would be executed in this code snippet
}