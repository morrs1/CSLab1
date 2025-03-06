namespace CSLabs.Models;

public class CartItem
{
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public int Price { get; set; }
    public int Total => Price * Quantity;
}