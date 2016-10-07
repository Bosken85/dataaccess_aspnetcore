using Digipolis.DataAccess.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Digipolis.DataAccess.UnitTests.Query
{
    public class OrderExtensionsTests
    {
        [Fact]
        private void QuerySuccessWhenNoOrderByGiven()
        {
            var query = new List<Tuple<int>> { new Tuple<int>(1), new Tuple<int>(2)}.AsQueryable();
            var orderedQuery = query.OrderByQuery();
            orderedQuery = orderedQuery.ThenByDescending(x => x.Item1);
            var items = orderedQuery.ToList();

            Assert.Equal(1, items[1].Item1);
            Assert.Equal(2, items[0].Item1);
        }

        [Fact]
        private void QuerySuccessWhenOrderByGiven()
        {
            var query = new List<Tuple<int, string, string>> {
                new Tuple<int, string, string>(1, "A", "Test 1"),
                new Tuple<int, string, string>(1, "B", "Test 2")
            }.AsQueryable();

            var orderedQuery = query.OrderByQuery("Item1");
            orderedQuery = orderedQuery.ThenByDescending(x => x.Item2);
            var items = orderedQuery.ToList();

            Assert.Equal("Test 2", items[0].Item3);
            Assert.Equal("Test 1", items[1].Item3);
        }

        [Fact]
        private void ExceptionOnUnknownField()
        {
            var query = new List<Tuple<int, string, string>> {
                new Tuple<int, string, string>(1, "A", "Test 1"),
                new Tuple<int, string, string>(1, "B", "Test 2")
            }.AsQueryable();

            Assert.ThrowsAny<ArgumentException>(() => query.OrderByQuery("NoField"));
        }
    }
}
