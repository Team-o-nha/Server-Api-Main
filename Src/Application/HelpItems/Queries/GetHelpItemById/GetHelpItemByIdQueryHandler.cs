//using AutoMapper;
//using ColabSpace.Application.Common.Exceptions;
//using ColabSpace.Application.Common.Interfaces;
//using ColabSpace.Application.HelpItems.Models;
//using ColabSpace.Domain.Entities;
//using MediatR;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace ColabSpace.Application.HelpItems.Queries.GetHelpItemById
//{
//    public class GetHelpItemByIdQueryHandler : IRequestHandler<GetHelpItemByIdQuery, HelpItemModel>
//    {
//        private readonly IColabSpaceDbContext _context;
//        private readonly IMapper _mapper;

//        public GetHelpItemByIdQueryHandler(IColabSpaceDbContext context, IMapper mapper)
//        {
//            _context = context;
//            _mapper = mapper;
//        }

//        public async Task<HelpItemModel> Handle(GetHelpItemByIdQuery request, CancellationToken cancellationToken)
//        {
//            var helpItem = await _context.HelpItems.FindAsync(request.HelpItemId);

//            if (helpItem == null)
//            {
//                throw new NotFoundException(nameof(HelpItem), request.HelpItemId);
//            }

//            return _mapper.Map<HelpItem, HelpItemModel>(helpItem);
//        }
//    }
//}
