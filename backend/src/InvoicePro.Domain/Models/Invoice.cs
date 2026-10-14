namespace InvoicePro.Domain.Models;

public enum InvoiceStatus
{
    Draft,
    Sent,
    PartiallyPaid,
    Paid,
    Overdue,
    Cancelled
}

public enum TaxMode
{
    Exclusive,
    Inclusive
}

public sealed class Business
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Currency { get; set; } = "USD";
    public string Country { get; set; } = "US";
}

public sealed class Customer
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string Currency { get; set; } = "USD";
}

public sealed class InvoiceLineItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Description { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal DiscountPercent { get; set; }
    public decimal TaxRate { get; set; }
    public TaxMode TaxMode { get; set; } = TaxMode.Exclusive;
}

public sealed class PaymentRecord
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public decimal Amount { get; set; }
    public string Method { get; set; } = "Bank Transfer";
    public DateTimeOffset PaidOn { get; set; } = DateTimeOffset.UtcNow;
    public string Reference { get; set; } = string.Empty;
}

public sealed class Invoice
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string InvoiceNumber { get; set; } = string.Empty;
    public Guid BusinessId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public DateTimeOffset IssueDate { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset DueDate { get; set; } = DateTimeOffset.UtcNow.AddDays(14);
    public string Currency { get; set; } = "USD";
    public decimal InvoiceDiscountPercent { get; set; }
    public InvoiceStatus Status { get; set; } = InvoiceStatus.Draft;
    public string Notes { get; set; } = string.Empty;
    public List<InvoiceLineItem> Items { get; set; } = new();
    public List<PaymentRecord> Payments { get; set; } = new();
}
