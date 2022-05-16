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
        Mock<IStock> _stockServiceStub;
        Mock<IEmail> _emailServiceMock;
        Mock<IBank> _bankServiceStub;
        Mock<IOrders> _ordersServiceStub;
        Mock<IPrice> _priceServiceStub;
        [SetUp]
        public void Setup()
        {
            _item = new Item() { Id = 0, Name = "test", Price = 10, Description = "alal" };
            _user = new User() { Name = "Alex", Adress = "some adress", Email = "someemail@mail.com" };
            _order = new Order();
            _stockServiceStub = new Mock<IStock>();
            _emailServiceMock = new Mock<IEmail>();
            _bankServiceStub = new Mock<IBank>();
            _ordersServiceStub = new Mock<IOrders>();
            _priceServiceStub = new Mock<IPrice>();
        }
        private Shop getSut()
        {
            return new Shop(_stockServiceStub.Object, _emailServiceMock.Object,
                _bankServiceStub.Object, _priceServiceStub.Object,
                _ordersServiceStub.Object);
        }

        [Test]
        [Category("AddItem_Tests")]
        public void AddItemToPrice_GoodData_ReturnTrue()
        {
            //Arrange           
            _priceServiceStub.Setup(opt =>
                opt.GetItemById(_item.Id)).Returns(new Item());
            _priceServiceStub.Setup(opt =>
                opt.AddItem(_item)).Returns(new Result() { Success = true });

            var sut = getSut();

            //Act
            var result = sut.AddItemToPrice(_item);

            //Assert
            StringAssert.AreEqualIgnoringCase(result.Success.ToString(), "True");
        }
        [Test]
        [Category("AddItem_Tests")]
        public void AddItemToPrice_BadData_ReturnFalse()
        {
            //Arrange          
            int badId = -2;
            var fakeItem = _item;
            _item.Id = badId;

            _priceServiceStub.Setup(opt =>
                opt.GetItemById(_item.Id)).Returns(new Item());
            _priceServiceStub.Setup(opt =>
                opt.AddItem(fakeItem)).Returns(new Result() { Success = true });

            var sut = getSut();

            //Act
            var result = sut.AddItemToPrice(fakeItem);

            //Assert
            result.Success.Should().BeFalse();
        }

        [Test]
        [Category("SetStock_Tests")]
        public void SetStockBalance_GoodData_ReturnTrue()
        {
            //Arrange
            int goodBalance = 10;

            _priceServiceStub.Setup(opt =>
                opt.GetItemById(_item.Id)).Returns(new Item());
            _stockServiceStub.Setup(opt =>
                opt.SetItemBalance(_item, goodBalance)).Returns
                (new Result() { Success = true });

            var sut = getSut();

            //Act
            var result = sut.SetStockBalance(_item, goodBalance);

            //Assert
            result.Success.Should().BeTrue();
        }
        [Test]
        [Category("SetStock_Tests")]
        public void SetStockBalance_BadData_ReturnFalse()
        {
            //Arrange           
            int badBalance = -2;

            _priceServiceStub.Setup(opt =>
               opt.GetItemById(_item.Id)).Returns(new Item());
            _stockServiceStub.Setup(opt =>
                opt.SetItemBalance(_item, badBalance)).Returns
                (new Result() { Success = true });

            var sut = getSut();

            //Act
            var result = sut.SetStockBalance(_item, badBalance);

            //Assert
            result.Success.Should().BeFalse();
        }
        [Test]
        [Category("BuyItems_Tests")]
        public void BuyItems_GoodData_ReturnTrue()
        {
            //Arrange
            int itemCount = 10;
            int stockItemCount = 10;
            int userMoney = 1000;

            _priceServiceStub.Setup(opt =>
               opt.GetItemById(_item.Id)).Returns(_item);
            _stockServiceStub.Setup(opt =>
                opt.GetItemBalance(_item)).Returns
                (stockItemCount);
            _stockServiceStub.Setup(opt =>
                opt.ShipOrder(_order)).Returns
                (new Result() { Success = true });
            _ordersServiceStub.Setup(opt =>
                opt.CreateOrder(_user, _item, itemCount))
                .Returns(_order);
            _bankServiceStub.Setup(opt =>
                opt.GetBalance(_user)).Returns
                (userMoney);
            _bankServiceStub.Setup(opt =>
                opt.DebitMoney(_user, _item.Price * itemCount)).Returns
                (new Result() { Success = true });

            var sut = getSut();

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
            _priceServiceStub.Setup(opt =>
              opt.GetItemById(_item.Id)).Returns(_item);
            _stockServiceStub.Setup(opt =>
                opt.GetItemBalance(_item)).Returns
                (stockItemCount);
            _stockServiceStub.Setup(opt =>
                opt.ShipOrder(_order)).Returns
                (new Result() { Success = true });
            _ordersServiceStub.Setup(opt =>
                opt.CreateOrder(_user, _item, itemCount))
                .Returns(_order);
            _bankServiceStub.Setup(opt =>
                opt.GetBalance(_user)).Returns
                (userMoney);
            _bankServiceStub.Setup(opt =>
                opt.DebitMoney(_user, _item.Price * itemCount)).Returns
                (new Result() { Success = true });

            var sut = getSut();

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
            int itemCount = 10;
            int stockItemCount = 10;
            int userMoney = 1000;

            _priceServiceStub.Setup(opt =>
             opt.GetItemById(_item.Id)).Returns(_item);
            _stockServiceStub.Setup(opt =>
                opt.GetItemBalance(_item)).Returns
                (stockItemCount);
            _stockServiceStub.Setup(opt =>
                opt.ShipOrder(_order)).Returns
                (new Result() { Success = true });
            _ordersServiceStub.Setup(opt =>
                opt.CreateOrder(_user, _item, itemCount))
                .Returns(_order);
            _bankServiceStub.Setup(opt =>
                opt.GetBalance(_user)).Returns
                (userMoney);
            _bankServiceStub.Setup(opt =>
                opt.DebitMoney(_user, _item.Price * itemCount)).Returns
                (new Result() { Success = true });

            var sut = getSut();

            //Act
            _ = sut.BuyItems(_user, _item, itemCount);

            //Assert
            _emailServiceMock.Verify(
             opt => opt.SendMessage(_user),
             Times.Once);
        }

    }
}
