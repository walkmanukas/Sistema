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
    /// Represents a Hives controller.
    /// </summary>
    /// <seealso cref="BeeApi.Controllers.BaseApiController" />
    [Authorize]
    [RoutePrefix("api/hive")]
    public class HivesController : BaseApiController
    {
        private readonly string _applicationUserId;

        /// <summary>
        /// Initializes a new instance of the <see cref="HivesController"/> class.
        /// </summary>
        public HivesController()
        {
            _applicationUserId = User.Identity.GetUserId();
        }

        /// <summary>
        /// Posts the specified apiary identifier.
        /// </summary>
        /// <param name="apiaryId">The apiary identifier.</param>
        /// <param name="hiveModel">The create hive model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{apiaryId:int}")]
        public IHttpActionResult Post(int apiaryId, HiveModel hiveModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Hive Model is not valid");
            }

            try
            {
                using (var context = new BeeAppContext())
                {
                    if (!EnsureCreatingInOwnApiary(context, apiaryId))
                    {
                        return BadRequest("The apiary could not be found.");
                    }

                    context.Hives.Add(new Hive
                    {
                        Name = hiveModel.Name,
                        Date = hiveModel.Date,
                        Status = hiveModel.Status,
                        Type = hiveModel.Type,
                        Note = hiveModel.Note,
                        Family = hiveModel.Family,
                        FamilyOrigin = hiveModel.FamilyOrigin,
                        ApiaryId = apiaryId
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
            return Ok(hiveModel);
        }

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <param name="apiaryId">The apiary identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{apiaryId:int}")]
        public IHttpActionResult Get(int apiaryId)
        {
            try
            {
                using (var context = new BeeAppContext())
                {
                    var hives = context.Beekeepers
                        .Where(x => x.ApplicationUserId == _applicationUserId)
                        .Include(x => x.Apiaries)
                        .SelectMany(x => x.Apiaries)
                        .Where(x => x.Id == apiaryId)
                        .Include(x => x.Hives)
                        .SelectMany(x => x.Hives)
                        .Select(x => new
                        {
                            x.Id,
                            x.Name,
                            x.Date,
                            x.Status,
                            x.Type,
                            x.Note,
                            x.Family,
                            x.FamilyOrigin
                        })
                        .ToArray();

                    // Return
                    return Ok(hives);
                }
            }
            catch (Exception ex)
            {
                // Return
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Gets the specified hive identifier.
        /// </summary>
        /// <param name="apiaryId">The apiary identifier.</param>
        /// <param name="hiveId">The hive identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{apiaryId:int}/{hiveId:int}")]
        public async Task<IHttpActionResult> Get(int apiaryId, int hiveId)
        {
            try
            {
                using (var context = new BeeAppContext())
                {
                    var hive = await context.Beekeepers
                        .Where(x => x.ApplicationUserId == _applicationUserId)
                        .Include(x => x.Apiaries)
                        .SelectMany(x => x.Apiaries)
                        .Where(x => x.Id == apiaryId)
                        .Include(x => x.Hives)
                        .SelectMany(x => x.Hives)
                        .Where(x => x.Id == hiveId)
                        .Select(x => new
                        {
                            x.Id,
                            x.Name,
                            x.Date,
                            x.Status,
                            x.Type,
                            x.Note,
                            x.Family,
                            x.FamilyOrigin
                        })
                        .FirstOrDefaultAsync();

                    // Return
                    return Ok(hive);
                }
            }
            catch (Exception ex)
            {
                // Return
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Puts the specified hive identifier.
        /// </summary>
        /// <param name="hiveId">The hive identifier.</param>
        /// <param name="hiveModel">The hive model.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{hiveId:int}")]
        public async Task<IHttpActionResult> Put(int hiveId, HiveModel hiveModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Hive Model is not valid");
            }

            try
            {
                using (var context = new BeeAppContext())
                {
                    var hive = await context.Beekeepers
                        .Where(x => x.ApplicationUserId == _applicationUserId)
                        .Include(x => x.Apiaries)
                        .SelectMany(x => x.Apiaries)
                        .Include(x => x.Hives)
                        .SelectMany(x => x.Hives)
                        .Where(x => x.Id == hiveId)
                        .FirstOrDefaultAsync();

                    if (hive != null)
                    {
                        hive.Name = hiveModel.Name;
                        hive.Date = hiveModel.Date;
                        hive.Status = hiveModel.Status;
                        hive.Type = hiveModel.Type;
                        hive.Note = hiveModel.Note;
                        hive.Family = hiveModel.Family;
                        hive.FamilyOrigin = hiveModel.FamilyOrigin;

                        // Save
                        context.SaveChanges();

                        // Return
                        return StatusCode(HttpStatusCode.NoContent);
                    }

                    // Return
                    return BadRequest("Hive could not be found");
                }
            }
            catch (Exception ex)
            {
                // Return
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes the specified hive identifier.
        /// </summary>
        /// <param name="hiveId">The hive identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{hiveId:int}")]
        public async Task<IHttpActionResult> Delete(int hiveId)
        {
            try
            {
                using (var context = new BeeAppContext())
                {
                    var hive = await context.Beekeepers
                        .Where(x => x.ApplicationUserId == _applicationUserId)
                        .Include(x => x.Apiaries)
                        .SelectMany(x => x.Apiaries)
                        .Include(x => x.Hives)
                        .SelectMany(x => x.Hives)
                        .Where(x => x.Id == hiveId)
                        .FirstOrDefaultAsync();

                    if (hive != null)
                    {
                        // Remove
                        context.Hives.Remove(hive);

                        // Save
                        context.SaveChanges();

                        // Return
                        return StatusCode(HttpStatusCode.NoContent);
                    }

                    // Return
                    return BadRequest("Hive could not be found");
                }
            }
            catch (Exception ex)
            {
                // Return
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Ensures the creating in own apiary.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="apiaryId">The apiary identifier.</param>
        /// <returns></returns>
        private bool EnsureCreatingInOwnApiary(BeeAppContext context, int apiaryId)
        {
            var result = false;

            var beekeeperApiaries = context.Beekeepers
                .Where(x => x.ApplicationUserId == _applicationUserId)
                .Include(x => x.Apiaries)
                .SelectMany(x => x.Apiaries)
                .Where(x => x.Id == apiaryId);

            if (beekeeperApiaries.Any())
            {
                result = true;
            }

            // Return
            return result;
        }
    }
}