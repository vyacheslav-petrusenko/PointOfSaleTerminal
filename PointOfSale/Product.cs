using System;

namespace PointOfSale
{
    public class Product
    {
        public Product(string productCode, decimal price)
        {
            if (string.IsNullOrEmpty(productCode))
            {
                throw new ArgumentException("ProductCode should not be empty.", nameof(productCode));
            }

            if (price <= decimal.Zero)
            {
                throw new ArgumentException("Price should be greater than zero.", nameof(price));
            }

            ProductCode = productCode;
            Price = price;
        }

        public string ProductCode { get; }

        public decimal Price { get; }

        public override bool Equals(object obj)
        {
            return obj is Product product &&
                   ProductCode == product.ProductCode &&
                   Price == product.Price;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ProductCode, Price);
        }
    }
}
