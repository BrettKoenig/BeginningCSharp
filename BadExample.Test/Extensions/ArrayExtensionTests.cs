using System.Collections.Generic;
using BadExample.Service.Extensions;
using Xunit;

namespace BadExample.Test.Extensions
{
    public class ArrayExtensionTests
    {
        [Fact]
        public void TestTrimValuesForArrayExtensionsCorrect()
        {
            string[] testValues = new string[3] {"                      extra front whitespace", "extra back whitespace            ", "                     extra front and back whitespace                "};
            testValues.TrimValues();
            Assert.Equal("extra front whitespace", testValues[0]);
            Assert.Equal("extra back whitespace", testValues[1]);
            Assert.Equal("extra front and back whitespace", testValues[2]);
        }

        [Fact]
        public void TestTrimValuesForNullArrayReturnsNullArray()
        {
            var listOfStrings = new List<string>();
            string[] arrayOfStrings = listOfStrings.ToArray();
            arrayOfStrings.TrimValues();
            Assert.True(arrayOfStrings == null || arrayOfStrings.Length == 0);
        }
    }
}
