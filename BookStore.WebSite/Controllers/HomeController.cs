using Microsoft.AspNetCore.Mvc;
using BookStore.WebSite.Models;
using BookStore.Entities;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace BookStore.WebSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            ViewBag.Categories = GetCategories();
            HomePageViewModel model = new HomePageViewModel();
            model.SizinIcinSectiklerimiz = GetBooks(false);
            model.CokSatanlar = GetBooks(true);
            return View(model);
        }

        private List<Category> GetCategories()
        {
            List<Category> categories = new List<Category>();
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("BookStoreContext")))
            {
                string sqlCommand = "SELECT * FROM dbo.Categories ORDER BY Name";
                SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand, connection);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    categories.Add(new Category
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        Name = row["Name"].ToString()
                    });
                }
            }

            return categories;
        }

        private List<Book> GetBooks(bool isBestSeller)
        {
            List<Book> books = new List<Book>();
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("BookStoreContext")))
            {
                string sqlCommand = "SELECT TOP 4 * FROM dbo.Books WHERE IsSelected = @IsBestSeller ORDER BY Name";
                if (isBestSeller)
                {
                    sqlCommand = "SELECT TOP 4 * FROM dbo.Books WHERE IsBestSeller = @IsBestSeller ORDER BY Name";
                }

                SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand, connection);
                adapter.SelectCommand.Parameters.AddWithValue("@IsBestSeller", isBestSeller);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    books.Add(new Book
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        CategoryId = Convert.ToInt32(row["CategoryId"]),
                        Description = row["Description"].ToString(),
                        ImageUrl = row["ImageUrl"].ToString(),
                        Name = row["Name"].ToString(),
                        PageCount = Convert.ToInt32(row["PageCount"]),
                        Price = Convert.ToDouble(row["Price"]),
                        PublishDate = Convert.ToDateTime(row["PublishDate"]),
                        WriterId = Convert.ToInt32(row["WriterId"]),
                        IsSelected = Convert.ToBoolean(row["IsSelected"]),
                        IsBestSeller = Convert.ToBoolean(row["IsBestSeller"])
                    });
                }
            }

            return books;
        }
    }
}
