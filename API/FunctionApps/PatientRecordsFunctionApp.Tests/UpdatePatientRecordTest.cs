using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PcmsApi.Functions;
using PcmsApi.Core.Models;
using PcmsApi.Core.Persistence;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace PcmsApi.Functions.Tests
{
    public class UpdatePatientRecordTests
    {
        private readonly Mock<ILogger<UpdatePatientRecord>> _loggerMock;
        private readonly Mock<PcmsDbContext> _dbContextMock;
        private readonly UpdatePatientRecord _function;

        public UpdatePatientRecordTests()
        {
            _loggerMock = new Mock<ILogger<UpdatePatientRecord>>();
            _dbContextMock = new Mock<PcmsDbContext>();
            _function = new UpdatePatientRecord(_loggerMock.Object, _dbContextMock.Object);
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
        public async Task Run_ReturnsNotFound_WhenRecordDoesNotExist()
        {
            // Arrange
            var record = new PcmsApi.Core.Models.Record { Id = Guid.NewGuid(), Patient = new Patient() };
            var request = new DefaultHttpContext().Request;
            request.Body = new MemoryStream(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(record)));

            _dbContextMock.Setup(db => db.Records.FindAsync(record.Id)).ReturnsAsync((PcmsApi.Core.Models.Record)null);

            // Act
            var result = await _function.Run(request);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Record not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task Run_ReturnsOk_WhenRecordIsUpdated()
        {
            // Arrange
            var id = Guid.NewGuid();
            var record = new PcmsApi.Core.Models.Record
            {
                Id = id,
                Description = "Test Description",
                CreatedBy = "Test User",
                CreatedDate = System.DateTime.UtcNow,
                LastUpdatedBy = "Test User",
                LastUpdatedDate = System.DateTime.UtcNow,
                Patient = new Patient(),
                Attachments = new List<Attachment>()
            };
            var existingRecord = new PcmsApi.Core.Models.Record { Id = id, Patient = new Patient() };
            var request = new DefaultHttpContext().Request;
            request.Body = new MemoryStream(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(record)));

            _dbContextMock.Setup(db => db.Records.FindAsync(record.Id)).ReturnsAsync(existingRecord);
            _dbContextMock.Setup(db => db.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            var result = await _function.Run(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Record updated successfully.", okResult.Value);
        }
    }
}
