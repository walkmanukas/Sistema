using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeeApi.Entities
{
    /// <summary>
    /// Represents a Treatment.
    /// </summary>
    public class Treatment
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
        /// Gets or sets the product.
        /// </summary>
        /// <value>
        /// The product.
        /// </value>
        [Required]
        [MaxLength(30)]
        public string Product { get; set; }
        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        /// <value>
        /// The quantity.
        /// </value>
        [Required]
        [MaxLength(30)]
        public string Quantity { get; set; }
        /// <summary>
        /// Gets or sets the unit.
        /// </summary>
        /// <value>
        /// The unit.
        /// </value>
        [Required]
        [MaxLength(30)]
        public string Unit { get; set; }
        /// <summary>
        /// Gets or sets the note.
        /// </summary>
        /// <value>
        /// The note.
        /// </value>
        [Required]
        [MaxLength(200)]
        public string Note { get; set; }
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