using SomeClass.Data;

namespace SomeClass
{
    public class Shop
    {
        private readonly IStock _stock;
        private readonly IEmail _email;
        private readonly IBank _bank;
        private readonly IPrice _price;
        private readonly IOrders _orders;
        public Shop(IStock stock, IEmail email,
            IBank bank, IPrice price, IOrders orders)
        {
            _stock = stock;
            _email = email;
            _bank = bank;
            _price = price;
            _orders = orders;
        }

        public Result AddItemToPrice(Item item)
        {
            if (item == null)
                return new Result() { Success = false, Message = "item is null" };
            if (item.Id < 0)
                return new Result() { Success = false, Message = "wrong id" };
            if (_price.GetItemById(item.Id) == null)
                return new Result() { Success = false, Message = "id is busy" };

            if (_price.AddItem(item).Success)
                return new Result() { Success = true };
            else
                return new Result() { Success = false, Message = "Smth wrong with price" };
        }
        public Result SetStockBalance(Item item, int balance)
        {
            if (item == null || balance < 0)
                return new Result() { Success = false, Message = "wrong entry data" };
            if (_price.GetItemById(item.Id) == null)
                return new Result() { Success = false, Message = "item not present in price" };

            if (_stock.SetItemBalance(item, balance).Success)
                return new Result() { Success = true };
            else
                return new Result() { Success = false, Message = "Smth wrong with stock" };
        }

        public Result BuyItems(User user, Item item, int count)
        {
            if (item == null || count <= 0)
                return new Result() { Success = false, Message = "wrong entry data" };
            if (_price.GetItemById(item.Id) == null)
                return new Result() { Success = false, Message = "item not present in price" };
            if (_stock.GetItemBalance(item) < count)
                return new Result() { Success = false, Message = "Not enough items" };
            if (_bank.GetBalance(user) <= item.Price * count)
                return new Result() { Success = false, Message = "Not enough money" };

            var userOrder = _orders.CreateOrder(user, item, count);
            if (!_bank.DebitMoney(user, item.Price * count).Success)
                return new Result() { Success = false, Message = "Smth wrong with bank " };
            if (!_stock.ShipOrder(userOrder).Success)
                return new Result() { Success = false, Message = "Smth wrong with stock" };

            _email.SendMessage(user);

            return new Result() { Success = true };
        }
    }


}
