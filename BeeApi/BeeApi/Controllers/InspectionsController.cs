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
    /// Represents an Inspection controller.
    /// </summary>
    /// <seealso cref="BeeApi.Controllers.BaseApiController" />
    [Authorize]
    [RoutePrefix("api/inspection")]
    public class InspectionsController : BaseApiController
    {
        private readonly string _applicationUserId;
        private readonly Ensurer _ensurer = new Ensurer();

        /// <summary>
        /// Initializes a new instance of the <see cref="InspectionsController"/> class.
        /// </summary>
        public InspectionsController()
        {
            _applicationUserId = User.Identity.GetUserId();
        }

        /// <summary>
        /// Posts the specified apiary identifier.
        /// </summary>
        /// <param name="apiaryId">The apiary identifier.</param>
        /// <param name="hiveId">The hive identifier.</param>
        /// <param name="inspectionModel">The inspection model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{apiaryId:int}/{hiveId:int}")]
        public async Task<IHttpActionResult> Post(int apiaryId, int hiveId, InspectionModel inspectionModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Inspection Model is not valid");
            }

            try
            {
                using (var context = new BeeAppContext())
                {
                    if (await _ensurer.EnsureHiveBelongsToApiary(context, apiaryId, hiveId, _applicationUserId))
                    {
                        context.Inspections.Add(new Inspection
                        {
                            Date = inspectionModel.Date,
                            Name = inspectionModel.Name,
                            Strength = inspectionModel.Strength,
                            Temper = inspectionModel.Temper,
                            Disease = inspectionModel.Disease,
                            FramesBees = inspectionModel.FramesBees,
                            FramesHoney = inspectionModel.FramesHoney,
                            FramesHoneySupers = inspectionModel.FramesHoneySupers,
                            Drones = inspectionModel.Drones,
                            DroneCells = inspectionModel.DroneCells,
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
            return Ok(inspectionModel);
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
                    var inspections = context.Beekeepers
                        .Where(x => x.ApplicationUserId == _applicationUserId)
                        .Include(x => x.Apiaries)
                        .SelectMany(x => x.Apiaries)
                        .Where(x => x.Id == apiaryId)
                        .Include(x => x.Hives)
                        .SelectMany(x => x.Hives)
                        .Where(x => x.Id == hiveId)
                        .Include(x => x.Inspections)
                        .SelectMany(x => x.Inspections)
                        .Select(x => new
                        {
                            x.Id,
                            x.Date,
                            x.Name,
                            x.Strength,
                            x.Temper,
                            x.Disease,
                            x.FramesBees,
                            x.FramesHoney,
                            x.FramesHoneySupers,
                            x.Drones,
                            x.DroneCells
                        })
                        .ToArray();

                    // Return
                    return Ok(inspections);
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
        /// <param name="inspectionId">The inspection identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{apiaryId:int}/{hiveId:int}/{inspectionId:int}")]
        public async Task<IHttpActionResult> Get(int apiaryId, int hiveId, int inspectionId)
        {
            try
            {
                using (var context = new BeeAppContext())
                {
                    var inspection = await context.Beekeepers
                        .Where(x => x.ApplicationUserId == _applicationUserId)
                        .Include(x => x.Apiaries)
                        .SelectMany(x => x.Apiaries)
                        .Where(x => x.Id == apiaryId)
                        .Include(x => x.Hives)
                        .SelectMany(x => x.Hives)
                        .Where(x => x.Id == hiveId)
                        .Include(x => x.Inspections)
                        .SelectMany(x => x.Inspections)
                        .Where(x => x.Id == inspectionId)
                        .Select(x => new
                        {
                            x.Id,
                            x.Date,
                            x.Name,
                            x.Strength,
                            x.Temper,
                            x.Disease,
                            x.FramesBees,
                            x.FramesHoney,
                            x.FramesHoneySupers,
                            x.Drones,
                            x.DroneCells
                        })
                        .FirstOrDefaultAsync();

                    // Return
                    return Ok(inspection);
                }
            }
            catch (Exception ex)
            {
                // Return
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Puts the specified inspection identifier.
        /// </summary>
        /// <param name="inspectionId">The inspection identifier.</param>
        /// <param name="inspectionModel">The inspection model.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{inspectionId:int}")]
        public async Task<IHttpActionResult> Put(int inspectionId, InspectionModel inspectionModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Inspection Model is not valid");
            }

            try
            {
                using (var context = new BeeAppContext())
                {
                    var inspection = await context.Beekeepers
                        .Where(x => x.ApplicationUserId == _applicationUserId)
                        .Include(x => x.Apiaries)
                        .SelectMany(x => x.Apiaries)
                        .Include(x => x.Hives)
                        .SelectMany(x => x.Hives)
                        .Include(x => x.Inspections)
                        .SelectMany(x => x.Inspections)
                        .Where(x => x.Id == inspectionId)
                        .FirstOrDefaultAsync();

                    if (inspection != null)
                    {
                        inspection.Name = inspectionModel.Name;
                        inspection.Date = inspectionModel.Date;
                        inspection.Strength = inspectionModel.Strength;
                        inspection.Temper = inspectionModel.Temper;
                        inspection.Disease = inspectionModel.Disease;
                        inspection.FramesBees = inspectionModel.FramesBees;
                        inspection.FramesHoney = inspectionModel.FramesHoney;
                        inspection.FramesHoneySupers = inspectionModel.FramesHoneySupers;
                        inspection.Drones = inspectionModel.Drones;
                        inspection.DroneCells = inspectionModel.DroneCells;

                        // Save
                        context.SaveChanges();

                        // Return
                        return StatusCode(HttpStatusCode.NoContent);
                    }

                    // Return
                    return BadRequest("Inspection could not be found");
                }
            }
            catch (Exception ex)
            {
                // Return
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes the specified inspection identifier.
        /// </summary>
        /// <param name="inspectionId">The inspection identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{inspectionId:int}")]
        public async Task<IHttpActionResult> Delete(int inspectionId)
        {
            try
            {
                using (var context = new BeeAppContext())
                {
                    var inspection = await context.Beekeepers
                        .Where(x => x.ApplicationUserId == _applicationUserId)
                        .Include(x => x.Apiaries)
                        .SelectMany(x => x.Apiaries)
                        .Include(x => x.Hives)
                        .SelectMany(x => x.Hives)
                        .Include(x => x.Inspections)
                        .SelectMany(x => x.Inspections)
                        .Where(x => x.Id == inspectionId)
                        .FirstOrDefaultAsync();

                    if (inspection != null)
                    {
                        // Remove
                        context.Inspections.Remove(inspection);

                        // Save
                        context.SaveChanges();

                        // Return
                        return StatusCode(HttpStatusCode.NoContent);
                    }

                    // Return
                    return BadRequest("Inspection could not be found");
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