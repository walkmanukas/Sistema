using BeeApi.Contexts;
using BeeApi.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace BeeApi.Identity
{
    /// <summary>
    /// Represents an User Manager class.
    /// </summary>
    /// <seealso />
    public class UserManager : UserManager<ApplicationUser>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserManager"/> class.
        /// </summary>
        /// <param name="store"></param>
        public UserManager(IUserStore<ApplicationUser> store)
            : base(store)
        { }

        /// <summary>
        /// Creates the specified options.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public static UserManager Create(IdentityFactoryOptions<UserManager> options, IOwinContext context)
        {
            var appDbContext = context.Get<BeeAppUserContext>();

            var appUserManager = new UserManager(new UserStore<ApplicationUser>(appDbContext));

            // Return
            return appUserManager;
        }
    }
}