using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeeApi.Entities
{
    /// <summary>
    /// Represents an Inspection.
    /// </summary>
    public class Inspection
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
        /// Gets or sets the strength.
        /// </summary>
        /// <value>
        /// The strength.
        /// </value>
        [Required]
        [MaxLength(20)]
        public string Strength { get; set; }
        /// <summary>
        /// Gets or sets the temper.
        /// </summary>
        /// <value>
        /// The temper.
        /// </value>
        [Required]
        [MaxLength(30)]
        public string Temper { get; set; }
        /// <summary>
        /// Gets or sets the disease.
        /// </summary>
        /// <value>
        /// The disease.
        /// </value>
        [Required]
        [MaxLength(30)]
        public string Disease { get; set; }
        /// <summary>
        /// Gets or sets the frames bees.
        /// </summary>
        /// <value>
        /// The frames bees.
        /// </value>
        public int FramesBees { get; set; }
        /// <summary>
        /// Gets or sets the frames honey.
        /// </summary>
        /// <value>
        /// The frames honey.
        /// </value>
        public int FramesHoney { get; set; }
        /// <summary>
        /// Gets or sets the frames honey supers.
        /// </summary>
        /// <value>
        /// The frames honey supers.
        /// </value>
        public int FramesHoneySupers { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Inspection"/> is drones.
        /// </summary>
        /// <value>
        ///   <c>true</c> if drones; otherwise, <c>false</c>.
        /// </value>
        public bool Drones { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [drone cells].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [drone cells]; otherwise, <c>false</c>.
        /// </value>
        public bool DroneCells { get; set; }
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