using System.Security.Claims;
using System.Threading.Tasks;
using BeeApi.Entities;
using Microsoft.AspNet.Identity;

namespace BeeApi.Identity
{
    public static class UserIdentityGenerator
    {
        /// <summary>
        /// Generates the user identity asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="manager">The manager.</param>
        /// <param name="authenticationType">Type of the authentication.</param>
        /// <returns></returns>
        public static async Task<ClaimsIdentity> GenerateUserIdentityAsync(
            this ApplicationUser user,
            UserManager<ApplicationUser> manager,
            string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(user, authenticationType);

            // Add custom user claims here
            return userIdentity;
        }
    }
}