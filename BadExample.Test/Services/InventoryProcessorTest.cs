using BadExample.Service.Interfaces;
using BadExample.Service.Models;
using BadExample.Service.Services;
using NSubstitute;
using Xunit;

namespace BadExample.Test.Services
{
    public class InventoryProcessorTest
    {
        private IInventoryProcessor _target;

        [Fact]
        public void MakeInventoryItemReturnsCorrectInventoryItem()
        {
            var mockDatabaseAccessor = Substitute.For<IDatabaseAccessor>();
            _target = new InventoryProcessor(mockDatabaseAccessor);
            var line = "1, Hello, Two, 3, 45.00";
            var result = _target.MakeInventoryItem(line);

            Assert.Equal(result.Id, 1);
            Assert.Equal(result.Name, "Hello");
            Assert.Equal(result.Type, "Two");
            Assert.Equal(result.Amount, 3);
            Assert.Equal(result.Cost, 45.00m);
        }
        
        [Fact]
        public void MakeInventoryItemReturnsCorrectInventoryItemWithBadId()
        {
            var mockDatabaseAccessor = Substitute.For<IDatabaseAccessor>();
            _target = new InventoryProcessor(mockDatabaseAccessor);
            var line = "No, Hello, Two, 3, 45.00";
            var result = _target.MakeInventoryItem(line);

            Assert.Equal(result.Id, -1);
            Assert.Equal(result.Name, "");
            Assert.Equal(result.Type, "");
            Assert.Equal(result.Amount, 0);
            Assert.Equal(result.Cost, 0m);
        }

        [Fact]
        public void MakeInventoryItemReturnsCorrectInventoryItemWithNonAlphaCharactersInName()
        {
            var mockDatabaseAccessor = Substitute.For<IDatabaseAccessor>();
            _target = new InventoryProcessor(mockDatabaseAccessor);
            var line = "1, Hello zoom, Two, 3, 45.00";
            var result = _target.MakeInventoryItem(line);

            Assert.Equal(result.Id, 1);
            Assert.Equal(result.Name, "Hello zoom");
            Assert.Equal(result.Type, "Two");
            Assert.Equal(result.Amount, 3);
            Assert.Equal(result.Cost, 45.00m);
        }

        [Fact]
        public void MakeInventoryItemReturnsCorrectInventoryItemWithBadCost()
        {
            var mockDatabaseAccessor = Substitute.For<IDatabaseAccessor>();
            _target = new InventoryProcessor(mockDatabaseAccessor);
            var line = "1, Hello zoom, Two, 3, moooooo";
            var result = _target.MakeInventoryItem(line);

            Assert.Equal(result.Id, -1);
            Assert.Equal(result.Name, "");
            Assert.Equal(result.Type, "");
            Assert.Equal(result.Amount, 0);
            Assert.Equal(result.Cost, 0m);
        }

        [Fact]
        public void ProcessLineItemCallsInsertInventoryWithGoodLine()
        {
            var mockDatabaseAccessor = Substitute.For<IDatabaseAccessor>();
            _target = new InventoryProcessor(mockDatabaseAccessor);
            var line = "1, Hello zoom, Two, 3, 45.00";
            _target.ProcessLineItem(line);
            mockDatabaseAccessor.Received(1).InsertInventory(Arg.Any<InventoryItem>());
        }

        [Fact]
        public void ProcessLineItemDoesNotCallInsertInventoryWithBadLine()
        {

            var mockDatabaseAccessor = Substitute.For<IDatabaseAccessor>();
            _target = new InventoryProcessor(mockDatabaseAccessor);
            var line = "1, Hello zoom, Two, 3, moooooo";
            _target.ProcessLineItem(line);
            mockDatabaseAccessor.DidNotReceive().InsertInventory(Arg.Any<InventoryItem>());
        }
    }
}
