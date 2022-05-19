using NUnit.Framework;
using System;

namespace Collections.Tests
{
    public class DataDrivenTests
    {

        [TestCase("Vlad", 0, "Vlad")]
        [TestCase("Vlad, Anton, Ned", 0, "Vlad")]
        [TestCase("Vlad, Anton, Ned", 1, "Anton")]
        [TestCase("Vlad, Anton, Ned", 2, "Ned")]
        public void DataDrivenTest_GetByValidIndex(string data, int index, string expectedValue)
        {
            var collection = new Collection<string>(data.Split(", ", StringSplitOptions.RemoveEmptyEntries));

            Assert.AreEqual(expectedValue, collection[index]);
        }
        
        [TestCase("", 0)]
        [TestCase("Vlad", 1)]
        [TestCase("Vlad", -1)]
        [TestCase("Vlad, Anton, Ned", -1)]
        [TestCase("Vlad, Anton, Ned", 3)]
        [TestCase("Vlad, Anton, Ned", 100)]
        public void DataDrivenTest_GetByInvalidIndex(String data, int index)
        {
            var collection = new Collection<string>(data.Split(", ", StringSplitOptions.RemoveEmptyEntries));

            Assert.That(() => collection[index], Throws.TypeOf<ArgumentOutOfRangeException>());
        }

    }
}
