using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextFilter.Tests.UnitTests
{
    using NUnit.Framework;

    [TestFixture]
    public class FilterMethodsTests
    {
        [TestCase("clean", ExpectedResult = true)]
        [TestCase("what", ExpectedResult = true)]
        [TestCase("currently", ExpectedResult = true)]
        [TestCase("the", ExpectedResult = false)]
        [TestCase("rather", ExpectedResult = false)]
        public bool MiddleVowelFilter_ShouldCorrectlyIdentifyMiddleVowel(string word)
        {
            return FilterMethods.MiddleVowelFilter(word);
        }

        [TestCase("an", ExpectedResult = true)]
        [TestCase("at", ExpectedResult = true)]
        [TestCase("word", ExpectedResult = false)]
        [TestCase("another", ExpectedResult = false)]
        public bool LessThanThreeCharactersFilter_ShouldFilterOutShortWords(string word)
        {
            return FilterMethods.LessThanThreeCharactersFilter(word);
        }

        [TestCase("tree", ExpectedResult = true)]
        [TestCase("test", ExpectedResult = true)]
        [TestCase("apple", ExpectedResult = false)]
        [TestCase("banana", ExpectedResult = false)]
        public bool WordContainingTFilter_ShouldFilterOutWordsContainingT(string word)
        {
            return FilterMethods.WordContainingTFilter(word);
        }
    }

}
