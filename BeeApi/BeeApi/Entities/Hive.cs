using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeeApi.Entities
{
    /// <summary>
    /// Represents a Hive.
    /// </summary>
    public class Hive
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Hive"/> class.
        /// </summary>
        public Hive()
        {
            Monitoring = new Collection<Monitoring>();
            Queens = new Collection<Queen>();
            Inspections = new Collection<Inspection>();
            Harvests = new Collection<Harvest>();
            Feedings = new Collection<Feeding>();
            Treatments = new Collection<Treatment>();
        }
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
        /// Gets or sets a value indicating whether this <see cref="Hive"/> is status.
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
        /// <summary>
        /// Gets or sets the apiary identifier.
        /// </summary>
        /// <value>
        /// The apiary identifier.
        /// </value>
        public int ApiaryId { get; set; }
        /// <summary>
        /// Gets or sets the apiary.
        /// </summary>
        /// <value>
        /// The apiary.
        /// </value>
        [ForeignKey("ApiaryId")]
        public Apiary Apiary { get; set; }
        /// <summary>
        /// Gets or sets the monitoring.
        /// </summary>
        /// <value>
        /// The monitoring.
        /// </value>
        public ICollection<Monitoring> Monitoring { get; set; }
        /// <summary>
        /// Gets or sets the queens.
        /// </summary>
        /// <value>
        /// The queens.
        /// </value>
        public ICollection<Queen> Queens { get; set; }
        /// <summary>
        /// Gets or sets the inspections.
        /// </summary>
        /// <value>
        /// The inspections.
        /// </value>
        public ICollection<Inspection> Inspections { get; set; }
        /// <summary>
        /// Gets or sets the harvests.
        /// </summary>
        /// <value>
        /// The harvests.
        /// </value>
        public ICollection<Harvest> Harvests { get; set; }
        /// <summary>
        /// Gets or sets the feedings.
        /// </summary>
        /// <value>
        /// The feedings.
        /// </value>
        public ICollection<Feeding> Feedings { get; set; }
        /// <summary>
        /// Gets or sets the treatments.
        /// </summary>
        /// <value>
        /// The treatments.
        /// </value>
        public ICollection<Treatment> Treatments { get; set; }
    }
}