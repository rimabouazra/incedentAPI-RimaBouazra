using incedentAPI_RimaBouazra.Classes;

namespace AppTests
{   [Trait("Category", "Unit")] 
    public class SumTests
    {
        [Fact]
        public void Sum_PositiveNumbers_ReturnsCorrectResult()
        {
            var math = new Mathematics();
            var result = math.Sum(5, 10);
            Assert.Equal(15, result);
        }

        [Fact]
        public void Sum_NegativeAndPositiveNumbers_ReturnsCorrectResult()
        {
            var math = new Mathematics();
            var result = math.Sum(-3, 7);
            Assert.Equal(4, result);
        }

        [Fact]
        public void Sum_NegativeNumbers_ReturnsCorrectResult()
        {
            var math = new Mathematics();
            var result = math.Sum(-6, -21);
            Assert.Equal(-27, result);
        }
    }
}
