using AutoMapper;
using ColabSpace.Application.Common.Exceptions;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.Teams.Models;
using ColabSpace.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ColabSpace.Application.Teams.Queries.GetTeam
{
    public class GetTeamQueryHandler : IRequestHandler<GetTeamQuery, TeamModel>
    {
        private readonly IColabSpaceDbContext _context;
        private readonly IMapper _mapper;

        public GetTeamQueryHandler(IColabSpaceDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TeamModel> Handle(GetTeamQuery request, CancellationToken cancellationToken)
        {
            var teams = await _context.Teams.FindAsync(request.TeamId);
            if (teams == null)
            {
                throw new NotFoundException(nameof(Team), request.TeamId);
            }

            return _mapper.Map<Team, TeamModel>(teams);
        }
    }
}
