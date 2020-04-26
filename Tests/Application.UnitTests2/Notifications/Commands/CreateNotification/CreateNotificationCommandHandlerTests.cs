using ColabSpace.Application.Common.Exceptions;
using ColabSpace.Application.Notifications.Command.CreateNotification;
using ColabSpace.Application.Teams.Models;
using ColabSpace.Application.UnitTests2.Common;
using ColabSpace.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.Application.UnitTests2.Notifications.Commands.CreateNotification
{
    public class CreateNotificationCommandHandlerTests : CommandTestBase
    {
        private readonly Guid userId1 = ColabSpaceDbContextFactory.userId1;
        private readonly Guid userId2 = ColabSpaceDbContextFactory.userId2;
        private readonly Guid conversationId = ColabSpaceDbContextFactory.conversation1;
        private readonly Guid teamId1 = Guid.NewGuid();
        private readonly Guid InvalidConversationId = Guid.NewGuid();

        [Fact]
        public async Task GiveWrongType_ShouldRaiseNotTypeExceptionAsync()
        {
            var sut = new CreateNotificationCommandHandler(_context, _mapper);

            var command = new CreateNotificationCommand()
            {
                Type = "WRONG TYPE"
            };

            await Should.ThrowAsync<NotTypeException>(() =>
               sut.Handle(command, CancellationToken.None));
        }

        /*
         Input:
            TeamId, ToUser, Type:Mention, ConversationId, MessageContent
         Result:
            Create notification with amount = toUserList.count,
                                Title contain:"mentioned you in a channel"
        */
        [Fact]
        public async Task GiveValidRequest_ShouldCreateNotification1()
        {
            var sut = new CreateNotificationCommandHandler(_context, _mapper);
            List<UserModel> toUserList = new List<UserModel>();
            toUserList.AddRange(new[]
            {
                new UserModel
                {
                    DisplayName = "User1",
                    UserId = userId1
                },
                new UserModel
                {
                    DisplayName = "User2",
                    UserId = userId2
                }
            });

            var command = new CreateNotificationCommand()
            {
                Type = "Mention",
                ConversationId = conversationId.ToString(),
                isRead = false,
                MessageContent = "@User -> test",
                MessageId = "",
                TeamId = teamId1.ToString(),
                RegUserId = "",
                ToUser = toUserList,
                ConversationName = "",
                URL = ""
            };

            await sut.Handle(command, CancellationToken.None);

            var result = _context.Notifications.Where(q => q.MessageContent == "@User -> test").ToList();

            result.ShouldNotBeNull();
            result.Count.ShouldBe(toUserList.Count());
            foreach (Notification nfc in result)
            {
                nfc.Type.ShouldBe("Mention");
                nfc.TeamId.ShouldBe(teamId1.ToString());
                nfc.isRead.ShouldBeFalse();
                nfc.Title.ShouldContain("mentioned you in a channel");
            }
        }

        /*
         Input:
            ToUser, Type:Mention, ConversationId, MessageContent
         Result:
            Create notification with amount = toUserList.count,
                                Title contain:"mentioned you in a message"
        */
        [Fact]
        public async Task GiveValidRequest_ShouldCreateNotification2()
        {
            var sut = new CreateNotificationCommandHandler(_context, _mapper);
            List<UserModel> toUserList = new List<UserModel>();
            toUserList.AddRange(new[]
            {
                new UserModel
                {
                    DisplayName = "User1",
                    UserId = userId1
                },
                new UserModel
                {
                    DisplayName = "User2",
                    UserId = userId2
                }
            });

            var command = new CreateNotificationCommand()
            {
                Type = "Mention",
                ConversationId = conversationId.ToString(),
                isRead = false,
                MessageContent = "@User -> test 2",
                MessageId = "",
                RegUserId = "",
                ToUser = toUserList,
                ConversationName = "",
                URL = ""
            };

            await sut.Handle(command, CancellationToken.None);

            var result = _context.Notifications.Where(q => q.MessageContent == "@User -> test 2").ToList();

            result.ShouldNotBeNull();
            result.Count.ShouldBe(toUserList.Count());
            foreach (Notification nfc in result)
            {
                nfc.Type.ShouldBe("Mention");
                nfc.isRead.ShouldBeFalse();
                nfc.Title.ShouldContain("mentioned you in a message");
            }
        }

        /*
          Input:
             ToUser, Type:AddGroup, ConversationId, MessageContent
          Result:
             Create notification with amount = toUserList.count,
                                 Title contain:"added you into a group"
        */
        [Fact]
        public async Task GiveValidRequest_ShouldCreateNotification3()
        {
            var sut = new CreateNotificationCommandHandler(_context, _mapper);
            List<UserModel> toUserList = new List<UserModel>();
            toUserList.AddRange(new[]
            {
                new UserModel
                {
                    DisplayName = "User1",
                    UserId = userId1
                },
                new UserModel
                {
                    DisplayName = "User2",
                    UserId = userId2
                }
            });

            var command = new CreateNotificationCommand()
            {
                Type = "AddGroup",
                ConversationId = conversationId.ToString(),
                isRead = false,
                MessageId = "",
                RegUserId = "",
                RegUserName = "TESTAddGroup",
                ToUser = toUserList,
                ConversationName = "",
                URL = ""
            };

            await sut.Handle(command, CancellationToken.None);

            var result = _context.Notifications.Where(q => q.Title == "TESTAddGroup added you into a group").ToList();

            result.ShouldNotBeNull();
            result.Count.ShouldBe(toUserList.Count());
            foreach (Notification nfc in result)
            {
                nfc.Type.ShouldBe("AddGroup");
                nfc.isRead.ShouldBeFalse();
                nfc.Title.ShouldContain("added you into a group");
            }
        }

        /*
          Input:
             ToUser, Type:Reaction, ConversationId, MessageContent
          Result:
             Create notification with amount = toUserList.count,
                                 Title contain:" added a reaction"
        */
        [Fact]
        public async Task GiveValidRequest_ShouldCreateNotification4()
        {
            var sut = new CreateNotificationCommandHandler(_context, _mapper);
            List<UserModel> toUserList = new List<UserModel>();
            toUserList.AddRange(new[]
            {
                new UserModel
                {
                    DisplayName = "User1",
                    UserId = userId1
                },
                new UserModel
                {
                    DisplayName = "User2",
                    UserId = userId2
                }
            });

            var command = new CreateNotificationCommand()
            {
                Type = "Reaction",
                ConversationId = conversationId.ToString(),
                isRead = false,
                MessageId = "",
                RegUserId = "",
                RegUserName = "TESTReaction",
                ToUser = toUserList,
                ConversationName = "",
                URL = ""
            };

            await sut.Handle(command, CancellationToken.None);

            var result = _context.Notifications.Where(q => q.Title == "TESTReaction added a reaction").ToList();

            result.ShouldNotBeNull();
            result.Count.ShouldBe(toUserList.Count());
            foreach (Notification nfc in result)
            {
                nfc.Type.ShouldBe("Reaction");
                nfc.isRead.ShouldBeFalse();
                nfc.Title.ShouldContain(" added a reaction");
            }
        }
    }
}
