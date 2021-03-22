using FirstAppDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FirstAppDemo.Controllers
{
    public class ProductController : Controller 
    {
        private string dbconnectionStr;

        [HttpPost]
       public IActionResult Product()
        {
            List productList = new List();
            var dbconfig = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();
            try
            {
                dbconnectionStr = dbconfig["ConnectionStrings:DefaultConnection"];
                string Sql = "SP_GET_ProductList";
                using (SqlConnection connection=new SqlConnection(dbconnectionStr))
                {
                    SqlCommand command = new SqlCommand(Sql, connection);
                    connection.Open();
                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            Product product = new Product();
                            product.Id = Convert.ToInt32(dataReader["Id"]);
                            product.ProductName = Convert.ToString(dataReader["ProductName"]);
                            product.ProductDescription = Convert.ToString(dataReader["ProductDescription"]);
                            product.ProductCost = Convert.ToDecimal(dataReader["ProductCost"]);
                            product.Stock = Convert.ToInt32(dataReader["Stock"]);
                            productList.Add(product);
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return View(productList);
        }
        public IActionResult ProductCreate()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ProductCreate(Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dbconfig = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json").Build();
                    if (!string.IsNullOrEmpty(dbconfig.ToString()))
                    {
                        dbconnectionStr = dbconfig["ConnectionStrings:DefaultConnection"];
                        using (SqlConnection connection=new SqlConnection(dbconnectionStr))
                        {
                            string sql = "SP_ADD_NEW_PRODUCT";
                            using (SqlCommand cmd=new SqlCommand(sql,connection))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                                cmd.Parameters.AddWithValue("@ProductDescription", product.ProductDescription);
                                cmd.Parameters.AddWithValue("@ProductCost", product.ProductCost);
                                cmd.Parameters.AddWithValue("@Stock", product.ProductCost);
                                connection.Open();
                                cmd.ExecuteNonQuery();
                                connection.Close();
                            
                            }
                        
                        }
                    }
                           
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return RedirectToAction("Product");
        }
        public IActionResult ProductUpdate(int id)
        {
            var dbconfig = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json").Build();
            dbconnectionStr = dbconfig["ConnectionStrings:DefaultConnection"];
            Product product = new Product();
            using (SqlConnection connection = new SqlConnection(dbconnectionStr))
            {
                string sql = "SP_Get_Product_By_Id";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    using (SqlDataReader dataReader = cmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            product.Id = Convert.ToInt32(dataReader["Id"]);
                            product.ProductName = Convert.ToString(dataReader["ProductName"]);
                            product.ProductDescription = Convert.ToString(dataReader["ProductDescription"]);
                            product.ProductCost = Convert.ToDecimal(dataReader["ProductCost"]);
                            product.Stock = Convert.ToInt32(dataReader["Stock"]);
                        }
                    }
                }
                connection.Close();
            }
            return View(product);
        }
        [HttpPost]
        public IActionResult ProductUpdate(Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dbconfig = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json").Build();
                    if (!string.IsNullOrEmpty(dbconfig.ToString()))
                    {
                        dbconnectionStr = dbconfig["ConnectStrings:DefaultConnection"];
                        using (SqlConnection connection=new SqlConnection(dbconnectionStr))
                        {
                            string sql = "SP_UPDATE_PRODUCT";
                            using (SqlCommand cmd = new SqlCommand(sql, connection))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@id", product.Id);
                                cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                                cmd.Parameters.AddWithValue("@ProductDescription", product.ProductDescription);
                                cmd.Parameters.AddWithValue("@ProductCost", product.ProductCost);
                                cmd.Parameters.AddWithValue("@Stock", product.Stock);
                                connection.Open();
                                cmd.ExecuteNonQuery();
                                connection.Close();

                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return RedirectToAction("Product");
        }
        [HttpPost]
        public IActionResult ProductDelete(int id)
        {
            var dbconfig = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();
            dbconnectionStr = dbconfig["ConnectionStrings:Default Connection"];
            using (SqlConnection connection = new SqlConnection(dbconnectionStr))
            {
                string sql = "SP_DELETE_PRODUCT-BY_Id";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    { }
                    connection.Close();
                }
            
            
            }
                return RedirectToAction("Product");
        }
    }
}
