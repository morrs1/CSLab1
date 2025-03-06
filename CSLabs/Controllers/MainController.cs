using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using CSLabs.Models;

namespace CSLabs.Controllers;

public class MainController(ILogger<MainController> logger) : Controller
{
    private readonly ILogger<MainController> _logger = logger;

    private readonly List<ProductModel> _products =
    [
        new("Мат плата", 20000, "Мат плата Asus"),
        new("Процессор", 15000, "Intel Pentium"),
        new("Оперативная память", 10000, "Adata XPG"),
        new("Блок питания", 1000, "CSAS 1000W"),
        new("Водянка", 7000, "Кастомная водянка"),
        new("Видеокарта", 100000, "5090 AssRock")
    ];

    private const string CartSessionKey = "Cart";

    private Dictionary<string, int> cart
    {
        get => HttpContext.Session.Get<Dictionary<string, int>>(CartSessionKey) ??
               new Dictionary<string, int>();
        set => HttpContext.Session.Set(CartSessionKey, value);
    }

    public IActionResult Index()
    {
        return View(_products);
    }

    public IActionResult Cart()
    {
        var cartItems = new List<CartItem>();
        foreach (var item in cart)
        {
            var product = _products.FirstOrDefault(p => p.Name == item.Key);
            if (product != null)
            {
                cartItems.Add(new CartItem
                {
                    ProductName = product.Name,
                    Quantity = item.Value,
                    Price = product.Price
                });
            }
        }

        return View(cartItems);
    }

    [HttpPost]
    public IActionResult AddProduct(string productName)
    {
        var newCart = cart;

        if (!newCart.TryAdd(productName, 1))
            newCart[productName]++;

        cart = newCart;

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult IncreaseQuantity(string productName)
    {
        var newCart = cart;

        if (!newCart.TryGetValue(productName, out var value)) return RedirectToAction(nameof(Cart));
        newCart[productName] = ++value;
        cart = newCart;

        return RedirectToAction(nameof(Cart));
    }

    [HttpPost]
    public IActionResult DecreaseQuantity(string productName)
    {
        var newCart = cart;

        if (!newCart.TryGetValue(productName, out var value)) return RedirectToAction(nameof(Cart));
        newCart[productName] = --value;
        if (value <= 0)
        {
            newCart.Remove(productName);
        }

        cart = newCart;

        return RedirectToAction(nameof(Cart));
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}