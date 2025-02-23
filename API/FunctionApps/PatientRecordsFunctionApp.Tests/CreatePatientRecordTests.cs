using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Pcms.Api.Functions;
using PcmsApi.Core.Models;
using PcmsApi.Core.Persistence;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Pcms.Api.Functions.Tests
{
    public class CreatePatientRecordTests
    {
        private readonly Mock<ILogger<CreatePatientRecord>> _loggerMock;
        private readonly Mock<PcmsDbContext> _dbContextMock;
        private readonly CreatePatientRecord _function;

        public CreatePatientRecordTests()
        {
            _loggerMock = new Mock<ILogger<CreatePatientRecord>>();
            _dbContextMock = new Mock<PcmsDbContext>();
            _function = new CreatePatientRecord(_loggerMock.Object, _dbContextMock.Object);
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
        public async Task Run_ReturnsBadRequest_WhenRecordIsNull()
        {
            // Arrange
            var request = new DefaultHttpContext().Request;
            request.Body = new MemoryStream(Encoding.UTF8.GetBytes("{}"));

            // Act
            var result = await _function.Run(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid record data.", badRequestResult.Value);
        }

        [Fact]
        public async Task Run_ReturnsOk_WhenRecordIsValid()
        {
            // Arrange
            var record = new PcmsApi.Core.Models.Record { Patient = new Patient() };
            var request = new DefaultHttpContext().Request;
            request.Body = new MemoryStream(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(record)));

            _dbContextMock.Setup(db => db.Records.AddAsync(It.IsAny<PcmsApi.Core.Models.Record>(), default))
                .Returns(new ValueTask<EntityEntry<PcmsApi.Core.Models.Record>>(Task.FromResult((EntityEntry<PcmsApi.Core.Models.Record>)null)));
            _dbContextMock.Setup(db => db.SaveChangesAsync(default)).Returns(Task.FromResult(1));

            // Act
            var result = await _function.Run(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Record created successfully.", okResult.Value);
        }
    }
}