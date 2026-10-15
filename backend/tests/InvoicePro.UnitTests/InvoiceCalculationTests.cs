using InvoicePro.Application.Services;
using InvoicePro.Domain.Models;
using Xunit;

namespace InvoicePro.UnitTests;

public class InvoiceCalculationTests
{
    [Fact]
    public void Calculate_ShouldApplyDiscountAndTaxForExclusiveLineItems()
    {
        var invoice = new Invoice
        {
            InvoiceNumber = "INV-1001",
            CustomerName = "Cobalt Studio",
            Items =
            [
                new InvoiceLineItem { Description = "Design services", Quantity = 2m, UnitPrice = 100m, DiscountPercent = 10m, TaxRate = 15m, TaxMode = TaxMode.Exclusive }
            ]
        };

        var result = new InvoiceCalculator().Calculate(invoice);

        Assert.Equal(180m, result.Subtotal);
        Assert.Equal(20m, result.DiscountTotal);
        Assert.Equal(27m, result.TaxTotal);
        Assert.Equal(207m, result.Total);
    }

    [Fact]
    public void Calculate_ShouldHonorPartialPayments()
    {
        var invoice = new Invoice
        {
            InvoiceNumber = "INV-1002",
            CustomerName = "Northwind Labs",
            Items =
            [
                new InvoiceLineItem { Description = "Consulting", Quantity = 1m, UnitPrice = 500m, TaxRate = 0m, TaxMode = TaxMode.Exclusive }
            ],
            Payments =
            [
                new PaymentRecord { Amount = 200m, Method = "Bank Transfer", Reference = "PAY-001" }
            ]
        };

        var result = new InvoiceCalculator().Calculate(invoice);

        Assert.Equal(500m, result.Total);
        Assert.Equal(200m, result.AmountPaid);
        Assert.Equal(300m, result.OutstandingBalance);
    }

    [Fact]
    public void Calculate_ShouldRejectNegativeDiscounts()
    {
        var invoice = new Invoice
        {
            InvoiceNumber = "INV-1003",
            Items =
            [
                new InvoiceLineItem { Description = "Bad line", Quantity = 1m, UnitPrice = 80m, DiscountPercent = -5m, TaxRate = 0m }
            ]
        };

        var sut = new InvoiceCalculator();

        Assert.Throws<InvalidOperationException>(() => sut.Calculate(invoice));
    }
}
