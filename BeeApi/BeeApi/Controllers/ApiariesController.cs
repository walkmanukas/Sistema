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
    /// Represents a Apiaries controller.
    /// </summary>
    /// <seealso cref="BeeApi.Controllers.BaseApiController" />
    [Authorize]
    [RoutePrefix("api/apiary")]
    public class ApiariesController : BaseApiController
    {
        private readonly string _applicationUserId;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiariesController"/> class.
        /// </summary>
        public ApiariesController()
        {
            _applicationUserId = User.Identity.GetUserId();
        }

        /// <summary>
        /// Posts the specified create apiary model.
        /// </summary>
        /// <param name="apiaryModel">The apiary model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Post(ApiaryModel apiaryModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Apiary Model is not valid");
            }

            try
            {
                using (var context = new BeeAppContext())
                {
                    var beekeeper = await context.Beekeepers
                        .FirstOrDefaultAsync(x => x.ApplicationUserId == _applicationUserId);

                    if (beekeeper == null)
                    {
                        return BadRequest("The application user could not be found.");
                    }

                    beekeeper.Apiaries.Add(new Apiary
                    {
                        Name = apiaryModel.Name,
                        Place = apiaryModel.Place,
                        Longtitude = apiaryModel.Longtitude,
                        Latitude = apiaryModel.Latitude
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
            return Ok(apiaryModel);
        }

        /// <summary>
        /// Gets all apiaries of the owner.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            try
            {
                using (var context = new BeeAppContext())
                {
                    var apiaries = context.Beekeepers
                        .Where(x => x.ApplicationUserId == _applicationUserId)
                        .Include(x => x.Apiaries)
                        .SelectMany(x => x.Apiaries)
                        .Select(x => new
                        {
                            x.Id,
                            x.Name,
                            x.Place,
                            x.Longtitude,
                            x.Latitude
                        })
                        .ToArray();

                    // Return
                    return Ok(apiaries);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Gets the specified apiary.
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
                    var apiary = context.Beekeepers
                        .Where(x => x.ApplicationUserId == _applicationUserId)
                        .Include(x => x.Apiaries)
                        .SelectMany(x => x.Apiaries)
                        .Where(x => x.Id == apiaryId)
                        .Select(x => new
                        {
                            x.Id,
                            x.Name,
                            x.Place,
                            x.Longtitude,
                            x.Latitude
                        })
                        .ToArray();

                    // Return
                    return Ok(apiary);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Puts the specified apiary identifier.
        /// </summary>
        /// <param name="apiaryId">The apiary identifier.</param>
        /// <param name="apiaryModel">The apiary model.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{apiaryId:int}")]
        public async Task<IHttpActionResult> Put(int apiaryId, ApiaryModel apiaryModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Apiary Model is not valid");
            }

            try
            {
                using (var context = new BeeAppContext())
                {
                    var apiary = await context.Beekeepers
                        .Where(x => x.ApplicationUserId == _applicationUserId)
                        .Include(x => x.Apiaries)
                        .SelectMany(x => x.Apiaries)
                        .FirstOrDefaultAsync(x => x.Id == apiaryId);

                    if (apiary != null)
                    {
                        apiary.Name = apiaryModel.Name;
                        apiary.Place = apiaryModel.Place;
                        apiary.Longtitude = apiaryModel.Longtitude;
                        apiary.Latitude = apiaryModel.Latitude;

                        // Save
                        context.SaveChanges();

                        // Return
                        return StatusCode(HttpStatusCode.NoContent);
                    }

                    // Return
                    return BadRequest("Apiary could not be found");
                }
            }
            catch (Exception ex)
            {
                // Return
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes the specified apiary identifier.
        /// </summary>
        /// <param name="apiaryId">The apiary identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{apiaryId:int}")]
        public async Task<IHttpActionResult> Delete(int apiaryId)
        {
            try
            {
                using (var context = new BeeAppContext())
                {
                    // Get the apiary
                    var apiary = await context.Beekeepers
                        .Where(x => x.ApplicationUserId == _applicationUserId)
                        .Include(x => x.Apiaries)
                        .SelectMany(x => x.Apiaries)
                        .FirstOrDefaultAsync(x => x.Id == apiaryId);

                    if (apiary != null)
                    {
                        // Remove
                        context.Apiaries.Remove(apiary);

                        // Save
                        context.SaveChanges();

                        // Return
                        return StatusCode(HttpStatusCode.NoContent);
                    }

                    // Return
                    return BadRequest("Apiary could not be found");
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