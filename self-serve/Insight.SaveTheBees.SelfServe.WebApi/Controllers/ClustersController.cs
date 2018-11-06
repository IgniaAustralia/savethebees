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
    [Route("api/users/{userId:Guid}/clusters")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]

    public class ClustersController : BaseController
    {
        #region Members

        private readonly IMapper _mapper;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a new instance of <see cref="ClustersController" /> and
        /// initialises all internal components.
        /// </summary>
        /// <param name="context">The application DB context.</param>
        /// <param name="mapper">The mapper instance.</param>
        public ClustersController(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            Context = context;
            _mapper = mapper;
        }

        #endregion

        #region End Points

        /// <summary>
        /// Creates the new cluster.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="cluster">The cluster to be created.</param>
        /// <returns>Action result for the end point.</returns>
        [HttpPut]
        public async Task<IActionResult> CreateCluster([FromRoute]Guid userId, [FromBody]ClusterDto cluster)
        {
            // Validate permission
            if (!await ValidatePermission(userId)) return Unauthorized();

            // Create the cluster
            await AddClusterToDatabase(userId, cluster);
            return NoContent();
        }

        /// <summary>
        /// Retrieves the clusters.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>Action result for the end point.</returns>
        [HttpGet]
        public async Task<IActionResult> RetrieveClusters([FromRoute]Guid userId)
        {
            // Validate permission
            if (!await ValidatePermission(userId)) return Unauthorized();

            // Retrieve the clusters
            var clusters = await RetrieveClustersFromDatabase(userId);
            if (!clusters.Any()) return NotFound();
            return new JsonResult(clusters);
        }

        /// <summary>
        /// Retrieves the cluster.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="clusterId">The cluster id.</param>
        /// <returns>Action result for the end point.</returns>
        [HttpGet("{clusterId:Guid}")]
        public async Task<IActionResult> RetrieveCluster([FromRoute]Guid userId, [FromRoute]Guid clusterId)
        {
            // Validate permission
            if (!await ValidatePermission(userId)) return Unauthorized();

            // Retrieve the cluster
            var cluster = await RetrieveClusterFromDatabase(clusterId);
            if (cluster == null) return NotFound();
            return new JsonResult(cluster);
        }

        /// <summary>
        /// Updates the cluster.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="clusterId">The cluster id.</param>
        /// <returns>Action result for the end point.</returns>
        [HttpPost("{clusterId:Guid}")]
        public async Task<IActionResult> UpdateCluster([FromRoute]Guid userId, [FromRoute]Guid clusterId, [FromBody]ClusterDto cluster)
        {
            // Validate permission
            if (!await ValidatePermission(userId)) return Unauthorized();

            // Check if the cluster exists
            try
            {
                await UpdateClusterInDatabase(clusterId, cluster);
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
        /// Deletes the cluster.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="clusterId">The cluster id.</param>
        /// <returns>Action result for the end point.</returns>
        [HttpDelete("{clusterId:Guid}")]
        public async Task<IActionResult> DeleteCluster([FromRoute]Guid userId, [FromRoute]Guid clusterId)
        {
            // Validate permission
            if (!await ValidatePermission(userId)) return Unauthorized();

            // Check if the cluster exists
            try
            {
                await DeleteClusterFromDatabase(clusterId);
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

        private async Task AddClusterToDatabase(Guid userId, ClusterDto cluster)
        {
            var hiveCluster = _mapper.Map<HiveCluster>(cluster);
            hiveCluster.UserId = userId;
            await Context.Clusters.AddAsync(hiveCluster);
            await Context.SaveChangesAsync();
        }

        private async Task<IList<ClusterOutputDto>> RetrieveClustersFromDatabase(Guid userId)
        {
            var clusters = await Context.Clusters.Where(x => x.UserId == userId).ToListAsync();
            return _mapper.Map<IList<ClusterOutputDto>>(clusters);
        }

        private async Task<ClusterOutputDto> RetrieveClusterFromDatabase(Guid clusterId)
        {
            var cluster = await Context.Clusters.FirstOrDefaultAsync(x => x.ClusterId == clusterId);
            return _mapper.Map<ClusterOutputDto>(cluster);
        }

        private async Task UpdateClusterInDatabase(Guid clusterId, ClusterDto cluster)
        {
            var currentCluster = await Context.Clusters.FirstOrDefaultAsync(x => x.ClusterId == clusterId);
            if (currentCluster == null) throw new NotFoundException();

            // Update the values and save into the database
            currentCluster.Name = cluster.Name;
            await Context.SaveChangesAsync();
        }

        private async Task DeleteClusterFromDatabase(Guid clusterId)
        {
            var currentCluster = await Context.Clusters.FirstOrDefaultAsync(x => x.ClusterId == clusterId);
            if (currentCluster == null) throw new NotFoundException();

            // Delete the cluster from database
            Context.Clusters.Remove(currentCluster);
            await Context.SaveChangesAsync();
        }

        #endregion
    }
}