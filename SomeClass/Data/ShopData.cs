namespace SomeClass.Data
{
    public class User
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Adress { get; set; }
    }
    public class Result
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
    }
    public class Order
    {
        public User user { get; set; }
        public Item item { get; set; }
        public int count { get; set; }
    }
}
