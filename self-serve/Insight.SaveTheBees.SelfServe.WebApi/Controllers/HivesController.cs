using AutoMapper;
using Insight.SaveTheBees.SelfServe.WebApi.Exceptions;
using Insight.SaveTheBees.SelfServe.WebApi.Models.Application;
using Insight.SaveTheBees.SelfServe.WebApi.Models.Application.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Insight.SaveTheBees.SelfServe.WebApi.Controllers
{
    /// <summary>
    /// This controller class contains all the end points to manage hive clusters.
    /// </summary>
    [Route("api/users/{userId:Guid}/hives")]
    [Route("api/users/{userId:Guid}/clusters/{clusterId:Guid}/hives")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]

    public class HivesController : BaseController
    {
        #region Members

        private readonly IMapper _mapper;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a new instance of <see cref="HivesController" /> and initialises
        /// all internal components.
        /// </summary>
        /// <param name="context">The application DB context.</param>
        /// <param name="mapper">The mapper instance.</param>
        public HivesController(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            Context = context;
            _mapper = mapper;
        }

        #endregion

        #region End Points

        /// <summary>
        /// Creates the new hive.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="clusterId">The cluster id.</param>
        /// <param name="hive">The hive to be created.</param>
        /// <returns>Action result for the end point.</returns>
        [HttpPut]
        public async Task<IActionResult> CreateHive([FromRoute]Guid userId, [FromRoute]Guid? clusterId, [FromBody]HiveDto hive)
        {
            // Validate permission
            if (!await ValidatePermission(userId)) return Unauthorized();

            // Create the hive
            await AddHiveToDatabase(userId, clusterId, hive);
            return NoContent();
        }

        /// <summary>
        /// Retrieves the hives.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="clusterId">The cluster id.</param>
        /// <returns>Action result for the end point.</returns>
        [HttpGet]
        public async Task<IActionResult> RetrieveHives([FromRoute]Guid userId, [FromRoute]Guid? clusterId)
        {
            // Validate permission
            if (!await ValidatePermission(userId)) return Unauthorized();

            // Retrieve the clusters
            var hives = await RetrieveHivesFromDatabase(userId, clusterId);
            if (!hives.Any()) return NotFound();
            return new JsonResult(hives);
        }

        /// <summary>
        /// Retrieves the hive.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="clusterId">The cluster id.</param>
        /// <param name="hiveId">The hive id.</param>
        /// <returns>Action result for the end point.</returns>
        [HttpGet("{hiveId:Guid}")]
        public async Task<IActionResult> RetrieveHive([FromRoute]Guid userId, [FromRoute]Guid? clusterId, [FromRoute]Guid hiveId)
        {
            // Validate permission
            if (!await ValidatePermission(userId)) return Unauthorized();

            // Retrieve the cluster
            var cluster = await RetrieveHiveFromDatabase(clusterId, hiveId);
            if (cluster == null) return NotFound();
            return new JsonResult(cluster);
        }

        /// <summary>
        /// Updates the hive.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="clusterId">The cluster id.</param>
        /// <param name="hiveId">The hive id.</param>
        /// <param name="hive">The hive to update.</param>
        /// <returns>Action result for the end point.</returns>
        [HttpPost("{hiveId:Guid}")]
        public async Task<IActionResult> UpdateHive([FromRoute]Guid userId, [FromRoute]Guid? clusterId, [FromRoute]Guid hiveId, [FromBody]HiveDto hive)
        {
            // Validate permission
            if (!await ValidatePermission(userId)) return Unauthorized();

            // Check if the cluster exists
            try
            {
                await UpdateHiveInDatabase(hiveId, clusterId, hive);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.Message);
            }

            return NoContent();
        }

        /// <summary>
        /// Deletes the hive.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="clusterId">The cluster id.</param>
        /// <param name="hiveId">The hive id.</param>
        /// <returns>Action result for the end point.</returns>
        [HttpDelete("{hiveId:Guid}")]
        public async Task<IActionResult> DeleteCluster([FromRoute]Guid userId, [FromRoute]Guid? clusterId, [FromRoute]Guid hiveId)
        {
            // Validate permission
            if (!await ValidatePermission(userId)) return Unauthorized();

            // Check if the cluster exists
            try
            {
                await DeleteHiveFromDatabase(clusterId, hiveId);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.Message);
            }

            return NoContent();
        }

        #endregion

        #region Methods

        private async Task AddHiveToDatabase(Guid userId, Guid? clusterId, HiveDto hive)
        {
            var newHive = _mapper.Map<Hive>(hive);
            newHive.UserId = userId;
            newHive.ClusterId = clusterId;
            await Context.Hives.AddAsync(newHive);
            await Context.SaveChangesAsync();
        }

        private async Task<IList<HiveOutputDto>> RetrieveHivesFromDatabase(Guid userId, Guid? clusterId)
        {
            var hives = clusterId.HasValue
                ? await Context.Hives.Where(x => x.UserId == userId && x.ClusterId == clusterId.Value).ToListAsync()
                : await Context.Hives.Where(x => x.UserId == userId).ToListAsync();
            return _mapper.Map<IList<HiveOutputDto>>(hives);
        }

        private async Task<HiveOutputDto> RetrieveHiveFromDatabase(Guid? clusterId, Guid hiveId)
        {
            var hive = clusterId.HasValue
                ? await Context.Hives.FirstOrDefaultAsync(x => x.ClusterId == clusterId.Value && x.HiveId == hiveId)
                : await Context.Hives.FirstOrDefaultAsync(x => x.HiveId == hiveId);
            return _mapper.Map<HiveOutputDto>(hive);
        }

        private async Task UpdateHiveInDatabase(Guid hiveId, Guid? clusterId, HiveDto hive)
        {
            var currentHive = await Context.Hives.FirstOrDefaultAsync(x => x.HiveId == hiveId);
            if (currentHive == null) throw new NotFoundException();

            // Update the values and save into the database
            currentHive.Name = hive.Name;
            currentHive.ClusterId = clusterId;
            await Context.SaveChangesAsync();
        }

        private async Task DeleteHiveFromDatabase(Guid? clusterId, Guid hiveId)
        {
            var currentHive = clusterId.HasValue
                ? await Context.Hives.FirstOrDefaultAsync(x => x.ClusterId == clusterId.Value && x.HiveId == hiveId)
                : await Context.Hives.FirstOrDefaultAsync(x => x.HiveId == hiveId);
            if (currentHive == null) throw new NotFoundException();

            // Delete the cluster from database
            Context.Hives.Remove(currentHive);
            await Context.SaveChangesAsync();
        }

        #endregion
    }
}