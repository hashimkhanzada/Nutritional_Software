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
    public class Intake_api
    {
        private DbContextOptionsBuilder<ApplicationDbContext> builder;


        public Intake_api()
        {
            builder = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite("DataSource=:memory:").EnableSensitiveDataLogging();
        }

        [Fact]
        public void Intake_crud_test()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(builder.Options))
            {
                //Steps to ensure sqlite database exist
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                //Create foods api controller
                var intake_api = new IntakesController(context);

                //Use api controller and get list of foods.
                Task<ActionResult<List<Intake>>> testGetAll = intake_api.GetIntakes();

                //Test to see if list is empty before entry
                Assert.Empty(testGetAll.Result.Value);

                //Create food test object
                Intake testIntake = new Intake();
                testIntake.Id = 123;
                testIntake.Amount = 5;

                //Post food to using api controller
                Task<ActionResult<Intake>> testPost = intake_api.PostIntake(testIntake);

                //Use api controller and get list of foods.
                testGetAll = intake_api.GetIntakes();

                //Test to see if list count is 1
                Assert.Single(testGetAll.Result.Value);

                //Test to see if data is correct
                Assert.Equal(5, testGetAll.Result.Value.Where(f => f.Id == 123).First().Amount);

                //Get food by id
                Task<ActionResult<Intake>> testGet = intake_api.GetIntake(123);

                //Test to see if return data is correct
                Assert.Equal(5, testGet.Result.Value.Amount);

                //Change food name
                testIntake.Amount = 6;

                //Get food by id
                Task<IActionResult> testPut = intake_api.PutIntake(123, testIntake);

                //Get food by id
                testGet = intake_api.GetIntake(123);

                //Test to see if return data is correct
                Assert.Equal(6, testGet.Result.Value.Amount);

                //Delete the test food
                Task<ActionResult<Intake>> testDelete = intake_api.Delete(123);

                //Use api controller and get list of foods.
                testGetAll = intake_api.GetIntakes();

                //Test to see if list is empty before entry
                Assert.Empty(testGetAll.Result.Value);
            }
        }
    }

}
