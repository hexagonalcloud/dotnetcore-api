using System;
using System.Linq;
using Core.Parameters;
using FluentAssertions;
using SqlAdventure.Services;
using Xunit;

namespace UnitTests.SqlAdventure
{
    public class SqlClauseServiceTests
    {
        [Fact]
        public void CreateWhereClause_Without_ColumnName_Throws_ArgumentNullException()
        {
            var service = new SqlClauseService();
            Assert.Throws<ArgumentNullException>(() => service.CreateWhereClause(null, ""));
            Assert.Throws<ArgumentNullException>(() => service.CreateWhereClause(string.Empty, ""));
        }

        [Fact]
        public void CreateWhereClause_Accepts_Null_Color_Parameter()
        {
            var service = new SqlClauseService();
            var querParams = new ProductQueryParameters();
            querParams.Color = null;

            var whereClause = service.CreateWhereClause("Color", querParams.Color);
            whereClause.Should().NotBeNull();
            whereClause.predicate.Should().Be(string.Empty);
        }

        [Fact]
        public void CreateWhereClause_Accepts_Empty_Color_Parameter()
        {
            var service = new SqlClauseService();
            var querParams = new ProductQueryParameters();
            querParams.Color = string.Empty;

            var whereClause = service.CreateWhereClause("Color", querParams.Color);

            whereClause.Should().NotBeNull();
            whereClause.predicate.Should().Be(string.Empty);
        }

        [Fact]
        public void CreateWhereClause_Accepts_Single_Color_Parameter()
        {
            var service = new SqlClauseService();
            var querParams = new ProductQueryParameters();
            querParams.Color = "red";

            var whereClause = service.CreateWhereClause("Color", querParams.Color);

            whereClause.Should().NotBeNull();
            whereClause.predicate.Should().Be("Color==@0");
            whereClause.parameters.Length.Should().Be(1);
            whereClause.parameters.First().Should().Be("red");
        }

        [Fact]
        public void CreateWhereClause_Accepts_Multiple_Color_Parameters()
        {
            var service = new SqlClauseService();
            var querParams = new ProductQueryParameters();
            querParams.Color = "red, blue";

            var whereClause = service.CreateWhereClause("Color", querParams.Color);

            whereClause.Should().NotBeNull();
            whereClause.predicate.Should().Be("Color==@0 OR Color==@1");
            whereClause.parameters.Length.Should().Be(2);
            whereClause.parameters.First().Should().Be("red");
            var blue = whereClause.parameters[1];
            blue.Should().NotBeNull();
            blue.ToString().Should().Be("blue");
        }

        [Fact]
        public void CreateOrderClause_Accepts_Null_Parameter()
        {
            var service = new SqlClauseService();
            var result = service.CreateOrderClause(null);
            result.Should().Be("Name");
        }

        [Fact]
        public void CreateOrderClause_Accepts_Empty_Parameter()
        {
            var service = new SqlClauseService();
            var result = service.CreateOrderClause(string.Empty);
            result.Should().Be("Name");
        }

        [Fact]
        public void CreateOrderClause_Accepts_Valid_Parameters()
        {
            var service = new SqlClauseService();
            var result = service.CreateOrderClause("Name");
            result.Should().Be("Name");

            result = service.CreateOrderClause("Name desc, color");
            result.Should().Be("Name desc, color");
        }

        [Fact]
        public void CreateOrderClause_Accepts_Invalid_Parameters()
        {
            var service = new SqlClauseService();
            var result = service.CreateOrderClause("Colors, Name dec");
            result.Should().Be("Name");

            result = service.CreateOrderClause("Color desc, names");
            result.Should().Be("Color desc");
        }

    }
}
