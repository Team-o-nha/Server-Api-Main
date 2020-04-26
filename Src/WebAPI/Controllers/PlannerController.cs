using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.Planners.Commands;
using ColabSpace.Application.Planners.Commands.Update;
using ColabSpace.Application.Planners.Commands.DeletePlanner;
using ColabSpace.Application.Planners.Models;
using ColabSpace.Application.Planners.Queries.GetPlanner;
using ColabSpace.Application.Planners.Queries.GetPlannersList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ColabSpace.WebAPI.Controllers
{
    [Authorize]
    public class PlannerController : ApiController
    {
        private readonly ICurrentUserService _currentUserService;

        public PlannerController(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        [HttpPost]
        public async Task<ActionResult<PlannerModel>> CreatePlanner(CreatePlannerCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpGet("GetAll/{teamId}")]
        public async Task<IEnumerable<PlannerModel>> GetAll(Guid teamId, [FromQuery] int? pageIndex, [FromQuery] string keyword)
        {
            return await Mediator.Send(new GetPlannersListQuery { TeamId = teamId, PageIndex = pageIndex ?? 1, Keyword = keyword });
        }

        [HttpGet("GetById/{plannerId}")]
        public async Task<PlannerModel> GetPlannerById(Guid plannerId)
        {
            return await Mediator.Send(new GetPlannerQuery { PlannerId = plannerId });
        }

        [HttpPut]
        public async Task<ActionResult> UpdatePlanner(UpdatePlannerCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<PlannerModel>> DeletePlanner(Guid id)
        {
            await Mediator.Send(new DeletePlannerCommand { Id = id });

            return Ok();
        }
    }
}
