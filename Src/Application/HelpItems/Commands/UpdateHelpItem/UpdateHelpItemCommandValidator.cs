//using FluentValidation;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace ColabSpace.Application.HelpItems.Commands.UpdateHelpItem
//{
//    public class UpdateHelpItemCommandValidator : AbstractValidator<UpdateHelpItemCommand>
//    {
//        public UpdateHelpItemCommandValidator()
//        {
//            RuleFor(helpItem => helpItem.Name)
//                .NotEmpty()
//                .MaximumLength(20);

//            RuleFor(helpItem => helpItem.Content).NotEmpty();

//            RuleFor(helpItem => helpItem.Description).MaximumLength(500);
//        }
//    }
//}
