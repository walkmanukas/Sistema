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
    /// Represents a Queens controller.
    /// </summary>
    /// <seealso cref="BeeApi.Controllers.BaseApiController" />
    [Authorize]
    [RoutePrefix("api/queen")]
    public class QueensController : BaseApiController
    {
        private readonly string _applicationUserId;
        private readonly Ensurer _ensurer = new Ensurer();

        /// <summary>
        /// Initializes a new instance of the <see cref="QueensController" /> class.
        /// </summary>
        public QueensController()
        {
            _applicationUserId = User.Identity.GetUserId();
        }

        /// <summary>
        /// Posts the specified apiary identifier.
        /// </summary>
        /// <param name="apiaryId">The apiary identifier.</param>
        /// <param name="hiveId">The hive identifier.</param>
        /// <param name="queenModel">The queen model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{apiaryId:int}/{hiveId:int}")]
        public async Task<IHttpActionResult> Post(int apiaryId, int hiveId, QueenModel queenModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Queen Model is not valid");
            }

            try
            {
                using (var context = new BeeAppContext())
                {
                    if (await _ensurer.EnsureHiveBelongsToApiary(context, apiaryId, hiveId, _applicationUserId))
                    {
                        context.Queens.Add(new Queen
                        {
                            Name = queenModel.Name,
                            Date = queenModel.Date,
                            Breed = queenModel.Breed,
                            Colour = queenModel.Colour,
                            State = queenModel.State,
                            Status = queenModel.Status,
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
            return Ok(queenModel);
        }

        /// <summary>
        /// Gets this instance.
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
                    var queens = context.Beekeepers
                        .Where(x => x.ApplicationUserId == _applicationUserId)
                        .Include(x => x.Apiaries)
                        .SelectMany(x => x.Apiaries)
                        .Where(x => x.Id == apiaryId)
                        .Include(x => x.Hives)
                        .SelectMany(x => x.Hives)
                        .Where(x => x.Id == hiveId)
                        .Include(x => x.Queens)
                        .SelectMany(x => x.Queens)
                        .Select(x => new
                        {
                            x.Id,
                            x.Name,
                            x.Date,
                            x.Breed,
                            x.Colour,
                            x.State,
                            x.Status
                        })
                        .ToArray();

                    // Return
                    return Ok(queens);
                }
            }
            catch (Exception ex)
            {
                // Return
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <param name="apiaryId">The apiary identifier.</param>
        /// <param name="hiveId">The hive identifier.</param>
        /// <param name="queenId">The queen identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{apiaryId:int}/{hiveId:int}/{queenId:int}")]
        public async Task<IHttpActionResult> Get(int apiaryId, int hiveId, int queenId)
        {
            try
            {
                using (var context = new BeeAppContext())
                {
                    var queen = await context.Beekeepers
                        .Where(x => x.ApplicationUserId == _applicationUserId)
                        .Include(x => x.Apiaries)
                        .SelectMany(x => x.Apiaries)
                        .Where(x => x.Id == apiaryId)
                        .Include(x => x.Hives)
                        .SelectMany(x => x.Hives)
                        .Where(x => x.Id == hiveId)
                        .Include(x => x.Queens)
                        .SelectMany(x => x.Queens)
                        .Where(x => x.Id == queenId)
                        .Select(x => new
                        {
                            x.Id,
                            x.Name,
                            x.Date,
                            x.Breed,
                            x.Colour,
                            x.State,
                            x.Status
                        })
                        .FirstOrDefaultAsync();

                    // Return
                    return Ok(queen);
                }
            }
            catch (Exception ex)
            {
                // Return
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Puts the specified queen identifier.
        /// </summary>
        /// <param name="queenId">The queen identifier.</param>
        /// <param name="queenModel">The queen model.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{queenId:int}")]
        public async Task<IHttpActionResult> Put(int queenId, QueenModel queenModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Queen Model is not valid");
            }

            try
            {
                using (var context = new BeeAppContext())
                {
                    var queen = await context.Beekeepers
                        .Where(x => x.ApplicationUserId == _applicationUserId)
                        .Include(x => x.Apiaries)
                        .SelectMany(x => x.Apiaries)
                        .Include(x => x.Hives)
                        .SelectMany(x => x.Hives)
                        .Include(x => x.Queens)
                        .SelectMany(x => x.Queens)
                        .Where(x => x.Id == queenId)
                        .FirstOrDefaultAsync();

                    if (queen != null)
                    {
                        queen.Name = queenModel.Name;
                        queen.Date = queenModel.Date;
                        queen.Breed = queenModel.Breed;
                        queen.Colour = queenModel.Colour;
                        queen.State = queenModel.State;
                        queen.Status = queenModel.Status;

                        // Save
                        context.SaveChanges();

                        // Return
                        return StatusCode(HttpStatusCode.NoContent);
                    }

                    // Return
                    return BadRequest("Queen could not be found");
                }
            }
            catch (Exception ex)
            {
                // Return
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes the specified queen identifier.
        /// </summary>
        /// <param name="queenId">The queen identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{queenId:int}")]
        public async Task<IHttpActionResult> Delete(int queenId)
        {
            try
            {
                using (var context = new BeeAppContext())
                {
                    var queen = await context.Beekeepers
                        .Where(x => x.ApplicationUserId == _applicationUserId)
                        .Include(x => x.Apiaries)
                        .SelectMany(x => x.Apiaries)
                        .Include(x => x.Hives)
                        .SelectMany(x => x.Hives)
                        .Include(x => x.Queens)
                        .SelectMany(x => x.Queens)
                        .Where(x => x.Id == queenId)
                        .FirstOrDefaultAsync();

                    if (queen != null)
                    {
                        // Remove
                        context.Queens.Remove(queen);

                        // Save
                        context.SaveChanges();

                        // Return
                        return StatusCode(HttpStatusCode.NoContent);
                    }

                    // Return
                    return BadRequest("Queen could not be found");
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