using FluentAssertions;
using NUnit.Framework;
using PointOfSale;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests
{
    public class PointOfSaleTerminalTests
    {
        private PointOfSaleTerminal _sut;

        [SetUp]
        public void Setup()
        {
            var productList = new List<Product>
            {
                new Product("A", 1.25m),
                new Product("B", 4.25m),
                new Product("C", 1m),
                new Product("D", 0.75m),
            };
            var offerList = new List<Offer>
            {
                new Offer("A", 3, 3m),
                new Offer("C", 6, 5m)
            };

            _sut = new PointOfSaleTerminal();
            _sut.SetPricing(productList, offerList);
        }

        [Test]
        public void Calculate_should_return_correct_value_when_no_offers_are_found()
        {
            // Arrange
            _sut.Scan("A");
            _sut.Scan("B");
            _sut.Scan("C");
            _sut.Scan("D");

            // Act
            var result = _sut.CalculateTotal();

            // Assert
            result.Should().Be(7.25m);
        }

        [Test]
        public void Calculate_should_return_correct_value_when_an_offer_is_found()
        {
            // Arrange
            _sut.Scan("C");
            _sut.Scan("C");
            _sut.Scan("C");
            _sut.Scan("C");
            _sut.Scan("C");
            _sut.Scan("C");

            _sut.Scan("C");

            // Act
            var result = _sut.CalculateTotal();

            // Assert
            result.Should().Be(6m);
        }

        [Test]
        public void Calculate_should_return_correct_value_when_an_offer_is_found_and_products_are_scanned_in_random_order()
        {
            // Arrange
            _sut.Scan("A");
            _sut.Scan("B");
            _sut.Scan("C");
            _sut.Scan("D");
            _sut.Scan("A");
            _sut.Scan("B");
            _sut.Scan("A");

            // Act
            var result = _sut.CalculateTotal();

            // Assert
            result.Should().Be(13.25m);
        }

        [Test]
        public void Calculate_should_return_correct_value_when_offer_is_applicable_multiple_times_for_the_same_product()
        {
            // Arrange
            _sut.Scan("C");
            _sut.Scan("C");
            _sut.Scan("C");
            _sut.Scan("C");
            _sut.Scan("C");
            _sut.Scan("C");

            _sut.Scan("C");
            _sut.Scan("C");
            _sut.Scan("C");
            _sut.Scan("C");
            _sut.Scan("C");
            _sut.Scan("C");

            _sut.Scan("C");
            _sut.Scan("C");

            // Act
            var result = _sut.CalculateTotal();

            // Assert
            result.Should().Be(12m);
        }

        [Test]
        public void Calculate_should_return_correct_value_when_multiple_offers_for_different_products_are_found()
        {
            // Arrange
            _sut.Scan("C");
            _sut.Scan("C");
            _sut.Scan("C");
            _sut.Scan("C");
            _sut.Scan("C");
            _sut.Scan("C");

            _sut.Scan("A");
            _sut.Scan("A");
            _sut.Scan("A");

            _sut.Scan("B");
            _sut.Scan("D");

            // Act
            var result = _sut.CalculateTotal();

            // Assert
            result.Should().Be(13m);
        }

        [Test]
        public void Calculate_should_return_correct_value_when_unknown_product_was_scanned()
        {
            // Arrange
            _sut.Scan("A");
            _sut.Scan("B");
            _sut.Scan("AB");
            _sut.Scan("C");
            _sut.Scan("CD");
            _sut.Scan("D");

            // Act
            var result = _sut.CalculateTotal();

            // Assert
            result.Should().Be(7.25m);
        }

        [Test]
        public void Calculate_should_return_correct_value_when_no_offers_are_set()
        {
            // Arrange
            var productList = new List<Product>
            {
                new Product("C", 1m),
            };
            _sut.SetPricing(productList, null);

            _sut.Scan("C");
            _sut.Scan("C");
            _sut.Scan("C");
            _sut.Scan("C");
            _sut.Scan("C");
            _sut.Scan("C");

            // Act
            var result = _sut.CalculateTotal();

            // Assert
            result.Should().Be(6);
        }

        [TestCaseSource(nameof(InvalidProductLists))]
        public void SetPricing_should_throw_when_product_list_is_null_or_empty(IEnumerable<Product> productList)
        {
            // Act && Assert
            Action action = () => _sut.SetPricing(productList, null);
            action.Should().Throw<ArgumentException>().WithMessage("There should be atleast 1 product for terminal to operate. (Parameter 'products')");
        }

        [Test]
        public void SetPricing_should_throw_when_product_list_contains_propducts_with_same_product_code()
        {
            // Arrange
            var duplicateProductCode = "C";
            var productList = new List<Product>
            {
                new Product(duplicateProductCode, 1m),
                new Product(duplicateProductCode, 2m),
            };

            // Act && Assert
            Action action = () => _sut.SetPricing(productList, null);
            action.Should().Throw<ArgumentException>().WithMessage($"An item with the same key has already been added. Key: {duplicateProductCode}");
        }

        static IEnumerable<Product>[] InvalidProductLists =
        {
            null,
            Enumerable.Empty<Product>(),
        };
    }
}