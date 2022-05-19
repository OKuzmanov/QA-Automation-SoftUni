using NUnit.Framework;
using System;

namespace Summator.Tests
{
    public class SummatorTests
    {
      
        [OneTimeSetUp]
        public void setup()
        {
            Console.WriteLine("Test started: " + DateTime.Now);
        }

        [Test]
        [Category("Critical")]
        public void Test_SumEquals_TwoPossitiveNumbers()
        {
            int actual = Summator.Sum(new int[] { 1, 5, 10 });

            int expected = 16;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [Category("Critical")]
        public void Test_SumNotEquals_TwoPossitiveNumbers()
        {
            int actual = Summator.Sum(new int[] { 1, 1});

            int expected = 3;

            Assert.AreNotEqual(expected, actual);
        }

        [Test]
        public void Test_SumEquals_TwoNegativeNumbers()
        {
            int actual = Summator.Sum(new int[] { -1, -1 });

            int expected = -2;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test_SumEquals_EmptyArray()
        {
            int actual = Summator.Sum(new int[] {});

            int expected = 0;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test_SumEquals_BigValues()
        {
            int actual = Summator.Sum(new int[] { });

            int expected = 0;

            Assert.That(expected, Is.EqualTo(actual));
        }

        [Test]
        public void TestAssertions_ValueInRange()
        {
            int value = 85;

            Assert.That(value, Is.InRange(80, 100));
        }

        [Test]
        public void TestAssertions_ContainsString()
        {
            String value = "QAs are awsome!";

            Assert.That(value, Does.Contain("awsome"));
        }

        [Test]
        public void TestAssertions_DateMatchesRegex()
        {
            String date = "7/11/2022";

            Assert.That(date, Does.Match(@"^\d{1,2}/\d{1,2}/\d{4}$"));
        }

        [Test]
        public void TestAssertions_ThrowsException()
        {
            int[] nums = new int[] { 1, 2, 3 };

            Assert.That(() => { nums[10]++; }, Throws.InstanceOf<IndexOutOfRangeException>());
        }

        [OneTimeTearDown]
        public void tearDown()
        {
            Console.WriteLine("Tests ended: " + DateTime.Now);
        }

    }
}