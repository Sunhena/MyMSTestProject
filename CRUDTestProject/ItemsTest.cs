
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MyCrudAPP.Controllers; // Replace with your actual namespace
using MyCrudAPP.Models;      // Replace with your Items model namespace
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Moq.Protected;
using System.ComponentModel.DataAnnotations;


namespace CRUDTestProject
{
    [TestClass]
    public class ItemsTest
    {
        [TestMethod]
        public void ItemsModel_ValidModel_PassesValidation()
        {
            // Arrange
            var model = new Items
            {
                Id = 1,
                Name = "Test Item",
                Gender = "Male",
                Email = "test@gmail.com"
            };

            var context = new ValidationContext(model);
            var results = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(model, context, results, true);

            // Assert
            Assert.IsTrue(isValid);
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void ItemsModel_InvalidModel_FailsValidation()
        {
            // Arrange
            var model = new Items
            {
                Id = 1,
                Name = "", // Required field violation
                Gender = "Male",
                Email = ""
            };

            var context = new ValidationContext(model);
            var results = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(model, context, results, true);

            // Assert
            Assert.IsFalse(isValid);
            Assert.AreEqual(2, results.Count);
            Assert.IsTrue(results.Exists(r => r.ErrorMessage.Contains("required") || r.ErrorMessage.Contains("length")));
            Assert.IsTrue(results.Exists(r => r.ErrorMessage.Contains("range")));
        }


        [TestMethod]
        public async Task Create_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var controller = new ItemsController(); // or mock dependencies
            controller.ModelState.AddModelError("Name", "Required");

            var invalidItem = new Items { Gender = "Female",Email="test@gmail.com" }; // Missing name

            // Act
            var result = controller.Create(invalidItem);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }
    }
}