using System;

namespace PointOfSale
{
    public class Offer
    {
        public Offer(string productCode, int amount, decimal price)
        {
            if (string.IsNullOrEmpty(productCode))
            {
                throw new ArgumentException("ProductCode should not be empty.", nameof(productCode));
            }

            if (amount <= 0)
            {
                throw new ArgumentException("Amount should be greater than zero.", nameof(amount));
            }

            if (price <= decimal.Zero)
            {
                throw new ArgumentException("Price should be greater than zero.", nameof(price));
            }

            ProductCode = productCode;
            Amount = amount;
            Price = price;
        }

        public string ProductCode { get; }

        public int Amount { get; }

        public decimal Price { get; }
    }
}
