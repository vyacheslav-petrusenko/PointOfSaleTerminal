using System;
using System.Collections.Generic;
using System.Linq;

namespace PointOfSale
{
    public class PointOfSaleTerminal
    {
        private Dictionary<string, Product> _products;
        private IEnumerable<Offer> _offers;
        private Dictionary<Product, int> _cheque = new Dictionary<Product, int>();

        public void SetPricing(IEnumerable<Product> products, IEnumerable<Offer> offers)
        {
            if (products == null || !products.Any())
            {
                throw new ArgumentException("There should be atleast 1 product for terminal to operate.", nameof(products));
            }

            _products = products.ToDictionary(p => p.ProductCode);
            _offers = offers ?? Enumerable.Empty<Offer>();
        }

        public void Scan(string productCode)
        {
            _products.TryGetValue(productCode, out var product);

            if (product == null)
            {
                // Log that unavailable product was scanned
                return;
            }

            _cheque.TryGetValue(product, out var currentCount);
            _cheque[product] = currentCount + 1;
        }

        public decimal CalculateTotal()
        {
            var total = 0m;

            foreach (KeyValuePair<Product, int> entry in _cheque)
            {
                var product = entry.Key;
                var amount = entry.Value;
                var offer = _offers.FirstOrDefault(o => o.ProductCode == product.ProductCode);

                if (offer != null)
                {
                    var offersCount = amount / offer.Amount;
                    var leftOverCount = amount % offer.Amount;

                    total += offersCount * offer.Price;
                    total += product.Price * leftOverCount;
                } else
                {
                    total += product.Price * amount;
                }
            }

            return total;
        }
    }
}
