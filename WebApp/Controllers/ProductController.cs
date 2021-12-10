using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class ProductController : Controller
    { 
        HttpClient _client;
        Uri _baseAddress;
        IConfiguration _configuration;

        public ProductController(IConfiguration configuration)
    {
            _configuration = configuration;
            _baseAddress = new Uri(_configuration["ApiAddress"]);
            _client = new HttpClient();
            _client.BaseAddress = _baseAddress;
    }
        public IActionResult Index()
        {
            IEnumerable<Product> model = new List<Product>();
            var response = _client.GetAsync(_baseAddress + "/product/getall").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                model = JsonSerializer.Deserialize<IEnumerable<Product>>(data);
            }
            return View(model);
        }

        private IEnumerable<Category> GetCategories()
        {
            IEnumerable<Category> model = new List<Category>();
            var response = _client.GetAsync(_baseAddress + "/category").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                model = JsonSerializer.Deserialize<IEnumerable<Category>>(data);
            }
            return model;
        }

            public IActionResult AddProduct()
        {
            ViewBag.Categories = GetCategories();
            return View();
        }

        [HttpPost]
        public IActionResult AddProduct(Product model)
        {
            ModelState.Remove("ProductId");
            if (ModelState.IsValid)
            {
                string strData = JsonSerializer.Serialize(model);
                StringContent content = new StringContent(strData, Encoding.UTF8, "application/json");
                var response = _client.PostAsync(_baseAddress + "/product/add", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            ViewBag.Categories = GetCategories();
            return View();
        }

        public IActionResult Edit(int id)
        {
            ViewBag.Categories = GetCategories();
            Product productModel = new Product();
            var response = _client.GetAsync(_baseAddress + "/product/get/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                productModel = JsonSerializer.Deserialize<Product>(content);
            }
            return View("AddProduct", productModel);
        }

        [HttpPost]
        public IActionResult Edit(Product model)
        {
            ModelState.Remove("ProductId");
            if (ModelState.IsValid)
            {
                string strData = JsonSerializer.Serialize(model);
                StringContent content = new StringContent(strData, Encoding.UTF8, "application/json");
                var response = _client.PutAsync(_baseAddress + "/product/update/" + model.ProductId, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            ViewBag.Categories = GetCategories();
            return View("AddProduct");
        }


        public IActionResult Delete(int id)
        {
            ViewBag.Categories = GetCategories();
            var response = _client.DeleteAsync(_baseAddress + "/product/delete/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

    }
}
