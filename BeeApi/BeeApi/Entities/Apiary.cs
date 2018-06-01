using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeeApi.Entities
{
    /// <summary>
    /// Represents An Apiary.
    /// </summary>
    public class Apiary
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Apiary"/> class.
        /// </summary>
        public Apiary()
        {
            Hives = new Collection<Hive>();
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
        [MaxLength(30)]
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the place.
        /// </summary>
        /// <value>
        /// The place.
        /// </value>
        [Required]
        [MaxLength(30)]
        public string Place { get; set; }
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
        /// Gets or sets the beekeeper identifier.
        /// </summary>
        /// <value>
        /// The beekeeper identifier.
        /// </value>
        public int BeekeeperId { get; set; }
        /// <summary>
        /// Gets or sets the beekeeper.
        /// </summary>
        /// <value>
        /// The beekeeper.
        /// </value>
        [ForeignKey("BeekeeperId")]
        public Beekeeper Beekeeper { get; set; }
        /// <summary>
        /// Gets or sets the hives.
        /// </summary>
        /// <value>
        /// The hives.
        /// </value>
        public ICollection<Hive> Hives { get; set; }
    }
}