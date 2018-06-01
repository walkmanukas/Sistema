using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;
using BeeApi.Contexts;

namespace BeeApi
{
    /// <summary>
    /// Represents an Ensurer helper class.
    /// </summary>
    public class Ensurer
    {
        /// <summary>
        /// Ensures the hive belongs to apiary.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="apiaryId">The apiary identifier.</param>
        /// <param name="hiveId">The hive identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public async Task<bool> EnsureHiveBelongsToApiary(BeeAppContext context, int apiaryId, int hiveId, string userId)
        {
            var result = false;

            // Search for specific apiary and hive
            var hive = await context.Beekeepers
                .Where(x => x.ApplicationUserId == userId)
                .Include(x => x.Apiaries)
                .SelectMany(x => x.Apiaries)
                .Where(x => x.Id == apiaryId)
                .Include(x => x.Hives)
                .SelectMany(x => x.Hives)
                .Where(x => x.Id == hiveId)
                .FirstOrDefaultAsync();

            if (hive != null)
            {
                result = true;
            }

            // Return
            return result;
        }

        /// <summary>
        /// Ensures the email is unique.
        /// </summary>
        /// <param name="modelState">State of the model.</param>
        /// <param name="userManager">The user manager.</param>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        public async Task<bool> EnsureEmailIsUnique(ModelStateDictionary modelState, Identity.UserManager userManager, string email)
        {
            var result = true;

            var userWithSameEmail = await userManager.FindByEmailAsync(email);

            if (userWithSameEmail != null)
            {
                modelState.AddModelError("Email", "Email is taken");

                result = false;
            }

            // Return
            return result;
        }
    }
}