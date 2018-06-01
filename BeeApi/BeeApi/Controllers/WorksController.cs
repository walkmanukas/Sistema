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
    /// Represents a Works controller.
    /// </summary>
    /// <seealso cref="BeeApi.Controllers.BaseApiController" />
    [Authorize]
    [RoutePrefix("api/work")]
    public class WorksController : BaseApiController
    {
        private readonly string _applicationUserId;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorksController"/> class.
        /// </summary>
        public WorksController()
        {
            _applicationUserId = User.Identity.GetUserId();
        }

        /// <summary>
        /// Posts the specified create work model.
        /// </summary>
        /// <param name="workModel">The work model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Post(WorkModel workModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Work Model is not valid");
            }

            try
            {
                using (var context = new BeeAppContext())
                {
                    var beekeeper = await context.Beekeepers
                        .FirstOrDefaultAsync(x => x.ApplicationUserId == _applicationUserId);

                    if (beekeeper == null)
                    {
                        return BadRequest();
                    }

                    // Add work to a beekeeper
                    beekeeper.Works.Add(new Work
                    {
                        Name = workModel.Name,
                        Date = workModel.Date,
                        Note = workModel.Note,
                        IsCompleted = workModel.IsCompleted
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
            return Ok(workModel);
        }

        /// <summary>
        /// Gets all works.
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
                    var works = context.Beekeepers
                        .Where(x => x.ApplicationUserId == _applicationUserId)
                        .Include(x => x.Works)
                        .SelectMany(x => x.Works)
                        .Select(x => new
                        {
                            x.Id,
                            x.Name,
                            x.Date,
                            x.Note,
                            x.IsCompleted
                        })
                        .ToArray();

                    // Return
                    return Ok(works);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Gets the specified work.
        /// </summary>
        /// <param name="workId">The work identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{workId:int}")]
        public IHttpActionResult Get(int workId)
        {
            try
            {
                using (var context = new BeeAppContext())
                {
                    var work = context.Beekeepers
                        .Where(x => x.ApplicationUserId == _applicationUserId)
                        .Include(x => x.Works)
                        .SelectMany(x => x.Works)
                        .Where(x => x.Id == workId)
                        .Select(x => new
                        {
                            x.Id,
                            x.Name,
                            x.Date,
                            x.Note,
                            x.IsCompleted
                        })
                        .ToArray();

                    // Return
                    return Ok(work);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Puts the specified work identifier.
        /// </summary>
        /// <param name="workId">The work identifier.</param>
        /// <param name="workModel">The work model.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{workId:int}")]
        public async Task<IHttpActionResult> Put(int workId, WorkModel workModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Work Model is not valid");
            }

            try
            {
                using (var context = new BeeAppContext())
                {
                    var work = await context.Beekeepers
                        .Where(x => x.ApplicationUserId == _applicationUserId)
                        .Include(x => x.Works)
                        .SelectMany(x => x.Works)
                        .FirstOrDefaultAsync(x => x.Id == workId);

                    if (work != null)
                    {
                        work.Name = workModel.Name;
                        work.Date = workModel.Date;
                        work.Note = workModel.Note;
                        work.IsCompleted = workModel.IsCompleted;

                        // Save
                        context.SaveChanges();

                        // Return
                        return StatusCode(HttpStatusCode.NoContent);
                    }

                    // Return
                    return BadRequest("Work could not be found");
                }
            }
            catch (Exception ex)
            {
                // Return
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes the specified work identifier.
        /// </summary>
        /// <param name="workId">The work identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{workId:int}")]
        public async Task<IHttpActionResult> Delete(int workId)
        {
            try
            {
                using (var context = new BeeAppContext())
                {
                    var work = await context.Beekeepers
                        .Where(x => x.ApplicationUserId == _applicationUserId)
                        .Include(x => x.Works)
                        .SelectMany(x => x.Works)
                        .Where(x => x.Id == workId)
                        .FirstOrDefaultAsync();

                    if (work != null)
                    {
                        // Remove
                        context.Works.Remove(work);

                        // Save
                        context.SaveChanges();

                        // Return
                        return StatusCode(HttpStatusCode.NoContent);
                    }

                    // Return
                    return BadRequest("Work could not be found");
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