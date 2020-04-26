//using FluentValidation;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace ColabSpace.Application.HelpItems.Commands.CreateHelpItem
//{
//    public class CreateHelpItemCommandValidator : AbstractValidator<CreateHelpItemCommand>
//    {
//        public CreateHelpItemCommandValidator()
//        {
//            RuleFor(helpItem => helpItem.Name)
//                .NotEmpty()
//                .MaximumLength(20);

//            RuleFor(helpItem => helpItem.Content).NotNull();
//            When(helpItem => helpItem.Content != null, () =>
//            {
//                RuleFor(helpItem => helpItem.Content).SetValidator(new ContentValidator());
//            });

//            RuleFor(helpItem => helpItem.Description).MaximumLength(500);
//        }
//    }
//}
