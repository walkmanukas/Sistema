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
    /// Represents a Feedings Controller.
    /// </summary>
    /// <seealso cref="BeeApi.Controllers.BaseApiController" />
    [Authorize]
    [RoutePrefix("api/feeding")]
    public class FeedingsController : BaseApiController
    {
        private readonly string _applicationUserId;
        private readonly Ensurer _ensurer = new Ensurer();

        /// <summary>
        /// Initializes a new instance of the <see cref="FeedingsController"/> class.
        /// </summary>
        public FeedingsController()
        {
            _applicationUserId = User.Identity.GetUserId();
        }

        /// <summary>
        /// Posts the specified apiary identifier.
        /// </summary>
        /// <param name="apiaryId">The apiary identifier.</param>
        /// <param name="hiveId">The hive identifier.</param>
        /// <param name="feedingModel">The feeding model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{apiaryId:int}/{hiveId:int}")]
        public async Task<IHttpActionResult> Post(int apiaryId, int hiveId, FeedingModel feedingModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Feeding Model is not valid");
            }

            try
            {
                using (var context = new BeeAppContext())
                {
                    if (await _ensurer.EnsureHiveBelongsToApiary(context, apiaryId, hiveId, _applicationUserId))
                    {
                        context.Feedings.Add(new Feeding
                        {
                            Name = feedingModel.Name,
                            Date = feedingModel.Date,
                            Product = feedingModel.Product,
                            Quantity = feedingModel.Quantity,
                            Unit = feedingModel.Unit,
                            Note = feedingModel.Note,
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
            return Ok(feedingModel);
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
                    var feedings = context.Beekeepers
                        .Where(x => x.ApplicationUserId == _applicationUserId)
                        .Include(x => x.Apiaries)
                        .SelectMany(x => x.Apiaries)
                        .Where(x => x.Id == apiaryId)
                        .Include(x => x.Hives)
                        .SelectMany(x => x.Hives)
                        .Where(x => x.Id == hiveId)
                        .Include(x => x.Feedings)
                        .SelectMany(x => x.Feedings)
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
                    return Ok(feedings);
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
                    var feedings = context.Beekeepers
                        .Where(x => x.ApplicationUserId == _applicationUserId)
                        .Include(x => x.Apiaries)
                        .SelectMany(x => x.Apiaries)
                        .Where(x => x.Id == apiaryId)
                        .Include(x => x.Hives)
                        .SelectMany(x => x.Hives)
                        .Include(x => x.Feedings)
                        .SelectMany(x => x.Feedings)
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
                    return Ok(feedings);
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
        /// <param name="feedingId">The feeding identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{apiaryId:int}/{hiveId:int}/{feedingId:int}")]
        public async Task<IHttpActionResult> Get(int apiaryId, int hiveId, int feedingId)
        {
            try
            {
                using (var context = new BeeAppContext())
                {
                    var feeding = await context.Beekeepers
                        .Where(x => x.ApplicationUserId == _applicationUserId)
                        .Include(x => x.Apiaries)
                        .SelectMany(x => x.Apiaries)
                        .Where(x => x.Id == apiaryId)
                        .Include(x => x.Hives)
                        .SelectMany(x => x.Hives)
                        .Where(x => x.Id == hiveId)
                        .Include(x => x.Feedings)
                        .SelectMany(x => x.Feedings)
                        .Where(x => x.Id == feedingId)
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
                    return Ok(feeding);
                }
            }
            catch (Exception ex)
            {
                // Return
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Puts the specified feeding identifier.
        /// </summary>
        /// <param name="feedingId">The feeding identifier.</param>
        /// <param name="feedingModel">The feeding model.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{feedingId:int}")]
        public async Task<IHttpActionResult> Put(int feedingId, FeedingModel feedingModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Feeding Model is not valid");
            }

            try
            {
                using (var context = new BeeAppContext())
                {
                    var feeding = await context.Beekeepers
                        .Where(x => x.ApplicationUserId == _applicationUserId)
                        .Include(x => x.Apiaries)
                        .SelectMany(x => x.Apiaries)
                        .Include(x => x.Hives)
                        .SelectMany(x => x.Hives)
                        .Include(x => x.Feedings)
                        .SelectMany(x => x.Feedings)
                        .Where(x => x.Id == feedingId)
                        .FirstOrDefaultAsync();

                    if (feeding != null)
                    {
                        feeding.Name = feedingModel.Name;
                        feeding.Date = feedingModel.Date;
                        feeding.Product = feedingModel.Product;
                        feeding.Quantity = feedingModel.Quantity;
                        feeding.Unit = feedingModel.Unit;
                        feeding.Note = feedingModel.Note;

                        // Save
                        context.SaveChanges();

                        // Return
                        return StatusCode(HttpStatusCode.NoContent);
                    }

                    // Return
                    return BadRequest("Feeding could not be found");
                }
            }
            catch (Exception ex)
            {
                // Return
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes the specified feeding identifier.
        /// </summary>
        /// <param name="feedingId">The feeding identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{feedingId:int}")]
        public async Task<IHttpActionResult> Delete(int feedingId)
        {
            try
            {
                using (var context = new BeeAppContext())
                {
                    var feeding = await context.Beekeepers
                        .Where(x => x.ApplicationUserId == _applicationUserId)
                        .Include(x => x.Apiaries)
                        .SelectMany(x => x.Apiaries)
                        .Include(x => x.Hives)
                        .SelectMany(x => x.Hives)
                        .Include(x => x.Feedings)
                        .SelectMany(x => x.Feedings)
                        .Where(x => x.Id == feedingId)
                        .FirstOrDefaultAsync();

                    if (feeding != null)
                    {
                        // Remove
                        context.Feedings.Remove(feeding);

                        // Save
                        context.SaveChanges();

                        // Return
                        return StatusCode(HttpStatusCode.NoContent);
                    }

                    // Return
                    return BadRequest("Feeding could not be found");
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