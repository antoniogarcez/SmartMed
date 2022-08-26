using Moq;
using Moq.EntityFrameworkCore;
using NUnit.Framework;
using SmartMed.API.Controllers;
using SmartMed.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using DatabaseEntities = SmartMed.Database.Entities;

namespace SmartMed.Tests
{
    [TestFixture]
    public class BaseControllerTests
    {
        protected const string MEDICATION_NAME = "MedTest";

        protected Mock<SmartMedContext> _mock = null;
        protected MedicationsController _medicationsController = null;

        [OneTimeSetUp]
        protected void SetUp()
        {
            _mock = new Mock<SmartMedContext>();

            var medication = new DatabaseEntities.Medication
            {
                Id = Guid.NewGuid(),
                Name = MEDICATION_NAME,
                Quantity = 10,
                CreationDate = DateTime.Now
            };
            var medications = new List<DatabaseEntities.Medication> { medication };

            _mock.Setup(x => x.Medication).ReturnsDbSet(medications);
            _mock.Setup(m => m.Medication.AddAsync(It.IsAny<DatabaseEntities.Medication>(), default)).Callback<DatabaseEntities.Medication, CancellationToken>((entity, cancellationToken) => { medications.Add(entity); });
            _mock.Setup(m => m.Medication.Remove(It.IsAny<DatabaseEntities.Medication>())).Callback<DatabaseEntities.Medication>((entity) => medications.Remove(entity));

            _medicationsController = new MedicationsController(_mock.Object);
        }

        [OneTimeTearDown]
        protected void TearDown()
        {
            _mock = null;
            _medicationsController = null;
        }
    }
}
