using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeeApi.Entities
{
    /// <summary>
    /// Represents a Queen.
    /// </summary>
    public class Queen
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
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        public DateTime Date { get; set; }
        /// <summary>
        /// Gets or sets the breed.
        /// </summary>
        /// <value>
        /// The breed.
        /// </value>
        [Required]
        [MaxLength(20)]
        public string Breed { get; set; }
        /// <summary>
        /// Gets or sets the colour.
        /// </summary>
        /// <value>
        /// The colour.
        /// </value>
        [Required]
        [MaxLength(20)]
        public string Colour { get; set; }
        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        [Required]
        [MaxLength(30)]
        public string State { get; set; }
        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        [Required]
        [MaxLength(30)]
        public string Status { get; set; }
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