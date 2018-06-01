using System;
using System.ComponentModel.DataAnnotations;

namespace BeeApi.Models
{
    /// <summary>
    /// Represents a Harvest Model.
    /// </summary>
    public class HarvestModel
    {
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
    }
}