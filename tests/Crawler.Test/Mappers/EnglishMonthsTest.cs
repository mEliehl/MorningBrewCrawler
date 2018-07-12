using System;
using Xunit;
using Crawler.Mappers;

namespace Crawler.Test.Mappers
{
    public class EnglishMonthsTest
    {
        [Fact]
        public void WithInvalidMonth_ThrowsException()
        {
            var month = "Decenuary";
            Assert.Throws<ArgumentException>(() => EnglishMonths.Map(month));
        }

        [Theory]
        [InlineData(EnglishMonths.January,1)]
        [InlineData(EnglishMonths.February,2)]
        [InlineData(EnglishMonths.March,3)]
        [InlineData(EnglishMonths.April,4)]
        [InlineData(EnglishMonths.May,5)]
        [InlineData(EnglishMonths.June,6)]
        [InlineData(EnglishMonths.July,7)]
        [InlineData(EnglishMonths.August,8)]
        [InlineData(EnglishMonths.September,9)]
        [InlineData(EnglishMonths.October,10)]
        [InlineData(EnglishMonths.November,11)]
        [InlineData(EnglishMonths.December,12)]
        public void WithValidMonth_ReturnExpectedInteger(string month, int expected)
        {
            var actual = EnglishMonths.Map(month);
            Assert.Equal(expected,actual);
        }
    }
}