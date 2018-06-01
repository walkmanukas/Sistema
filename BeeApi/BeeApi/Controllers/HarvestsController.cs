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
    /// Represents a Harvests Controller.
    /// </summary>
    /// <seealso cref="BeeApi.Controllers.BaseApiController" />
    [Authorize]
    [RoutePrefix("api/harvest")]
    public class HarvestsController : BaseApiController
    {
        private readonly string _applicationUserId;
        private readonly Ensurer _ensurer = new Ensurer();

        /// <summary>
        /// Initializes a new instance of the <see cref="HarvestsController"/> class.
        /// </summary>
        public HarvestsController()
        {
            _applicationUserId = User.Identity.GetUserId();
        }

        /// <summary>
        /// Posts the specified apiary identifier.
        /// </summary>
        /// <param name="apiaryId">The apiary identifier.</param>
        /// <param name="hiveId">The hive identifier.</param>
        /// <param name="harvestModel">The harvest model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{apiaryId:int}/{hiveId:int}")]
        public async Task<IHttpActionResult> Post(int apiaryId, int hiveId, HarvestModel harvestModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Harvest Model is not valid");
            }

            try
            {
                using (var context = new BeeAppContext())
                {
                    if (await _ensurer.EnsureHiveBelongsToApiary(context, apiaryId, hiveId, _applicationUserId))
                    {
                        context.Harvests.Add(new Harvest
                        {
                            Name = harvestModel.Name,
                            Date = harvestModel.Date,
                            Product = harvestModel.Product,
                            Quantity = harvestModel.Quantity,
                            Unit = harvestModel.Unit,
                            Note = harvestModel.Note,
                            HiveId = hiveId
                        });

                        // Save
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            // Return
            return Ok(harvestModel);
        }

        /// <summary>
        /// Gets the specified apiary identifier.
        /// </summary>
        /// <param name="apiaryId">The apiary identifier.</param>
        /// <param name="hiveId">The hive identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{apiaryId:int}/{hiveId:int}")]
        public IHttpActionResult Get(int apiaryId, int hiveId)
        {
            try
            {
                using (var context = new BeeAppContext())
                {
                    var harvests = context.Beekeepers
                        .Where(x => x.ApplicationUserId == _applicationUserId)
                        .Include(x => x.Apiaries)
                        .SelectMany(x => x.Apiaries)
                        .Where(x => x.Id == apiaryId)
                        .Include(x => x.Hives)
                        .SelectMany(x => x.Hives)
                        .Where(x => x.Id == hiveId)
                        .Include(x => x.Harvests)
                        .SelectMany(x => x.Harvests)
                        .Select(x => new
                        {
                            x.Id,
                            x.Name,
                            x.Date,
                            x.Product,
                            x.Quantity,
                            x.Unit,
                            x.Note
                        })
                        .ToArray();

                    // Return
                    return Ok(harvests);
                }
            }
            catch (Exception ex)
            {
                // Return
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Gets the specified apiary identifier.
        /// </summary>
        /// <param name="apiaryId">The apiary identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-all/{apiaryId:int}")]
        public IHttpActionResult Get(int apiaryId)
        {
            try
            {
                using (var context = new BeeAppContext())
                {
                    var harvests = context.Beekeepers
                        .Where(x => x.ApplicationUserId == _applicationUserId)
                        .Include(x => x.Apiaries)
                        .SelectMany(x => x.Apiaries)
                        .Where(x => x.Id == apiaryId)
                        .Include(x => x.Hives)
                        .SelectMany(x => x.Hives)
                        .Include(x => x.Harvests)
                        .SelectMany(x => x.Harvests)
                        .Select(x => new
                        {
                            x.Id,
                            x.Name,
                            x.Date,
                            x.Product,
                            x.Quantity,
                            x.Unit,
                            x.Note,
                            x.HiveId
                        })
                        .ToArray();

                    // Return
                    return Ok(harvests);
                }
            }
            catch (Exception ex)
            {
                // Return
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Gets the specified apiary identifier.
        /// </summary>
        /// <param name="apiaryId">The apiary identifier.</param>
        /// <param name="hiveId">The hive identifier.</param>
        /// <param name="harvestId">The harvest identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{apiaryId:int}/{hiveId:int}/{harvestId:int}")]
        public async Task<IHttpActionResult> Get(int apiaryId, int hiveId, int harvestId)
        {
            try
            {
                using (var context = new BeeAppContext())
                {
                    var harvest = await context.Beekeepers
                        .Where(x => x.ApplicationUserId == _applicationUserId)
                        .Include(x => x.Apiaries)
                        .SelectMany(x => x.Apiaries)
                        .Where(x => x.Id == apiaryId)
                        .Include(x => x.Hives)
                        .SelectMany(x => x.Hives)
                        .Where(x => x.Id == hiveId)
                        .Include(x => x.Harvests)
                        .SelectMany(x => x.Harvests)
                        .Where(x => x.Id == harvestId)
                        .Select(x => new
                        {
                            x.Id,
                            x.Name,
                            x.Date,
                            x.Product,
                            x.Quantity,
                            x.Unit,
                            x.Note
                        })
                        .FirstOrDefaultAsync();

                    // Return
                    return Ok(harvest);
                }
            }
            catch (Exception ex)
            {
                // Return
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Puts the specified harvest identifier.
        /// </summary>
        /// <param name="harvestId">The harvest identifier.</param>
        /// <param name="harvestModel">The harvest model.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{harvestId:int}")]
        public async Task<IHttpActionResult> Put(int harvestId, HarvestModel harvestModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Harvest Model is not valid");
            }

            try
            {
                using (var context = new BeeAppContext())
                {
                    var harvest = await context.Beekeepers
                        .Where(x => x.ApplicationUserId == _applicationUserId)
                        .Include(x => x.Apiaries)
                        .SelectMany(x => x.Apiaries)
                        .Include(x => x.Hives)
                        .SelectMany(x => x.Hives)
                        .Include(x => x.Harvests)
                        .SelectMany(x => x.Harvests)
                        .Where(x => x.Id == harvestId)
                        .FirstOrDefaultAsync();

                    if (harvest != null)
                    {
                        harvest.Name = harvestModel.Name;
                        harvest.Date = harvestModel.Date;
                        harvest.Product = harvestModel.Product;
                        harvest.Quantity = harvestModel.Quantity;
                        harvest.Unit = harvestModel.Unit;
                        harvest.Note = harvestModel.Note;

                        // Save
                        context.SaveChanges();

                        // Return
                        return StatusCode(HttpStatusCode.NoContent);
                    }

                    // Return
                    return BadRequest("Harvest could not be found");
                }
            }
            catch (Exception ex)
            {
                // Return
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes the specified harvest identifier.
        /// </summary>
        /// <param name="harvestId">The harvest identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{harvestId:int}")]
        public async Task<IHttpActionResult> Delete(int harvestId)
        {
            try
            {
                using (var context = new BeeAppContext())
                {
                    var harvest = await context.Beekeepers
                        .Where(x => x.ApplicationUserId == _applicationUserId)
                        .Include(x => x.Apiaries)
                        .SelectMany(x => x.Apiaries)
                        .Include(x => x.Hives)
                        .SelectMany(x => x.Hives)
                        .Include(x => x.Harvests)
                        .SelectMany(x => x.Harvests)
                        .Where(x => x.Id == harvestId)
                        .FirstOrDefaultAsync();

                    if (harvest != null)
                    {
                        // Remove
                        context.Harvests.Remove(harvest);

                        // Save
                        context.SaveChanges();

                        // Return
                        return StatusCode(HttpStatusCode.NoContent);
                    }

                    // Return
                    return BadRequest("Harvest could not be found");
                }
            }
            catch (Exception ex)
            {
                // Return
                return BadRequest(ex.Message);
            }
        }
    }
}