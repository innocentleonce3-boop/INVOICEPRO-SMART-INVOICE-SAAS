using InvoicePro.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoicePro.Infrastructure.Data;

public sealed class InvoiceProDbContext : DbContext
{
    public InvoiceProDbContext(DbContextOptions<InvoiceProDbContext> options) : base(options)
    {
    }

    public DbSet<Business> Businesses => Set<Business>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<InvoiceLineItem> InvoiceLineItems => Set<InvoiceLineItem>();
    public DbSet<PaymentRecord> Payments => Set<PaymentRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Business>().HasKey(x => x.Id);
        modelBuilder.Entity<Customer>().HasKey(x => x.Id);
        modelBuilder.Entity<Invoice>().HasKey(x => x.Id);
        modelBuilder.Entity<InvoiceLineItem>().HasKey(x => x.Id);
        modelBuilder.Entity<PaymentRecord>().HasKey(x => x.Id);

        modelBuilder.Entity<Invoice>()
            .HasMany(x => x.Items)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Invoice>()
            .HasMany(x => x.Payments)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
    }
}
