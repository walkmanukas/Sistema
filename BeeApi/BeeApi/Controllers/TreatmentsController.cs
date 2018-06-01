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
    /// Represents a Treatments Controller.
    /// </summary>
    /// <seealso cref="BeeApi.Controllers.BaseApiController" />
    [Authorize]
    [RoutePrefix("api/treatment")]
    public class TreatmentsController : BaseApiController
    {
        private readonly string _applicationUserId;
        private readonly Ensurer _ensurer = new Ensurer();

        /// <summary>
        /// Initializes a new instance of the <see cref="TreatmentsController"/> class.
        /// </summary>
        public TreatmentsController()
        {
            _applicationUserId = User.Identity.GetUserId();
        }

        /// <summary>
        /// Posts the specified apiary identifier.
        /// </summary>
        /// <param name="apiaryId">The apiary identifier.</param>
        /// <param name="hiveId">The hive identifier.</param>
        /// <param name="treatmentModel">The treatment model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{apiaryId:int}/{hiveId:int}")]
        public async Task<IHttpActionResult> Post(int apiaryId, int hiveId, TreatmentModel treatmentModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Treatment Model is not valid");
            }

            try
            {
                using (var context = new BeeAppContext())
                {
                    if (await _ensurer.EnsureHiveBelongsToApiary(context, apiaryId, hiveId, _applicationUserId))
                    {
                        context.Treatments.Add(new Treatment
                        {
                            Name = treatmentModel.Name,
                            Date = treatmentModel.Date,
                            Product = treatmentModel.Product,
                            Quantity = treatmentModel.Quantity,
                            Unit = treatmentModel.Unit,
                            Note = treatmentModel.Note,
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
            return Ok(treatmentModel);
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
                    var treatments = context.Beekeepers
                        .Where(x => x.ApplicationUserId == _applicationUserId)
                        .Include(x => x.Apiaries)
                        .SelectMany(x => x.Apiaries)
                        .Where(x => x.Id == apiaryId)
                        .Include(x => x.Hives)
                        .SelectMany(x => x.Hives)
                        .Where(x => x.Id == hiveId)
                        .Include(x => x.Treatments)
                        .SelectMany(x => x.Treatments)
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
                    return Ok(treatments);
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
                    var treatments = context.Beekeepers
                        .Where(x => x.ApplicationUserId == _applicationUserId)
                        .Include(x => x.Apiaries)
                        .SelectMany(x => x.Apiaries)
                        .Where(x => x.Id == apiaryId)
                        .Include(x => x.Hives)
                        .SelectMany(x => x.Hives)
                        .Include(x => x.Treatments)
                        .SelectMany(x => x.Treatments)
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
                    return Ok(treatments);
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
        /// <param name="treatmentId">The treatment identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{apiaryId:int}/{hiveId:int}/{treatmentId:int}")]
        public async Task<IHttpActionResult> Get(int apiaryId, int hiveId, int treatmentId)
        {
            try
            {
                using (var context = new BeeAppContext())
                {
                    var treatment = await context.Beekeepers
                        .Where(x => x.ApplicationUserId == _applicationUserId)
                        .Include(x => x.Apiaries)
                        .SelectMany(x => x.Apiaries)
                        .Where(x => x.Id == apiaryId)
                        .Include(x => x.Hives)
                        .SelectMany(x => x.Hives)
                        .Where(x => x.Id == hiveId)
                        .Include(x => x.Treatments)
                        .SelectMany(x => x.Treatments)
                        .Where(x => x.Id == treatmentId)
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
                    return Ok(treatment);
                }
            }
            catch (Exception ex)
            {
                // Return
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Puts the specified treatment identifier.
        /// </summary>
        /// <param name="treatmentId">The treatment identifier.</param>
        /// <param name="treatmentModel">The treatment model.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{treatmentId:int}")]
        public async Task<IHttpActionResult> Put(int treatmentId, TreatmentModel treatmentModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Treatment Model is not valid");
            }

            try
            {
                using (var context = new BeeAppContext())
                {
                    var treatment = await context.Beekeepers
                        .Where(x => x.ApplicationUserId == _applicationUserId)
                        .Include(x => x.Apiaries)
                        .SelectMany(x => x.Apiaries)
                        .Include(x => x.Hives)
                        .SelectMany(x => x.Hives)
                        .Include(x => x.Treatments)
                        .SelectMany(x => x.Treatments)
                        .Where(x => x.Id == treatmentId)
                        .FirstOrDefaultAsync();

                    if (treatment != null)
                    {
                        treatment.Name = treatmentModel.Name;
                        treatment.Date = treatmentModel.Date;
                        treatment.Product = treatmentModel.Product;
                        treatment.Quantity = treatmentModel.Quantity;
                        treatment.Unit = treatmentModel.Unit;
                        treatment.Note = treatmentModel.Note;

                        // Save
                        context.SaveChanges();

                        // Return
                        return StatusCode(HttpStatusCode.NoContent);
                    }

                    // Return
                    return BadRequest("Treatment could not be found");
                }
            }
            catch (Exception ex)
            {
                // Return
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes the specified treatment identifier.
        /// </summary>
        /// <param name="treatmentId">The treatment identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{treatmentId:int}")]
        public async Task<IHttpActionResult> Delete(int treatmentId)
        {
            try
            {
                using (var context = new BeeAppContext())
                {
                    var treatment = await context.Beekeepers
                        .Where(x => x.ApplicationUserId == _applicationUserId)
                        .Include(x => x.Apiaries)
                        .SelectMany(x => x.Apiaries)
                        .Include(x => x.Hives)
                        .SelectMany(x => x.Hives)
                        .Include(x => x.Treatments)
                        .SelectMany(x => x.Treatments)
                        .Where(x => x.Id == treatmentId)
                        .FirstOrDefaultAsync();

                    if (treatment != null)
                    {
                        // Remove
                        context.Treatments.Remove(treatment);

                        // Save
                        context.SaveChanges();

                        // Return
                        return StatusCode(HttpStatusCode.NoContent);
                    }

                    // Return
                    return BadRequest("Treatment could not be found");
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