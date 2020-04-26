using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Domain.Entities;
using ColabSpace.Domain.Interfaces;
using ColabSpace.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;

namespace ColabSpace.Application.UnitTests2.Common
{
    public static class ColabSpaceDbContextFactory
    {
        public static Guid teamId1 = Guid.NewGuid();
        public static Guid teamId2 = Guid.NewGuid();
        public static Guid teamId3 = Guid.NewGuid();
        public static Guid teamId4 = Guid.NewGuid();
        public static Guid taskItemId1 = Guid.NewGuid();
        public static Guid taskItemId2 = Guid.NewGuid();
        public static Guid taskItemId3 = Guid.NewGuid();
        public static Guid taskItemId4 = Guid.NewGuid();
        public static Guid userId1 = Guid.NewGuid();
        public static Guid userId2 = Guid.NewGuid();
        public static Guid userId3 = Guid.NewGuid();
        public static Guid userId4 = Guid.NewGuid();
        public static Guid conversation1 = Guid.NewGuid();
        public static Guid conversation2 = Guid.NewGuid();
        public static Guid conversation3 = Guid.NewGuid();
        public static Guid messageId1 = Guid.NewGuid();
        public static Guid messageId2 = Guid.NewGuid();
        public static Guid messageId3 = Guid.NewGuid();
        public static Guid messageId4 = Guid.NewGuid();
        public static Guid notificationId1 = Guid.NewGuid();
        public static Guid notificationId2 = Guid.NewGuid();
        public static Guid notificationId3 = Guid.NewGuid();
        public static Guid notificationId4 = Guid.NewGuid();
        public static Guid channelId1 = Guid.NewGuid();
        public static Guid channelId2 = Guid.NewGuid();
        public static Guid channelId3 = Guid.NewGuid();
        public static Guid channelId4 = Guid.NewGuid();
        public static Guid channelId5 = Guid.NewGuid();
        public static Guid channelId6 = Guid.NewGuid();
        public static string channel1Name = "Channel 1 Name";
        public static string channel2Name = "Channel 2 Name";
        public static string channel3Name = "Channel 3 Name";
        public static string channel4Name = "Channel 4 Name";
        public static string channel5Name = "General";
        public static string channel6Name = "General";
        public static Guid messagechannelId1 = Guid.NewGuid();
        public static Guid messagechannelId2 = Guid.NewGuid();
        public static Guid messagechannelId3 = Guid.NewGuid();
        public static Guid messagechannelId4 = Guid.NewGuid();
        public static Guid relatedMessageId1 = Guid.NewGuid();
        public static Guid relatedMessageId2 = Guid.NewGuid();
        public static Guid relatedMessageId3 = Guid.NewGuid();
        public static string validBlobStorageUrl = "some-url/kxg5qfz4.jxl";
        public static Guid plannerId1 = Guid.NewGuid();
        public static Guid plannerId2 = Guid.NewGuid();
        public static Guid plannerId3 = Guid.NewGuid();

        private static readonly string privateKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        private static readonly string databaseName = "ColabSpaceDb";

        public static ColabSpaceDbContext Create()
        {
            var endPoint = "https://localhost:8081";

            // kiem tra bien moi truong, khi test bang DevOps pipeline se override lai bien moi truong nay
            string endPointEnvironment = Environment.GetEnvironmentVariable("CosmosDBEndpoint", EnvironmentVariableTarget.Process);
            Console.WriteLine($"CosmosDBEndpoint from pipeline {endPointEnvironment}");

            if (!string.IsNullOrWhiteSpace(endPointEnvironment))
            {
                endPoint = endPointEnvironment;
            }

            var options = new DbContextOptionsBuilder<ColabSpaceDbContext>()
                .UseCosmos(endPoint, privateKey, databaseName)
                //.UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var dateTimeMock = new Mock<IDateTime>();
            dateTimeMock.Setup(m => m.Now)
                .Returns(new DateTime(3001, 1, 1));

            var currentUserServiceMock = new Mock<ICurrentUserService>();
            currentUserServiceMock.Setup(m => m.UserId)
                .Returns("00000000-0000-0000-0000-000000000000");

            var context = new ColabSpaceDbContext(options,
                currentUserServiceMock.Object, dateTimeMock.Object);

            context.Database.EnsureCreated();

            SeedSampleData(context);

            return context;
        }

        public static void SeedSampleData(ColabSpaceDbContext context)
        {
            context.Teams.AddRange(new[] {
                new Team
                {
                    Id = teamId1,
                    Name = "Team1",
                    Users = new List<User>
                    {
                        new User
                        {
                            UserOid = userId1.ToString(),
                            DisplayName = "User1",
                            TeamRole = "Leader"
                        },
                        new User
                        {
                            UserOid = userId2.ToString(),
                            DisplayName = "User2",
                            TeamRole = "Member"
                        },
                        new User
                        {
                            UserOid = userId3.ToString(),
                            DisplayName = "User3",
                            TeamRole = "Member"
                        },
                        new User
                        {
                            UserOid = userId4.ToString(),
                            DisplayName = "User4",
                            TeamRole = "Member"
                        }
                    }
                },
                new Team { Id = teamId2, Name = "Team2" },
                new Team { Id = teamId3, Name = "Team3" },
            });

            context.TaskItems.AddRange(new[]
            {
                new TaskItem {
                    Id = taskItemId1,
                    Name = "Task1",
                    TeamId = teamId1,
                    //AttachFiles = attachFiles,
                    CreatedBy = new User{ DisplayName = "User1", UserOid = userId1.ToString()},
                    Assignee = new User{ DisplayName = "User3", UserOid = userId3.ToString()},
                    IsPin = true
                },
                new TaskItem {
                    Id = taskItemId2,
                    Name = "Task2",
                    TeamId = teamId1,
                    //AttachFiles = attachFiles,
                    CreatedBy = new User{ DisplayName = "User2", UserOid = userId2.ToString()},
                    Assignee = new User{ DisplayName = "User3", UserOid = userId3.ToString()},
                    IsPin = true
                },
                 new TaskItem {
                    Id = Guid.NewGuid(),
                    Name = "Test for TEST search by assignee",
                    TeamId = teamId1,
                    //AttachFiles = attachFiles,
                    CreatedBy = new User{ DisplayName = "User2", UserOid = userId2.ToString()},
                    Assignee = new User{ DisplayName = "Assignee", UserOid = userId3.ToString()},
                    IsPin = true
                },
                new TaskItem {
                    Id = taskItemId3,
                    Name = "Task3",
                    TeamId = teamId2,
                    //AttachFiles = attachFiles,
                    CreatedBy = new User{ DisplayName = "User1", UserOid = userId1.ToString()},
                    Assignee = new User{ DisplayName = "User3", UserOid = userId3.ToString()},
                    IsPin = false
                },
                new TaskItem { Id = taskItemId4,
                    Name = "Task4",
                    TeamId = teamId4,
                    //AttachFiles = attachFiles,
                    CreatedBy = new User{ DisplayName = "User2", UserOid = userId2.ToString()},
                    Assignee = new User{ DisplayName = "User3", UserOid = userId3.ToString()},
                    IsPin = true
                },
            });
            context.Conversations.AddRange(new[]
            {
                new Conversation
                {
                    Id = conversation1,
                    Members = new List<User>
                    {
                        new User
                        {
                            UserOid = userId1.ToString(),
                            DisplayName = "User1",
                        },
                        new User
                        {
                            UserOid = userId2.ToString(),
                            DisplayName = "User2",
                        },
                    },
                    Type = "pair"
                }, new Conversation
                {
                    Id = conversation3,
                    Members = new List<User>
                    {
                        new User
                        {
                            UserOid = userId1.ToString(),
                            DisplayName = "User1",
                        },
                        new User
                        {
                            UserOid = userId2.ToString(),
                            DisplayName = "User2",
                        },
                        new User
                        {
                            UserOid = userId3.ToString(),
                            DisplayName = "User3",
                        },
                    },
                    Type = "group",
                    Name = "Group 3"
                },
                new Conversation
                {
                    Id = conversation2,
                    Members = new List<User>
                    {
                        new User
                        {
                            UserOid = userId1.ToString(),
                            DisplayName = "User1",
                        },
                        new User
                        {
                            UserOid = userId3.ToString(),
                            DisplayName = "User3",
                        },
                    },
                    Type = "pair"
                },
                new Conversation
                {
                    Id = channelId1,
                    Members = new List<User>
                    {
                        new User
                        {
                            UserOid = userId1.ToString(),
                            DisplayName = "User1",
                        },
                        new User
                        {
                            UserOid = userId2.ToString(),
                            DisplayName = "User3",
                        },
                    },
                    Type = "channel",
                    TeamId = teamId1.ToString(),
                    CreatedBy= userId1.ToString(),
                    Name = channel1Name
                },
                new Conversation
                {
                    Id = channelId2,
                    Members = new List<User>
                    {
                        new User
                        {
                            UserOid = userId1.ToString(),
                            DisplayName = "User1",
                        },
                        new User
                        {
                            UserOid = userId2.ToString(),
                            DisplayName = "User3",
                        },
                    },
                    Type = "channel",
                    TeamId = teamId1.ToString(),
                    CreatedBy= userId2.ToString(),
                    Name = channel2Name
                },
                new Conversation
                {
                    Id = channelId3,
                    Members = new List<User>
                    {
                        new User
                        {
                            UserOid = userId1.ToString(),
                            DisplayName = "User1",
                        },
                        new User
                        {
                            UserOid = userId2.ToString(),
                            DisplayName = "User3",
                        },
                    },
                    Type = "channel",
                    TeamId = teamId3.ToString(),
                    CreatedBy= userId2.ToString(),
                    Name = channel3Name
                },
                new Conversation
                {
                    Id = channelId4,
                    Members = new List<User>
                    {
                        new User
                        {
                            UserOid = userId1.ToString(),
                            DisplayName = "User1",
                        },
                        new User
                        {
                            UserOid = userId2.ToString(),
                            DisplayName = "User3",
                        },
                    },
                    Type = "channel",
                    TeamId = Guid.NewGuid().ToString(),
                    CreatedBy= userId2.ToString(),
                    Name = channel4Name
                }, new Conversation
                {
                    Id = channelId5,
                    Members = new List<User>
                    {
                        new User
                        {
                            UserOid = userId1.ToString(),
                            DisplayName = "User1",
                        },
                        new User
                        {
                            UserOid = userId2.ToString(),
                            DisplayName = "User3",
                        },
                    },
                    Type = "channel",
                    TeamId = teamId1.ToString(),
                    CreatedBy= userId2.ToString(),
                    Name = channel5Name,
                    isPublic = true
                },
                new Conversation
                {
                    Id = channelId6,
                    Members = new List<User>
                    {
                        new User
                        {
                            UserOid = userId1.ToString(),
                            DisplayName = "User1",
                        },
                        new User
                        {
                            UserOid = userId2.ToString(),
                            DisplayName = "User3",
                        },
                    },
                    Type = "channel",
                    TeamId = teamId2.ToString(),
                    CreatedBy= userId2.ToString(),
                    Name = channel6Name,
                    isPublic = true
                }
            });

            context.MessageChats.AddRange(new[]
            {
                new MessageChat
                {
                    Id = messageId1,
                    Content = "message 1",
                    ConversationId = conversation1,
                    RegUserName = "Long",
                    AttachFileList = new List<AttachFile>
                    {
                        new AttachFile
                        {
                            BlobStorageUrl = validBlobStorageUrl,
                            FileName = "file",
                            FileSize = 20,
                            FileStorageName = "kxg5qfz4.jxl",
                            IsPin = false
                        }
                    }
                },
                new MessageChat
                {
                    Id = messageId2,
                    Content = "message 2",
                    ConversationId = conversation1,
                    RegUserName = "Quan"
                },
                new MessageChat
                {
                    Id = messageId3,
                    Content = "message 3",
                    ConversationId = conversation2,
                    RegUserName = "Quan"
                },
                new MessageChat
                {
                    Id = messagechannelId1,
                    Content = "message in channel 1",
                    ConversationId = channelId1,
                    RegUserName = "Quan"
                },
                new MessageChat
                {
                    Id = messagechannelId2,
                    Content = "message in channel 2",
                    ConversationId = channelId1,
                    RegUserName = "Quan",
                    IsPin = true
                },
                new MessageChat
                {
                    Id = messagechannelId3,
                    Content = "message has one reaction",
                    ConversationId = channelId1,
                    RegUserName = "Quan",
                    ReactionList = new List<Reaction>
                    {
                        new Reaction
                        {
                            ReactorId = userId1.ToString(),
                            ReactorName = "User1",
                            ReactionType = "Like"
                        }
                    },
                    IsPin = true
                },
                new MessageChat
                {
                    Id = messagechannelId4,
                    Content = "message has one reaction",
                    ConversationId = channelId5,
                    RegUserName = "Quan",
                    ReactionList = new List<Reaction>
                    {
                        new Reaction
                        {
                            ReactorId = userId1.ToString(),
                            ReactorName = "User1",
                            ReactionType = "Like"
                        }
                    },
                    IsPin = true
                },
                new MessageChat
                {
                    Id = relatedMessageId1,
                    Content = "message in channel 1",
                    ConversationId = channelId1,
                    RegUserName = "Quan",
                    RelatedMessagesId = relatedMessageId1,
                },
                new MessageChat
                {
                    Id = relatedMessageId2,
                    Content = "message in channel 2",
                    ConversationId = channelId1,
                    RegUserName = "Quan",
                    RelatedMessagesId = relatedMessageId2,
                },
                new MessageChat
                {
                    Id = relatedMessageId3,
                    Content = "message in channel 2",
                    ConversationId = channelId1,
                    RegUserName = "Quan",
                    RelatedMessagesId = relatedMessageId2,
                }, new MessageChat
                {
                    Content = "message in conversation 3: Group 3",
                    ConversationId = conversation3,
                    RegUserName = "User 1",
                    Created = DateTime.UtcNow,
                    ConversationName = "Group 3"
                },
            });

            context.Notifications.AddRange(new[]
            {
                new Notification
                {
                    Id = notificationId1,
                    MessageContent = "message 1 @Quan",
                    ConversationId = conversation1.ToString(),
                    MessageId = messageId1.ToString(),
                    RegUserName = "Long",
                    CreatedBy = userId1.ToString(),
                    isRead = false,
                    ToUser = new User {UserOid = userId2.ToString(), DisplayName = "Quan"},
                    Type = "Mention"
                },
                new Notification
                {
                    Id = notificationId2,
                    MessageContent = "message 2 @Long",
                    ConversationId = conversation1.ToString(),
                    MessageId = messageId2.ToString(),
                    CreatedBy = userId2.ToString(),
                    RegUserName = "Quan",
                    isRead = false,
                    ToUser = new User {UserOid = userId1.ToString(), DisplayName = "Long"},
                    Type = "Mention"
                },
                new Notification
                {
                    Id = notificationId3,
                    MessageContent = "message 2 @Quan",
                    ConversationId = conversation1.ToString(),
                    MessageId = messageId3.ToString(),
                    RegUserName = "Long",
                    CreatedBy = userId1.ToString(),
                    isRead = false,
                    ToUser = new User {UserOid = userId2.ToString(), DisplayName = "Quan"},
                    Type = "Mention"
                },
                new Notification
                {
                    Id = notificationId4,
                    MessageContent = "@Quan mention",
                    ConversationId = conversation1.ToString(),
                    MessageId = messageId4.ToString(),
                    RegUserName = "Long",
                    CreatedBy = userId1.ToString(),
                    isRead = false,
                    ToUser = new User {UserOid = userId1.ToString(), DisplayName = "Quan"},
                    Type = "Mention",
                    TeamId = teamId1.ToString()
                }
            });

            context.Planners.AddRange(new[]
            {
                new Planner()
                {
                    Id = plannerId1,
                    TeamId = teamId1,
                    Title = "Planner 1",
                    Purpose = "Purpose 1",
                    Tags = new List<Tag>
                    {
                        new Tag()
                        {
                            TagName = "Tag 1"
                        },
                        new Tag()
                        {
                            TagName = "Tag 2"
                        }
                    },
                    Milestones =new List<Milestone>
                    {
                        new Milestone()
                        {
                            Title = "Milestone 1",
                            Description = "Milestone 1 Description",
                            Date = DateTime.UtcNow,
                            TaskIds = new List<Guid>
                            {
                                taskItemId1,
                                Guid.NewGuid(),
                            }
                        },
                        new Milestone()
                        {
                            Title = "Milestone 2",
                            Description = "Milestone 2 Description",
                            Date = DateTime.UtcNow,
                        }
                    }
                },
                new Planner()
                {
                    Id = plannerId2,
                    TeamId = teamId1,
                    Title = "Planner 2",
                    Purpose = "Purpose 2",
                    Tags =new List<Tag>
                    {
                        new Tag()
                        {
                            TagName = "Tag 2"
                        },
                        new Tag()
                        {
                            TagName = "Tag 3"
                        }
                    }
                },
                new Planner()
                {
                    Id = plannerId3,
                    TeamId = teamId2,
                    Title = "Planner 3",
                    Purpose = "Purpose 3",
                    Tags =new List<Tag>
                    {
                        new Tag()
                        {
                            TagName = "Tag 3"
                        },
                        new Tag()
                        {
                            TagName = "Tag 1"
                        }
                    }
                }
            });

            context.SaveChanges();
        }

        public static void Destroy(ColabSpaceDbContext context)
        {
            context.Database.EnsureDeleted();

            context.Dispose();
        }
    }
}
