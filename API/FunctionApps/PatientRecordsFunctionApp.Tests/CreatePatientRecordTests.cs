using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Pcms.Api.Functions;
using PcmsApi.Core.Models;

namespace Pcms.Api.Functions.Tests
{
    public class CreatePatientRecordTests
    {
        [Fact]
        public async Task Run_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<CreatePatientRecord>>();
            var function = new CreatePatientRecord(loggerMock.Object);

            var requestMock = new Mock<HttpRequest>();
            var json = @"{
                ""Id"": ""1"",
                ""Description"": ""Test record"",
                ""CreatedBy"": ""testuser"",
                ""CreatedDate"": ""2025-02-23T00:00:00Z"",
                ""LastUpdatedBy"": ""testuser"",
                ""LastUpdatedDate"": ""2025-02-23T00:00:00Z"",
                ""Patient"": {
                    ""Id"": ""1"",
                    ""FirstName"": ""John"",
                    ""LastName"": ""Doe""
                }
            }";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            requestMock.Setup(r => r.Body).Returns(stream);

            // Act
            var result = await function.Run(requestMock.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Record created successfully.", okResult.Value);
        }

        [Fact]
        public async Task Run_InvalidJson_ReturnsBadRequest()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<CreatePatientRecord>>();
            var function = new CreatePatientRecord(loggerMock.Object);

            var requestMock = new Mock<HttpRequest>();
            var json = @"{ invalid json }";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            requestMock.Setup(r => r.Body).Returns(stream);

            // Act
            var result = await function.Run(requestMock.Object);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid JSON format.", badRequestResult.Value);
        }

        [Fact]
        public async Task Run_InvalidRecordData_ReturnsBadRequest()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<CreatePatientRecord>>();
            var function = new CreatePatientRecord(loggerMock.Object);

            var requestMock = new Mock<HttpRequest>();
            var json = @"{
                ""Id"": ""1"",
                ""Description"": ""Test record"",
                ""CreatedBy"": ""testuser"",
                ""CreatedDate"": ""2025-02-23T00:00:00Z"",
                ""LastUpdatedBy"": ""testuser"",
                ""LastUpdatedDate"": ""2025-02-23T00:00:00Z"",
                ""Patient"": null
            }";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            requestMock.Setup(r => r.Body).Returns(stream);

            // Act
            var result = await function.Run(requestMock.Object);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid record data.", badRequestResult.Value);
        }
}
