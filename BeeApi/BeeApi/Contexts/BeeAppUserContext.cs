using System.Data.Entity;
using BeeApi.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BeeApi.Contexts
{
    /// <summary>
    /// Represents a context for manipulating ASP.NET Identity users.
    /// </summary>
    /// <seealso />
    public class BeeAppUserContext : IdentityDbContext<ApplicationUser>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BeeAppUserContext"/> class.
        /// </summary>
        public BeeAppUserContext() 
            : base("BeeAppConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer<BeeAppUserContext>(null);

            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns></returns>
        public static BeeAppUserContext Create()
        {
            return new BeeAppUserContext();
        }
    }
}