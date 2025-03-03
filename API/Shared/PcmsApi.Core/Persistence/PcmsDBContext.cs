using Microsoft.EntityFrameworkCore;
using PcmsApi.Core.Models;

namespace PcmsApi.Core.Persistence
{
    /// <summary>
    /// Represents the database context for the Patient Care Management System.
    /// </summary>
    public class PcmsDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PcmsDbContext"/> class.
        /// </summary>
        /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
        public PcmsDbContext(DbContextOptions<PcmsDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PcmsDbContext"/> class.
        /// </summary>
        public PcmsDbContext() : base()
        {
        }

        /// <summary>
        /// Gets or sets the Patients table.
        /// </summary>
        public virtual DbSet<Patient> Patients { get; set; }

        /// <summary>
        /// Gets or sets the Attachments table.
        /// </summary>
        public virtual DbSet<Attachment> Attachments { get; set; }

        /// <summary>
        /// Gets or sets the Records table.
        /// </summary>
        public virtual DbSet<Record> Records { get; set; }
    }
}
