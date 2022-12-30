using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rlask_engine
{
    /// <summary>
    /// Database context is relying on Entity Framework Core and SQLite. The connection string is provided 
    /// using $env:connectionString environment variable. 
    /// The model relies on 5 data classes: Invoices, Contractors, Customers, InvoiceRows and Products
    /// </summary>

    public class InvoicingContext : DbContext
    {
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Contractor> Contractors { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<InvoiceRow> InvoiceRows { get; set; }
        public DbSet<Product> Products { get; set; }

        public InvoicingContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite(Environment.GetEnvironmentVariable("connectionString"));
    }

    public class Invoice
    {
        public int InvoiceId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public Contractor Contractor { get; set; }
        public Customer Customer { get; set; }
        public List<InvoiceRow> InvoiceRows { get; } = new();
        public string ExtraDetails { get; set; }
        public decimal TotalSum { get => InvoiceRows.Count() == 0 ? 0 : InvoiceRows.Sum(n => n.Total); }

        public override string ToString()
        {
            return $"LASKU\n" +
                $"Laskuttaja\n{Contractor.ContractorName}\n" +
                $"{Contractor.ContractorAddress}\n" +
                $"Päiväys: {InvoiceDate:dd.MM.yyyy}\n" +
                $"Laskun numero: {InvoiceId}\n" +
                $"Eräpäivä: {DueDate:dd.MM.yyyy}\n\n" +
                $"Asiakas:\n{Customer.CustomerName}\n" +
                $"{Customer.CustomerAddress}\n\n" +
                $"Lisätiedot: {ExtraDetails}\n\n" +
                $"{RowsGridView()}\n" +
                $"YHTEENSÄ: {TotalSum}\n\n";
        }

        

        public string RowsGridView()
        {
            string Pad(string[] values, int Width) => $"|{values[0].PadRight(Width, ' ')}|{values[1],20}|{values[2],15}|{values[3],15}|{values[4],15}|";
            
            int width = InvoiceRows.Select(n => n.Product.ProductName).Select(n => n.Length).Max();
            string headers = Pad(new string[] { "Tuote", "Määrä", "Yksikkö", "A-hinta", "Yhteensä" }, width);
            string[] rows = InvoiceRows.Select(n => Pad(n.GetValuesAsArray(), width)).ToArray();

            return headers + '\n' + string.Join('\n', rows);
        }
    }

    public class Contractor
    {
        public int ContractorId { get; set; }
        public string ContractorName { get; set; }
        public string ContractorAddress { get; set; }
    }

    public class Customer
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
    }

    public class InvoiceRow
    {
        public int InvoiceRowId { get; set; }
        public Product Product { get; set; }
        public decimal Amount { get; set; }
        public Invoice Invoice { get; set; }
        public decimal Total { get; set; }
        public string[] GetValuesAsArray() => 
            new string[] { Product.ProductName, Amount.ToString("N2"), Product.Unit, Product.UnitPrice.ToString("N2"), Total.ToString("N2") };
    }

    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }
        public decimal UnitPrice { get; set; }
        public bool IsMaterial { get; set; }
        public override string ToString() => $"Tuote: {ProductName}, kappalehinta: {UnitPrice:N2}/{Unit}";
    }
}
