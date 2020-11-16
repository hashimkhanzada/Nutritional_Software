using D301_WebApp;
using D301_WebApp.Api;
using D301_WebApp.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;


namespace D301_XUnitTest
{
    public class Foods_api
    {
        private DbContextOptionsBuilder<ApplicationDbContext> builder;


        public Foods_api()
        {
            builder = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite("DataSource=:memory:").EnableSensitiveDataLogging();
        }

        [Fact]
        public void Food_crud_test()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(builder.Options))
            {
                //Steps to ensure sqlite database exist
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                //Create foods api controller
                var foods_api = new FoodsController(context);

                //Use api controller and get list of foods.
                Task<ActionResult<IEnumerable<Food>>> testGetAll = foods_api.GetFoods();

                //Test to see if list is empty before entry
                Assert.Empty(testGetAll.Result.Value);

                //Create food test object
                Food testFood = new Food();
                testFood.Id = "123";
                testFood.Name = "Test1";

                //Post food to using api controller
                Task<ActionResult<Food>> testPost = foods_api.PostFood(testFood);

                //Use api controller and get list of foods.
                testGetAll = foods_api.GetFoods();

                //Test to see if list count is 1
                Assert.Single(testGetAll.Result.Value);

                //Test to see if data is correct
                Assert.Equal("Test1", testGetAll.Result.Value.Where(f => f.Id == "123").First().Name);

                //Get food by id
                Task<ActionResult<Food>> testGet = foods_api.GetFood("123");

                //Test to see if return data is correct
                Assert.Equal("Test1", testGet.Result.Value.Name);

                //Change food name
                testFood.Name = "Test2";

                //Get food by id
                Task<IActionResult> testPut = foods_api.PutFood("123", testFood);

                //Get food by id
                testGet = foods_api.GetFood("123");

                //Test to see if return data is correct
                Assert.Equal("Test2", testGet.Result.Value.Name);

                //Delete the test food
                Task<ActionResult<Food>> testDelete = foods_api.DeleteFood("123");

                //Use api controller and get list of foods.
                testGetAll = foods_api.GetFoods();

                //Test to see if list is empty before entry
                Assert.Empty(testGetAll.Result.Value);
            }
        }
    }

}
