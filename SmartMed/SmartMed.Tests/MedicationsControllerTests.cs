using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientEntities = SmartMed.Client.Entities;
using DatabaseEntities = SmartMed.Database.Entities;

namespace SmartMed.Tests
{
    public class MedicationsControllerTests : BaseControllerTests
    {
        [Test]
        public void TestGet()
        {
            var result = _medicationsController.Get();
            Assert.NotNull(result);

            var objectResult = result as ObjectResult;
            Assert.AreEqual(StatusCodes.Status200OK, objectResult.StatusCode);

            var okResult = result as OkObjectResult;
            Assert.True(result is OkObjectResult);
            Assert.IsInstanceOf<IQueryable<DatabaseEntities.Medication>>(okResult.Value);
        }

        [Test]
        public async Task TestCreate()
        {
            var result = await _medicationsController.Create(new ClientEntities.Medication()
            {
                Name = MEDICATION_NAME + " Test",
                Quantity = 10
            });
            Assert.NotNull(result);

            var statusCodeResult = result as StatusCodeResult;
            Assert.AreEqual(StatusCodes.Status204NoContent, statusCodeResult.StatusCode);
        }

        [Test]
        public async Task TestCreateBadRequest_Null()
        {
            var result = await _medicationsController.Create(null);
            Assert.NotNull(result);

            var badRequestResult = result as BadRequestObjectResult;
            Assert.True(result is BadRequestObjectResult);
        }

        [Test]
        public async Task TestCreateBadRequest_NullName()
        {
            var result = await _medicationsController.Create(new ClientEntities.Medication()
            {
                Name = null,
                Quantity = 10
            });
            Assert.NotNull(result);

            var badRequestResult = result as BadRequestObjectResult;
            Assert.True(result is BadRequestObjectResult);
        }

        [Test]
        public async Task TestCreateConflict_InvalidName()
        {
            var result = await _medicationsController.Create(new ClientEntities.Medication()
            {
                Name = MEDICATION_NAME,
                Quantity = 20
            });
            Assert.NotNull(result);

            var conflictResult = result as ConflictObjectResult;
            Assert.True(result is ConflictObjectResult);
        }

        [Test]
        public async Task TestCreateBadRequest_InvalidQuantity()
        {
            var result = await _medicationsController.Create(new ClientEntities.Medication()
            {
                Name = MEDICATION_NAME + "Test 2",
                Quantity = 0
            });
            Assert.NotNull(result);

            var badRequestResult = result as BadRequestObjectResult;
            Assert.True(result is BadRequestObjectResult);
        }

        [Test]
        public async Task TestDelete()
        {
            DatabaseEntities.Medication medication = _mock.Object.Medication.First();

            var result = await _medicationsController.Delete(medication.Id.ToString());
            Assert.NotNull(result);

            var statusCodeResult = result as StatusCodeResult;
            Assert.AreEqual(StatusCodes.Status204NoContent, statusCodeResult.StatusCode);
        }

        [Test]
        public async Task TestDeleteBadRequest_NullId()
        {
            var result = await _medicationsController.Delete(null);
            Assert.NotNull(result);

            var badRequestResult = result as BadRequestObjectResult;
            Assert.True(result is BadRequestObjectResult);
        }

        [Test]
        public async Task TestDeleteBadRequest_EmptyId()
        {
            var result = await _medicationsController.Delete(string.Empty);
            Assert.NotNull(result);

            var badRequestResult = result as BadRequestObjectResult;
            Assert.True(result is BadRequestObjectResult);
        }

        [Test]
        public async Task TestDeleteNotFound()
        {
            var result = await _medicationsController.Delete(Guid.NewGuid().ToString());
            Assert.NotNull(result);

            var statusCodeResult = result as StatusCodeResult;
            Assert.AreEqual(StatusCodes.Status404NotFound, statusCodeResult.StatusCode);
        }
    }
}
