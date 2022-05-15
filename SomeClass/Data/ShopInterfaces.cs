namespace SomeClass.Data
{
    public interface IStock
    {
        public int GetItemBalance(Item item);
        public Result SetItemBalance(Item item, int balance);
        public Result ShipOrder(Order order);

    }
    public interface IEmail
    {
        public Result SendMessage(User user);
    }
    public interface IBank
    {
        public int GetBalance(User user);
        public Result DebitMoney(User user, int money);
    }
    public interface IPrice
    {
        public Result AddItem(Item item);
        public Result DeleteItem(Item item);
        public Item GetItemById(int id);
        public Item GetItemByName(string name);

    }
    public interface IOrders
    {
        public Order CreateOrder(User user, Item item, int count);
        public string CheckOrder(User user);

    }
}
