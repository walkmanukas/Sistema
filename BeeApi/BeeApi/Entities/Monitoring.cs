using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeeApi.Entities
{
    /// <summary>
    /// Represents a Monitoring.
    /// </summary>
    public class Monitoring
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the timestamp.
        /// </summary>
        /// <value>
        /// The timestamp.
        /// </value>
        [Required]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Gets or sets the temperature.
        /// </summary>
        /// <value>
        /// The temperature.
        /// </value>
        [Required]
        [MaxLength(30)]
        public string Temperature { get; set; }
        /// <summary>
        /// Gets or sets the humidity.
        /// </summary>
        /// <value>
        /// The humidity.
        /// </value>
        [Required]
        [MaxLength(30)]
        public string Humidity { get; set; }
        /// <summary>
        /// Gets or sets the longtitude.
        /// </summary>
        /// <value>
        /// The longtitude.
        /// </value>
        [Required]
        [MaxLength(30)]
        public string Longtitude { get; set; }
        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>
        /// The latitude.
        /// </value>
        [Required]
        [MaxLength(30)]
        public string Latitude { get; set; }
        /// <summary>
        /// Gets or sets the hive identifier.
        /// </summary>
        /// <value>
        /// The hive identifier.
        /// </value>
        public int HiveId { get; set; }
        /// <summary>
        /// Gets or sets the hive.
        /// </summary>
        /// <value>
        /// The hive.
        /// </value>
        [ForeignKey("HiveId")]
        public Hive Hive { get; set; }
    }
}