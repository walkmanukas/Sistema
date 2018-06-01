using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using BeeApi.Contexts;
using BeeApi.Entities;
using BeeApi.Models;
using Microsoft.AspNet.Identity;

namespace BeeApi.Controllers
{
    /// <summary>
    /// Represents a Beekeeper controller.
    /// </summary>
    /// <seealso cref="BeeApi.Controllers.BaseApiController" />
    [RoutePrefix("api/beekeeper")]
    public class BeekeepersController : BaseApiController
    {
        private readonly Ensurer _ensurer = new Ensurer();

        /// <summary>
        /// Creates the user.
        /// </summary>
        /// <param name="beekeeperModel">The create beekeeper model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Post(BeekeeperModel beekeeperModel)
        {
            var user = new ApplicationUser
            {
                UserName = beekeeperModel.Email,
                Email = beekeeperModel.Email,
                FirstName = beekeeperModel.FirstName,
                LastName = beekeeperModel.LastName,
                PhoneNumber = beekeeperModel.Phone,
                Number = beekeeperModel.Number
            };

            try
            {
                await _ensurer.EnsureEmailIsUnique(ModelState, UserManager, beekeeperModel.Email);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                
                var addUserResult = await UserManager.CreateAsync(user, beekeeperModel.Password);

                if (!addUserResult.Succeeded)
                {
                    return GetErrorResult(addUserResult);
                }

                using (var context = new BeeAppContext())
                {
                    context.Beekeepers.Add(new Beekeeper
                    {
                        FirstName = beekeeperModel.FirstName,
                        LastName = beekeeperModel.LastName,
                        Email = beekeeperModel.Email,
                        PhoneNumber = beekeeperModel.Phone,
                        Number = beekeeperModel.Number,
                        ApplicationUserId = user.Id
                    });

                    // Save
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            // Return
            return Ok(new
            {
                user.Id,
                user.Email
            });
        }

        /// <summary>
        /// Gets the information of current Beekeeper.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("")]
        public async Task<IHttpActionResult> Get()
        {
            var applicationUserId = User.Identity.GetUserId();

            try
            {
                using (var context = new BeeAppContext())
                {
                    var beekeeper = await context.Beekeepers
                        .Where(x => x.ApplicationUserId == applicationUserId)
                        .Select(x => new
                        {
                            x.Id,
                            x.FirstName,
                            x.LastName,
                            x.Email,
                            x.PhoneNumber,
                            x.Number
                        })
                        .ToArrayAsync();

                    // Return
                    return Ok(beekeeper);
                }
            }
            catch (Exception ex)
            {
                // Return
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates the information.
        /// </summary>
        /// <param name="beekeeperUpdateInfo">The beekeeper update information.</param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        [Route("update-info")]
        public async Task<IHttpActionResult> UpdateInfo(BeekeeperUpdateInfo beekeeperUpdateInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Get current User Id
            var applicationUserId = User.Identity.GetUserId();

            using (var context = new BeeAppContext())
            {
                var beekeeper = await context.Beekeepers
                    .FirstOrDefaultAsync(x => x.ApplicationUserId == applicationUserId);

                if (beekeeper != null)
                {
                    beekeeper.FirstName = beekeeperUpdateInfo.FirstName ?? beekeeper.FirstName;
                    beekeeper.LastName = beekeeperUpdateInfo.LastName ?? beekeeper.LastName;
                    beekeeper.PhoneNumber = beekeeperUpdateInfo.Phone ?? beekeeper.PhoneNumber;
                    beekeeper.Number = beekeeperUpdateInfo.Number ?? beekeeper.Number;

                    // Save
                    context.SaveChanges();

                    // Return
                    return StatusCode(HttpStatusCode.NoContent);
                }
            }

            // Return
            return Ok();
        }

        /// <summary>
        /// Puts the new password.
        /// </summary>
        /// <param name="beekeeperChangePasswordModel">The change password model.</param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        [Route("change-password")]
        public async Task<IHttpActionResult> PutNewPassword(BeekeeperChangePasswordModel beekeeperChangePasswordModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Update the password
                var result = await UserManager.ChangePasswordAsync(
                    User.Identity.GetUserId(),
                    beekeeperChangePasswordModel.OldPassword,
                    beekeeperChangePasswordModel.NewPassword);

                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            // Return
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}