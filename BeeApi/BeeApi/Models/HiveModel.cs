using System;
using System.ComponentModel.DataAnnotations;

namespace BeeApi.Models
{
    /// <summary>
    /// Represents a Hive Model.
    /// </summary>
    public class HiveModel
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        public DateTime Date { get; set; }
        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        ///   <c>true</c> if status; otherwise, <c>false</c>.
        /// </value>
        public bool Status { get; set; }
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        [Required]
        [MaxLength(20)]
        public string Type { get; set; }
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
        /// Gets or sets the family.
        /// </summary>
        /// <value>
        /// The family.
        /// </value>
        [Required]
        [MaxLength(30)]
        public string Family { get; set; }
        /// <summary>
        /// Gets or sets the family origin.
        /// </summary>
        /// <value>
        /// The family origin.
        /// </value>
        [Required]
        [MaxLength(30)]
        public string FamilyOrigin { get; set; }
    }
}