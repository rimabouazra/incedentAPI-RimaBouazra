using incedentAPI_RimaBouazra.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppTests
{   [Trait("Category", "Unit")] 
    public class FactorialTests
    {
        [Fact]
        public void Factorial_PositiveInteger_ReturnsCorrectResult()
        {
            var mathematics = new Mathematics();
            var result = mathematics.Factorial(5);
            Assert.Equal(120, result);
        }
        [Fact]
        public void Factorial_Zero_ReturnsOne()
        {
            var mathematics = new Mathematics();
            var result = mathematics.Factorial(0);
            Assert.Equal(1, result);
        }
        [Fact]
        public void Factorial_One_ReturnsOne()
        {
            var mathematics = new Mathematics();
            var result = mathematics.Factorial(1);
            Assert.Equal(1, result);
        }
        [Fact]
        public void Factorial_NegativeInteger_ThrowsArgumentException()
        {
            var mathematics = new Mathematics();
            Assert.Throws<ArgumentException>(() => mathematics.Factorial(-3));
        }
    }
}
