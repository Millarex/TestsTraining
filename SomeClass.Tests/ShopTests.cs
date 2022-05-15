using FluentAssertions;
using Moq;
using NUnit.Framework;
using SomeClass.Data;
using System;

namespace SomeClass.Tests
{
    [TestFixture]
    public class ShopTests
    {
        private Item _item;
        private User _user;
        private Order _order;
        [SetUp]
        public void Setup()
        {
            _item = new Item() { Id = 0, Name = "test", Price = 10, Description = "alal" };
            _user = new User() { Name = "Alex", Adress = "some adress", Email = "someemail@mail.com" };
            _order = new Order();
        }

        [Test]
        [Category("AddItem_Tests")]
        public void AddItemToPrice_GoodData_ReturnTrue()
        {
            //Arrange
            var stockServiceMock = new Mock<IStock>();
            var emailServiceMock = new Mock<IEmail>();
            var bankServiceMock = new Mock<IBank>();
            var ordersServiceMock = new Mock<IOrders>();
            var priceServiceStub = new Mock<IPrice>();

            priceServiceStub.Setup(opt =>
                opt.GetItemById(It.IsAny<int>())).Returns(new Item());
            priceServiceStub.Setup(opt =>
                opt.AddItem(It.IsAny<Item>())).Returns(new Result() { Success = true });

            var sut = new Shop(stockServiceMock.Object, emailServiceMock.Object,
                bankServiceMock.Object, priceServiceStub.Object, ordersServiceMock.Object);
            var fakeItem = _item;
            //Act
            var result = sut.AddItemToPrice(fakeItem);
            //Assert
            //Assert.That(result.Success, Is.EqualTo(true));  - variant
            //Assert.IsTrue(result.Success); - variant
            result.Success.Should().BeTrue();
        }
        [Test]
        [Category("AddItem_Tests")]
        public void AddItemToPrice_BadData_ReturnFalse()
        {
            //Arrange
            var stockServiceMock = new Mock<IStock>();
            var emailServiceMock = new Mock<IEmail>();
            var bankServiceMock = new Mock<IBank>();
            var ordersServiceMock = new Mock<IOrders>();
            var priceServiceStub = new Mock<IPrice>();
            int badId = -2;

            priceServiceStub.Setup(opt =>
                opt.GetItemById(It.IsAny<int>())).Returns(new Item());
            priceServiceStub.Setup(opt =>
                opt.AddItem(It.IsAny<Item>())).Returns(new Result() { Success = true });

            var sut = new Shop(stockServiceMock.Object, emailServiceMock.Object,
                bankServiceMock.Object, priceServiceStub.Object, ordersServiceMock.Object);
            var fakeItem = _item;
            _item.Id = badId;
            //Act
            var result = sut.AddItemToPrice(fakeItem);
            //Assert
            //Assert.That(result.Success, Is.EqualTo(false));  - variant
            //Assert.IsFalse(result.Success); - variant
            result.Success.Should().BeFalse();
        }

        [Test]
        [Category("SetStock_Tests")]
        public void SetStockBalance_GoodData_ReturnTrue()
        {
            //Arrange
            var stockServiceStub = new Mock<IStock>();
            var emailServiceMock = new Mock<IEmail>();
            var bankServiceMock = new Mock<IBank>();
            var ordersServiceMock = new Mock<IOrders>();
            var priceServiceStub = new Mock<IPrice>();
            int goodBalance = 10;

            priceServiceStub.Setup(opt =>
                opt.GetItemById(It.IsAny<int>())).Returns(new Item());
            stockServiceStub.Setup(opt =>
                opt.SetItemBalance(It.IsAny<Item>(), It.IsAny<int>())).Returns
                (new Result() { Success = true });

            var sut = new Shop(stockServiceStub.Object, emailServiceMock.Object,
                bankServiceMock.Object, priceServiceStub.Object, ordersServiceMock.Object);
            var fakeItem = _item;
            //Act
            var result = sut.SetStockBalance(fakeItem, goodBalance);
            //Assert
            //Assert.That(result.Success, Is.EqualTo(true));  - variant
            //Assert.IsTrue(result.Success); - variant
            result.Success.Should().BeTrue();
        }
        [Test]
        [Category("SetStock_Tests")]
        public void SetStockBalance_BadData_ReturnFalse()
        {
            //Arrange
            var stockServiceStub = new Mock<IStock>();
            var emailServiceMock = new Mock<IEmail>();
            var bankServiceMock = new Mock<IBank>();
            var ordersServiceMock = new Mock<IOrders>();
            var priceServiceStub = new Mock<IPrice>();
            int badBalance = -2;

            priceServiceStub.Setup(opt =>
               opt.GetItemById(It.IsAny<int>())).Returns(new Item());
            stockServiceStub.Setup(opt =>
                opt.SetItemBalance(It.IsAny<Item>(), It.IsAny<int>())).Returns
                (new Result() { Success = true });

            var sut = new Shop(stockServiceStub.Object, emailServiceMock.Object,
                bankServiceMock.Object, priceServiceStub.Object, ordersServiceMock.Object);
            var fakeItem = _item;
            //Act
            var result = sut.SetStockBalance(fakeItem, badBalance);
            //Assert
            //Assert.That(result.Success, Is.EqualTo(false));  - variant
            //Assert.IsFalse(result.Success); - variant
            result.Success.Should().BeFalse();
        }
        [Test]
        [Category("BuyItems_Tests")]
        public void BuyItems_GoodData_ReturnTrue()
        {
            //Arrange
            var stockServiceStub = new Mock<IStock>();
            var emailServiceMock = new Mock<IEmail>();
            var bankServiceStub = new Mock<IBank>();
            var ordersServiceStub = new Mock<IOrders>();
            var priceServiceStub = new Mock<IPrice>();
            int itemCount = 10;
            int stockItemCount = 10;
            int userMoney = 1000;

            priceServiceStub.Setup(opt =>
               opt.GetItemById(It.IsAny<int>())).Returns(new Item());

            stockServiceStub.Setup(opt =>
                opt.GetItemBalance(It.IsAny<Item>())).Returns
                (stockItemCount);
            stockServiceStub.Setup(opt =>
                opt.ShipOrder(It.IsAny<Order>())).Returns
                (new Result() { Success = true });

            ordersServiceStub.Setup(opt =>
                opt.CreateOrder(_user, It.IsAny<Item>(), It.IsAny<int>()))
                .Returns(_order);

            bankServiceStub.Setup(opt =>
                opt.GetBalance(_user)).Returns
                (userMoney);
            bankServiceStub.Setup(opt =>
                opt.DebitMoney(_user, It.IsAny<int>())).Returns
                (new Result() { Success = true });

            var sut = new Shop(stockServiceStub.Object, emailServiceMock.Object,
                bankServiceStub.Object, priceServiceStub.Object, ordersServiceStub.Object);
            //Act
            var result = sut.BuyItems(_user, _item, itemCount);
            //Assert
            result.Success.Should().BeTrue();
        }
        [Test]
        [Category("BuyItems_Tests")]
        public void BuyItems_BadData_ReturnFalse([Values(-2, 5, 0, 1)] int itemCount,
           [Values(5, 4, 4, 5)] int stockItemCount,
           [Values(10, 10, 10, 5)] int userMoney)
        {
            //Arrange
            var stockServiceStub = new Mock<IStock>();
            var emailServiceMock = new Mock<IEmail>();
            var bankServiceStub = new Mock<IBank>();
            var ordersServiceStub = new Mock<IOrders>();
            var priceServiceStub = new Mock<IPrice>();

            priceServiceStub.Setup(opt =>
               opt.GetItemById(It.IsAny<int>())).Returns(new Item());

            stockServiceStub.Setup(opt =>
                opt.GetItemBalance(It.IsAny<Item>())).Returns
                (stockItemCount);
            stockServiceStub.Setup(opt =>
                opt.ShipOrder(It.IsAny<Order>())).Returns
                (new Result() { Success = true });

            ordersServiceStub.Setup(opt =>
                opt.CreateOrder(_user, It.IsAny<Item>(), It.IsAny<int>()))
                .Returns(_order);

            bankServiceStub.Setup(opt =>
                opt.GetBalance(_user)).Returns
                (userMoney);
            bankServiceStub.Setup(opt =>
                opt.DebitMoney(_user, It.IsAny<int>())).Returns
                (new Result() { Success = true });

            var sut = new Shop(stockServiceStub.Object, emailServiceMock.Object,
                bankServiceStub.Object, priceServiceStub.Object, ordersServiceStub.Object);
            //Act
            var result = sut.BuyItems(_user, _item, itemCount);
            //Assert       
            result.Success.Should().BeFalse();
        }
        [Test]
        [Category("BuyItems_Tests")]
        public void BuyItems_GoodData_EmailSend()
        {
            //Arrange
            var stockServiceStub = new Mock<IStock>();
            var emailServiceMock = new Mock<IEmail>();
            var bankServiceStub = new Mock<IBank>();
            var ordersServiceStub = new Mock<IOrders>();
            var priceServiceStub = new Mock<IPrice>();
            int itemCount = 10;
            int stockItemCount = 10;
            int userMoney = 1000;

            priceServiceStub.Setup(opt =>
               opt.GetItemById(It.IsAny<int>())).Returns(new Item());

            stockServiceStub.Setup(opt =>
                opt.GetItemBalance(It.IsAny<Item>())).Returns
                (stockItemCount);
            stockServiceStub.Setup(opt =>
                opt.ShipOrder(It.IsAny<Order>())).Returns
                (new Result() { Success = true });

            ordersServiceStub.Setup(opt =>
                opt.CreateOrder(_user, It.IsAny<Item>(), It.IsAny<int>()))
                .Returns(_order);

            bankServiceStub.Setup(opt =>
                opt.GetBalance(_user)).Returns
                (userMoney);
            bankServiceStub.Setup(opt =>
                opt.DebitMoney(_user, It.IsAny<int>())).Returns
                (new Result() { Success = true });

            var sut = new Shop(stockServiceStub.Object, emailServiceMock.Object,
                bankServiceStub.Object, priceServiceStub.Object, ordersServiceStub.Object);
            //Act
            _ = sut.BuyItems(_user, _item, itemCount);
            //Assert
            emailServiceMock.Verify(
             opt => opt.SendMessage(_user),
             Times.Once);
        }

    }
}
