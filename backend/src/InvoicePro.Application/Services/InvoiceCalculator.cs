using InvoicePro.Domain.Models;

namespace InvoicePro.Application.Services;

public sealed record InvoiceTotals(
    decimal Subtotal,
    decimal DiscountTotal,
    decimal TaxTotal,
    decimal Total,
    decimal AmountPaid,
    decimal OutstandingBalance);

public sealed class InvoiceCalculator
{
    public InvoiceTotals Calculate(Invoice invoice)
    {
        ArgumentNullException.ThrowIfNull(invoice);

        if (invoice.Items.Count == 0)
        {
            return new InvoiceTotals(0m, 0m, 0m, 0m, 0m, 0m);
        }

        var subtotal = 0m;
        var discountTotal = 0m;
        var taxTotal = 0m;

        foreach (var item in invoice.Items)
        {
            if (item.Quantity < 0)
                throw new InvalidOperationException("Invoice quantities cannot be negative.");

            if (item.UnitPrice < 0)
                throw new InvalidOperationException("Invoice prices cannot be negative.");

            if (item.DiscountPercent < 0 || item.DiscountPercent > 100)
                throw new InvalidOperationException("Discount percentages must be between 0 and 100.");

            if (item.TaxRate < 0 || item.TaxRate > 100)
                throw new InvalidOperationException("Tax rates must be between 0 and 100.");

            var grossAmount = item.Quantity * item.UnitPrice;
            var discountAmount = grossAmount * (item.DiscountPercent / 100m);
            var taxableBase = grossAmount - discountAmount;
            discountTotal += RoundMoney(discountAmount);

            var lineTax = item.TaxMode == TaxMode.Inclusive
                ? taxableBase - (taxableBase / (1m + (item.TaxRate / 100m)))
                : taxableBase * (item.TaxRate / 100m);

            var lineTotal = taxableBase + lineTax;
            subtotal += RoundMoney(lineTotal);
            taxTotal += RoundMoney(lineTax);
        }

        var invoiceDiscount = invoice.InvoiceDiscountPercent < 0 || invoice.InvoiceDiscountPercent > 100
            ? throw new InvalidOperationException("Invoice discount percentage must be between 0 and 100.")
            : invoice.InvoiceDiscountPercent;

        var invoiceLevelDiscount = subtotal * (invoiceDiscount / 100m);
        discountTotal += RoundMoney(invoiceLevelDiscount);

        var postDiscountSubtotal = subtotal - invoiceLevelDiscount;
        var total = RoundMoney(postDiscountSubtotal + taxTotal);
        var amountPaid = RoundMoney(invoice.Payments.Sum(p => p.Amount));
        var outstandingBalance = total > amountPaid ? RoundMoney(total - amountPaid) : 0m;

        return new InvoiceTotals(
            RoundMoney(subtotal),
            RoundMoney(discountTotal),
            RoundMoney(taxTotal),
            total,
            amountPaid,
            outstandingBalance);
    }

    private static decimal RoundMoney(decimal value) => Math.Round(value, 2, MidpointRounding.AwayFromZero);
}
