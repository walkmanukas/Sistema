using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using BeeApi.Contexts;
using Microsoft.AspNet.Identity;

namespace BeeApi.Controllers
{
    /// <summary>
    /// Represents a Monitoring Controller.
    /// </summary>
    /// <seealso cref="BeeApi.Controllers.BaseApiController" />
    [Authorize]
    [RoutePrefix("api/monitoring")]
    public class MonitoringController : BaseApiController
    {
        private readonly string _applicationUserId;

        /// <summary>
        /// Initializes a new instance of the <see cref="MonitoringController"/> class.
        /// </summary>
        public MonitoringController()
        {
            _applicationUserId = User.Identity.GetUserId();
        }

        /// <summary>
        /// Gets the specified hive identifier.
        /// </summary>
        /// <param name="hiveId">The hive identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{hiveId:int}")]
        public IHttpActionResult Get(int hiveId)
        {
            try
            {
                using (var context = new BeeAppContext())
                {
                    var monitoring = context.Beekeepers
                        .Where(x => x.ApplicationUserId == _applicationUserId)
                        .Include(x => x.Apiaries)
                        .SelectMany(x => x.Apiaries)
                        .Include(x => x.Hives)
                        .SelectMany(x => x.Hives)
                        .Where(x => x.Id == hiveId)
                        .Include(x => x.Monitoring)
                        .SelectMany(x => x.Monitoring)
                        .Select(x => new
                        {
                            x.Id,
                            x.Timestamp,
                            x.Temperature,
                            x.Humidity,
                            x.Longtitude,
                            x.Latitude
                        })
                        .ToArray();

                    // Return
                    return Ok(monitoring);
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