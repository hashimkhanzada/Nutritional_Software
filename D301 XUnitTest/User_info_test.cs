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
    public class User_info_test
    {
        [Fact]
        public void User_test()
        {
            //Create test user
            ApplicationUser testUser = new ApplicationUser();

            //Set initial values
            testUser.DOB = DateTime.Now.AddYears(-20);
            testUser.FirstName = "John";
            testUser.LastName = "Doe";

            //Test if age was returned correctly
            Assert.Equal(20, testUser.Age);

            //Test if fullname was returned correctly
            Assert.Equal("John Doe", testUser.FullName);

        }
    }

}
