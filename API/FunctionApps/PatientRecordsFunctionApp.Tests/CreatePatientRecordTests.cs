using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PcmsApi.Core.Models;
using PcmsApi.Core.Persistence;
using PcmsApi.Functions;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace PcmsApi.Functions.Tests
{
    public class CreatePatientRecordTest
    {
        private readonly Mock<ILogger<CreatePatientRecord>> _loggerMock;
        private readonly Mock<PcmsDbContext> _dbContextMock;
        private readonly CreatePatientRecord _function;
        private readonly Mock<DbSet<PcmsApi.Core.Models.Record>> _dbSetMock;

        public CreatePatientRecordTest()
        {
            _loggerMock = new Mock<ILogger<CreatePatientRecord>>();
            _dbContextMock = new Mock<PcmsDbContext>();
            _dbSetMock = new Mock<DbSet<PcmsApi.Core.Models.Record>>();
            _function = new CreatePatientRecord(_loggerMock.Object, _dbContextMock.Object);

            _dbContextMock.Setup(db => db.Records).Returns(_dbSetMock.Object);
        }

        [Fact]
        public async Task Run_ReturnsBadRequest_WhenJsonIsInvalid()
        {
            // Arrange
            var request = new DefaultHttpContext().Request;
            request.Body = new MemoryStream(Encoding.UTF8.GetBytes("invalid json"));

            // Act
            var result = await _function.Run(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid JSON format.", badRequestResult.Value);
        }

        [Fact]
        public async Task Run_ReturnsOk_WhenRecordIsValid()
        {
            // Arrange
            var record = new PcmsApi.Core.Models.Record { Patient = new PcmsApi.Core.Models.Patient() };
            var request = new DefaultHttpContext().Request;
            request.Body = new MemoryStream(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(record)));

            var mockDbSet = new Mock<DbSet<PcmsApi.Core.Models.Record>>();
            _dbContextMock.Setup(db => db.Records).Returns(mockDbSet.Object);
            mockDbSet.Setup(db => db.AddAsync(It.IsAny<PcmsApi.Core.Models.Record>(), default))
                .ReturnsAsync((PcmsApi.Core.Models.Record r, CancellationToken ct) => (EntityEntry<PcmsApi.Core.Models.Record>)null);
            _dbContextMock.Setup(db => db.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            var result = await _function.Run(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Record created successfully.", okResult.Value);
        }
    }
}