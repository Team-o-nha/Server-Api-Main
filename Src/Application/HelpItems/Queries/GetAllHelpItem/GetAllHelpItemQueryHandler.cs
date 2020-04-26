//using AutoMapper;
//using ColabSpace.Application.Common.Interfaces;
//using ColabSpace.Application.HelpItems.Models;
//using MediatR;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace ColabSpace.Application.HelpItems.Queries.GetAllHelpItem
//{
//    public class GetAllHelpItemQueryHandler : IRequestHandler<GetAllHelpItemQuery, IEnumerable<HelpItemModel>>
//    {
//        private readonly IColabSpaceDbContext _context;
//        private readonly IMapper _mapper;

//        public GetAllHelpItemQueryHandler(IColabSpaceDbContext context, IMapper mapper)
//        {
//            _context = context;
//            _mapper = mapper;
//        }

//        public async Task<IEnumerable<HelpItemModel>> Handle(GetAllHelpItemQuery request, CancellationToken cancellationToken)
//        {
//            return _mapper.Map<List<HelpItemModel>>(_context.HelpItems);
//        }
//    }
//}
