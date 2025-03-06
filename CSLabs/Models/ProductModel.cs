namespace CSLabs.Models;

public class ProductModel(string productName, int price, string description)
{
    public string Name { get; set; } = productName;
    public int Price { get; set; } = price;
    public string Description { get; set; } = description;
}