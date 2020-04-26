using AutoMapper;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.TaskItems.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ColabSpace.Application.TaskItems.Queries.GetPinnedTaskItemList
{
    public class GetPinnedTaskItemListQueryHandler : IRequestHandler<GetPinnedTaskItemListQuery, IEnumerable<TaskItemModel>>
    {
        private readonly IColabSpaceDbContext _context;
        private readonly IMapper _mapper;

        private readonly int pageSize = 30;

        public GetPinnedTaskItemListQueryHandler(IColabSpaceDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TaskItemModel>> Handle(GetPinnedTaskItemListQuery request, CancellationToken cancellationToken)
        {
            // kiem tra co truyen pageIndex hay khong
            request.PageIndex ??= 1;

            // lay danh sach cac task duoc pin cua team
            var lstPinnedTaskItem = await _context.TaskItems
                .Where(taskItem => taskItem.TeamId == request.TeamId && taskItem.IsPin == true && taskItem.Status != 3)
                // sap xep theo ngay duoc pin giam dan
                .OrderByDescending(taskItem => taskItem.PinnedDate)
                // phan trang
                .Skip(((int)request.PageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return _mapper.Map<List<TaskItemModel>>(lstPinnedTaskItem);
        }
    }
}
