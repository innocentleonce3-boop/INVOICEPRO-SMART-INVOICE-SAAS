using InvoicePro.Application.Services;
using InvoicePro.Domain.Models;
using InvoicePro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<InvoiceProDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")
        ?? "Server=localhost,1433;Database=InvoicePro;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;"));

builder.Services.AddScoped<InvoiceCalculator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/healthz", () => Results.Ok(new { status = "ok" }));

app.MapGet("/api/v1/dashboard/summary", () => Results.Ok(new
{
    totalInvoiced = 128450.75m,
    amountPaid = 94210.25m,
    outstandingBalance = 34240.50m,
    overdueBalance = 14600.00m,
    draftCount = 4,
    paidCount = 28,
    collectionRate = 73.2m,
    currency = "USD"
}));

app.MapGet("/api/v1/invoices", () => Results.Ok(new
{
    items = new[]
    {
        new { id = Guid.NewGuid(), invoiceNumber = "INV-1001", status = "Sent", total = 2450.00m },
        new { id = Guid.NewGuid(), invoiceNumber = "INV-1002", status = "Paid", total = 1640.75m }
    },
    totalCount = 2
}));

app.MapPost("/api/v1/auth/register", (RegisterRequest request) =>
{
    if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
    {
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
            ["email"] = ["Email and password are required."]
        });
    }

    return Results.Ok(new { message = "Registration request accepted.", businessName = request.BusinessName, email = request.Email });
});

app.MapPost("/api/v1/invoices", (InvoiceDraft invoice, InvoiceCalculator calculator) =>
{
    if (invoice.Items.Count == 0)
    {
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
            ["items"] = ["At least one invoice line is required."]
        });
    }

    var totals = calculator.Calculate(invoice.ToDomain());
    return Results.Ok(new { invoiceNumber = invoice.InvoiceNumber, totals });
});

app.Run();

public sealed record RegisterRequest(string BusinessName, string Email, string Password);

public sealed class InvoiceDraft
{
    public string InvoiceNumber { get; set; } = "INV-1001";
    public string CustomerName { get; set; } = string.Empty;
    public List<InvoiceLineDraft> Items { get; set; } = new();
    public decimal InvoiceDiscountPercent { get; set; }

    public Invoice ToDomain() => new()
    {
        InvoiceNumber = InvoiceNumber,
        CustomerName = CustomerName,
        InvoiceDiscountPercent = InvoiceDiscountPercent,
        Items = Items.Select(x => new InvoicePro.Domain.Models.InvoiceLineItem
        {
            Description = x.Description,
            Quantity = x.Quantity,
            UnitPrice = x.UnitPrice,
            DiscountPercent = x.DiscountPercent,
            TaxRate = x.TaxRate,
            TaxMode = x.TaxMode
        }).ToList()
    };
}

public sealed class InvoiceLineDraft
{
    public string Description { get; set; } = string.Empty;
    public decimal Quantity { get; set; } = 1m;
    public decimal UnitPrice { get; set; } = 0m;
    public decimal DiscountPercent { get; set; }
    public decimal TaxRate { get; set; }
    public InvoicePro.Domain.Models.TaxMode TaxMode { get; set; } = InvoicePro.Domain.Models.TaxMode.Exclusive;
}
