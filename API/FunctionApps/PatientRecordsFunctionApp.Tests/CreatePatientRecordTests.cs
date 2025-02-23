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
using PcmsApi.Core.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Pcms.Api.Functions.Tests
{
    public class CreatePatientRecordTests
    {
        private PcmsDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<PcmsDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            var dbContext = new PcmsDbContext(options);
            return dbContext;
        }
    }
}
