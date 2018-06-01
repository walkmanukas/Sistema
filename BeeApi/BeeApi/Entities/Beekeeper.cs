using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace BeeApi.Entities
{
    /// <summary>
    /// Represents a Beekeeper.
    /// </summary>
    /// <seealso cref="Microsoft.AspNet.Identity.EntityFramework.IdentityUser" />
    public class Beekeeper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Beekeeper"/> class.
        /// </summary>
        public Beekeeper()
        {
            Works = new Collection<Work>();
            Apiaries = new Collection<Apiary>();
        }
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        [Required]
        [MaxLength(30)]
        public string FirstName { get; set; }
        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        [Required]
        [MaxLength(30)]
        public string LastName { get; set; }
        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        /// <value>
        /// The email address.
        /// </value>
        [EmailAddress]
        [MaxLength(30)]
        public string Email { get; set; }
        /// <summary>
        /// PhoneNumber for the user
        /// </summary>
        [Required]
        public string PhoneNumber { get; set; }
        /// <summary>
        /// Gets or sets the number.
        /// </summary>
        /// <value>
        /// The number.
        /// </value>
        [Required]
        [MaxLength(20)]
        public string Number { get; set; }
        /// <summary>
        /// Gets or sets the application user identifier.
        /// </summary>
        /// <value>
        /// The application user identifier.
        /// </value>
        public string ApplicationUserId { get; set; }
        /// <summary>
        /// Gets or sets the works.
        /// </summary>
        /// <value>
        /// The works.
        /// </value>
        public ICollection<Work> Works { get; set; }
        /// <summary>
        /// Gets or sets the apiaries.
        /// </summary>
        /// <value>
        /// The apiaries.
        /// </value>
        public ICollection<Apiary> Apiaries { get; set; }
    }
}