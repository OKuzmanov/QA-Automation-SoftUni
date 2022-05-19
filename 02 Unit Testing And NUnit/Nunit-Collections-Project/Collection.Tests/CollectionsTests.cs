using System;
using System.Linq;
using NUnit.Framework;

namespace Collections.Tests
{
    public class CollectionsTests
    {
        private Collection<int> nums;

        [SetUp]
        public void init()
        {
            nums = new Collection<int>(new int[] { 10, 20, 30, 40, 50, 60, 70, 80 });
        }

        [Test]
        public void Test_Collections_EmptyConstructor()
        {
            nums = new Collection<int>();

            Assert.AreEqual(0, nums.Count);
            Assert.AreEqual(16, nums.Capacity);
            Assert.That(nums.ToString, Is.EqualTo("[]"));
        }

        [Test]
        public void Test_Collections_ConstructorSingleItem()
        {
            nums = new Collection<int>(10);

            Assert.AreEqual(1, nums.Count);
            Assert.That(nums.ToString, Is.EqualTo("[10]"));
        }

        [Test]
        public void Test_Collections_ConstructorMultipleItems()
        {
            nums = new Collection<int>(10, 20, 30, 40, 50, 60, 70, 80);

            Assert.AreEqual(8, nums.Count);
            Assert.That(nums.ToString, Is.EqualTo("[10, 20, 30, 40, 50, 60, 70, 80]"));
        }

        [Test]
        public void Test_Collections_Add()
        {
            Collection<int> nums = new Collection<int>(new int[] { 10, 20, 30, 40, 50, 60 });

            int numToAdd = 70;
            nums.Add(numToAdd);

            Assert.That(numToAdd, Is.EqualTo(nums[nums.Count - 1]));
            Assert.AreEqual(7, nums.Count);
            Assert.That(nums.ToString, Is.EqualTo("[10, 20, 30, 40, 50, 60, 70]"));
        }

        [Test]
        public void Test_Collections_AddRange()
        {
            int oldCapacity = nums.Capacity;

            nums.AddRange(new int[] { 10, 20, 30, 40, 50, 60, 70, 80 });

            Assert.AreEqual(16, nums.Count);
            Assert.That(oldCapacity, Is.EqualTo(nums.Capacity));
        }

        [Test]
        public void Test_Collections_AddRangeWithGrow()
        {
            int oldCapacity = nums.Capacity;

            nums.AddRange(new int[] { 10, 20, 30, 40, 50, 60, 70, 80, 90 });

            Assert.AreEqual(17, nums.Count);
            Assert.Greater(nums.Capacity, oldCapacity);
            Assert.Greater(nums.Capacity, nums.Count);
        }

        [Test]
        public void Test_Collections_InsertAtEnd()
        { 
            int numToAdd = 10;
            nums.InsertAt(nums.Count, numToAdd);

            Assert.AreEqual(9, nums.Count);
            Assert.That(nums[nums.Count - 1], Is.EqualTo(numToAdd));
        }

        [Test]
        public void Test_Collections_InsertAtStart()
        {
            int numToAdd = 10;
            nums.InsertAt(0, numToAdd);

            Assert.AreEqual(9, nums.Count);
            Assert.That(nums[0], Is.EqualTo(numToAdd));
        }

        [Test]
        public void Test_Collections_InsertAtMiddle()
        {
            Collection<int> nums = new Collection<int>(new int[] { 10, 20, 30, 40, 50, 60, 70});

            int numToAdd = 10;
            nums.InsertAt(3, numToAdd);

            Assert.AreEqual(8, nums.Count);
            Assert.That(nums[3], Is.EqualTo(numToAdd));
        }

        [Test]
        public void Test_Collections_InsertWithGrow()
        {
            int oldCapacity = nums.Capacity;

            nums.AddRange(new int[] { 10, 20, 30, 40, 50, 60, 70, 80 });

            int numToAdd = 10;
            nums.InsertAt(nums.Count, numToAdd);

            Assert.Greater(nums.Capacity, oldCapacity);
            Assert.AreEqual(17, nums.Count);
            Assert.That(nums[nums.Count-1], Is.EqualTo(numToAdd));
        }

        [Test]
        public void Test_Collections_InsertAtInvalidIndex()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => nums.InsertAt(-1, 10));
        }

        [Test]
        public void Test_Collections_ToString()
        {
            Assert.That(nums.ToString, Is.EqualTo("[10, 20, 30, 40, 50, 60, 70, 80]"));
        }

        [Test]
        public void Test_Collections_ToStringNestedCollections()
        {
            Collection<String> names = new Collection<String>("Bob", "John");
            Collection<int> age = new Collection<int>(10, 20);
            Collection<DateTime> dates = new Collection<DateTime>();

            Collection<Object> nestedArr = new Collection<object>(names, age, dates);

            Assert.That(nestedArr.ToString, Is.EqualTo("[[Bob, John], [10, 20], []]"));
        }

        [Test]
        public void Test_Collections_Clear()
        {
            nums.Clear();

            Assert.AreEqual(0, nums.Count);
            Assert.That(nums.ToString, Is.EqualTo("[]"));
        }

        [Test]
        public void Test_Collections_GetByIndex()
        {
            int numAtFirstIndex = nums[0];
            int numAtLastIndex = nums[nums.Count - 1];

            Assert.AreEqual(10, numAtFirstIndex);
            Assert.AreEqual(80, numAtLastIndex);
        }

        [Test]
        public void Test_Collections_GetByInvalidIndex()
        { 
            Assert.That(() => nums[-1], Throws.InstanceOf<ArgumentOutOfRangeException>());
            //There are 8 elements in the setup array, therefore 9 is an invalid index.
            Assert.That(() => nums[9], Throws.InstanceOf<ArgumentOutOfRangeException>());
            //No such index 500 in the collection.
            Assert.That(() => nums[500], Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Test_Collections_SetByIndex()
        {
            nums[1] = 100;

            Assert.AreEqual(100, nums[1]);
        }

        [Test]
        public void Test_Collections_SetByInvalidIndex()
        {
            nums[1] = 100;

            Assert.Throws<ArgumentOutOfRangeException>(() => nums[-1] = 100);
            //There are 8 elements in the setup array, therefore 9 is an invalid index.
            Assert.Throws<ArgumentOutOfRangeException>(() => nums[9] = 100);
        }

        [Test]
        public void Test_Collections_ExchangeFirstLast()
        {
            int numCount = nums.Count;

            int numFirstIndex = nums[0];
            int numLastIndex = nums[nums.Count - 1];

            nums.Exchange(0, nums.Count - 1);

            Assert.AreEqual(numCount, nums.Count);
            Assert.AreEqual(numFirstIndex, nums[nums.Count - 1]);
            Assert.AreEqual(numLastIndex, nums[0]);
        }

        [Test]
        public void Test_Collections_ExchangeMidleFirst()
        {
            int numCount = nums.Count;

            int numFirstIndex = nums[0];
            int numMiddleIndex = nums[nums.Count / 2];

            nums.Exchange(0, nums.Count / 2);

            Assert.AreEqual(numCount, nums.Count);
            Assert.AreEqual(numFirstIndex, nums[nums.Count / 2]);
            Assert.AreEqual(numMiddleIndex, nums[0]);
        }

        [Test]
        public void Test_Collections_ExchangeInvalidIndexes()
        {
            // nums have only 8 elements, therefore accessing the 9th index will throw an exception.
            Assert.Throws<ArgumentOutOfRangeException>(() => nums.Exchange(-1, 9));
        }

        [Test]
        public void Test_Collections_RemoveFirstIndex()
        {
            int oldCount = nums.Count;

            int removedNum = nums.RemoveAt(0);

            Assert.Greater(oldCount, nums.Count);
            Assert.AreEqual(10, removedNum);
        }

        [Test]
        public void Test_Collections_RemoveLastIndex()
        {
            int oldCount = nums.Count;

            int removedNum = nums.RemoveAt(nums.Count - 1);

            Assert.Greater(oldCount, nums.Count);
            Assert.AreEqual(80, removedNum);
        }

        [Test]
        public void Test_Collections_RemoveMidleIndex()
        {
            int oldCount = nums.Count;

            int removedNum = nums.RemoveAt(nums.Count / 2);

            Assert.Greater(oldCount, nums.Count);
            Assert.AreEqual(50, removedNum);
        }

        [Test]
        public void Test_Collections_RemoveInvalidIndex()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => nums.RemoveAt(-1));
            Assert.Throws<ArgumentOutOfRangeException>(() => nums.RemoveAt(9));
        }

        [Test]
        [Timeout(1000)]
        public void Test_Collection_1MillionItems()
        {
            const int itemsCount = 1000000;
            var nums = new Collection<int>();
            nums.AddRange(Enumerable.Range(1, itemsCount).ToArray());
            Assert.That(nums.Count == itemsCount);
            Assert.That(nums.Capacity >= nums.Count);
            for (int i = itemsCount - 1; i >= 0; i--)
                nums.RemoveAt(i);
            Assert.That(nums.ToString() == "[]");
            Assert.That(nums.Capacity >= nums.Count);
        }

    }
}