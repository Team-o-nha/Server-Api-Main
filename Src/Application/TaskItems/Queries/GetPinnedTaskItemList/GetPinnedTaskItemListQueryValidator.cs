using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ColabSpace.Application.TaskItems.Queries.GetPinnedTaskItemList
{
    public class GetPinnedTaskItemListQueryValidator : AbstractValidator<GetPinnedTaskItemListQuery>
    {
        public GetPinnedTaskItemListQueryValidator()
        {
            RuleFor(query => query.TeamId).NotEqual(Guid.Empty);
        }
    }
}
