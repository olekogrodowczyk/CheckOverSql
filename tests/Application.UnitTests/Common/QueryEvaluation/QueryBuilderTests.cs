using Xunit;
using Application.Common.QueryEvaluation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace Application.Common.QueryEvaluation.Tests
{
    public class QueryBuilderTests
    {
        private readonly QueryBuilder _sut;

        public QueryBuilderTests()
        {
            _sut = new QueryBuilder();
        }

        [Fact]
        public void InitTest_ForSpecifiedQuery_ReturnsTrimmedQuery()
        {
            _sut.SetInitQuery("  SELECT * FROM Orders  ");
            _sut.GetResult().Should().Be("SELECT * FROM Orders");
        }

        [Fact]
        public void HandleSpacesTest_ForSpecifiedQuery_ReturnsTheQueryWithoutMultipleSpaces()
        {
            _sut.SetInitQuery("  SELECT   *   FROM Orders  ").HandleSpaces();
            _sut.GetResult().Should().Be("SELECT * FROM Orders");
        }

        [Fact]
        public void AddCountTest_ForSpecifiedQuery_ReturnsValidQueryWithSubquery()
        {
            _sut.SetInitQuery("SELECT *  FROM Orders  ").HandleSpaces().AddCount();
            _sut.GetResult().Should().Be("SELECT COUNT(*) AS COUNT FROM (SELECT * FROM Orders) QUERY");
        }

        [Fact]
        public void CheckOrderByTest_ForQueryWithOrderBy_ReturnsValidQuery()
        {
            _sut.SetInitQuery("SELECT * FROM Orders  ORDER BY  Id").HandleSpaces().CheckOrderBy();
            _sut.GetResult().Should().Be("SELECT * FROM Orders ORDER BY Id OFFSET 0 ROW");
        }

        [Fact]
        public void CheckOrderByTest_ForQueryWithoutOrderBy_ReturnsTheSameQuery()
        {
            _sut.SetInitQuery("SELECT * FROM Orders");
            _sut.GetResult().Should().Be("SELECT * FROM Orders");
        }

        [Fact]
        public void GetSpecificRowTest_ForSpecifiedQuery_ReturnsValidQueryWithSubquery()
        {
            _sut.SetInitQuery("SELECT * FROM Orders").HandleSpaces().GetSpecificRow(5);
            _sut.GetResult().Should().Be("SELECT * FROM(SELECT *, ROW_NUMBER() OVER(ORDER BY(SELECT NULL)) AS RowNumber" +
                $" FROM (SELECT * FROM Orders) QUERY ) AS myTable WHERE RowNumber = 5");
        }

        [Fact]
        public void AddRowNumberColumnTest_ForSpecifiedQuery_ReturnsValidQueryWithSubquery()
        {
            _sut.SetInitQuery("SELECT * FROM   Orders").HandleSpaces().AddRowNumberColumn();
            _sut.GetResult().Should().Be("SELECT * FROM(SELECT *, ROW_NUMBER() OVER(ORDER BY(SELECT NULL)) AS RowNumber" +
                $" FROM (SELECT * FROM Orders) QUERY ) AS MyTable");
        }
    }
}