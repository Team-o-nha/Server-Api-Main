using ColabSpace.Domain.Entities;
using ColabSpace.Infrastructure.Persistence;
using System;
using System.Collections.Generic;

namespace ColabSpace.WebAPI.IntegrationTests.Common
{
    public class Utilities
    {
        public static void InitializeDbForTests(ColabSpaceDbContext context)
        {
            string loginUserId = "020cdee0-8ecd-408a-b662-cd4d9cdf0100";

            context.Teams.AddRange(
                new Team
                {
                    Id = new Guid("197d0438-e04b-453d-b5de-eca05960c6ae"),
                    Name = "Team 1",
                    Description = "Des1",
                    Users = new List<User>
                    {
                        new User()
                        {
                            UserOid = loginUserId,
                            DisplayName = "TestUser1",
                            TeamRole = "Leader"
                        }
                    }
                },
                new Team
                {
                    Id = new Guid("7567347E-0580-4807-97F1-8EDAD42C9758"),
                    Name = "Team 2",
                    Description = "Des2",
                    Users = new List<User>
                    {
                        new User()
                        {
                            UserOid = loginUserId,
                            DisplayName = "TestUser1",
                            TeamRole = "Leader"
                        }
                    }
                },
                new Team
                {
                    Id = Guid.NewGuid(),
                    Name = "Team 3",
                    Description = "Des3",
                    Users = new List<User>
                    {
                        new User()
                        {
                            UserOid = loginUserId,
                            DisplayName = "TestUser1",
                            TeamRole = "Leader"
                        }
                    }
                },
                new Team
                {
                    Id = Guid.NewGuid(),
                    Name = "Team 4",
                    Description = "Des4",
                    Users = new List<User>
                    {
                        new User()
                        {
                            UserOid = Guid.NewGuid().ToString(),
                            DisplayName = "TestUser1",
                            TeamRole = "Leader"
                        }
                    }
                }
            );
            context.TaskItems.AddRange(
                new TaskItem
                {
                    Id = new Guid("197d0438-e04b-453d-b5de-eca05960c6ae"),
                    Name = "Task 1",
                    Description = "Des1",
                    CreatedBy = new User() { UserOid = loginUserId, DisplayName = "TestUser1" },
                    Status = 1,
                    TeamId = new Guid("197d0438-e04b-453d-b5de-eca05960c6ae"),
                    IsPin = true
                },
                new TaskItem
                {
                    Id = new Guid("DE14A885-71D4-4DA0-BB17-048D74D33ADA"),
                    Name = "Task 2",
                    Description = "Des2",
                    CreatedBy = new User() { UserOid = loginUserId, DisplayName = "TestUser1" },
                    Status = 1,
                    TeamId = new Guid("197d0438-e04b-453d-b5de-eca05960c6ae"),
                    IsPin = true
                },
                new TaskItem
                {
                    Id = Guid.NewGuid(),
                    Name = "Task 3",
                    Description = "Des3",
                    CreatedBy = new User() { UserOid = loginUserId, DisplayName = "TestUser1" },
                    Status = 1,
                    TeamId = Guid.NewGuid(),
                },
                new TaskItem
                {
                    Id = Guid.NewGuid(),
                    Name = "Task 4",
                    Description = "Des4",
                    CreatedBy = new User() { UserOid = loginUserId, DisplayName = "TestUser1" },
                    Status = 1,
                    TeamId = Guid.NewGuid(),
                }
            );

            //context.HelpItems.AddRange(
            //    new HelpItem()
            //    {
            //        Id = new Guid("7B41389E-7C41-4594-8AF7-06F01D21F973"),
            //        Name = "Topic",
            //        Description = string.Empty,
            //        Content = new AttachFile()
            //        {
            //            FileName = "test.a",
            //            FileSize = 10,
            //            BlobStorageUrl = null,
            //            FileStorageName = "test.b",
            //        },
            //    }
            //);

            context.Conversations.AddRange(
                new Conversation
                {
                    Name = string.Empty,
                    isPublic = false,
                    Members = new List<User>
                    {
                        new User
                        {
                            UserOid = loginUserId,
                            DisplayName = "TestUser1"
                        },
                        new User
                        {
                            UserOid = Guid.NewGuid().ToString(),
                            DisplayName = "TestUser1"
                        }
                    },
                    Type = "pair",
                    TeamId = null,
                    Id = new Guid("B73477A4-F61D-46FA-873C-7D71C01DFBDF")
                }, new Conversation
                {
                    Name = string.Empty,
                    isPublic = false,
                    Members = new List<User>
                    {
                        new User
                        {
                            UserOid = loginUserId,
                            DisplayName = "TestUser1"
                        },
                        new User
                        {
                            UserOid = "9a1709db-fddf-4f91-8046-e3bc92316bab",
                            DisplayName = "TestUser2"
                        }
                    },
                    Type = "pair",
                    TeamId = null,
                    Id = Guid.NewGuid()
                }, new Conversation
                {
                    Name = "duplicate",
                    isPublic = true,
                    Members = new List<User>
                    {
                        new User
                        {
                            UserOid = loginUserId,
                            DisplayName = "TestUser1"
                        },
                        new User
                        {
                            UserOid = "9a1709db-fddf-4f91-8046-e3bc92316bab",
                            DisplayName = "TestUser2"
                        }
                    },
                    Type = "channel",
                    TeamId = "197d0438-e04b-453d-b5de-eca05960c6ae",
                    Id = new Guid("CF7F4DE0-58B8-4CE1-8758-055706A41BE7"),
                    CreatedBy = "020cdee0-8ecd-408a-b662-cd4d9cdf0100"
                }
            );

            context.Notifications.AddRange(new[] {
                new Notification()
                {
                    MessageContent = "message 2 @USER2",
                    ConversationId = Guid.NewGuid().ToString(),
                    MessageId = Guid.NewGuid().ToString(),
                    CreatedBy = Guid.NewGuid().ToString(),
                    RegUserName = "USER 1",
                    isRead = false,
                    ToUser = new User {UserOid =new Guid("66EDB7C7-11BF-40A5-94AB-75A3364FEF60").ToString(), DisplayName = "USER2"},
                    Type = "Mention"
                }
            });

            context.MessageChats.AddRange(
                new MessageChat
                {
                    Id = new Guid("B73477A4-F61D-46FA-873C-7D71C01DFBDF"),
                    ConversationId = new Guid("B73477A4-F61D-46FA-873C-7D71C01DFBDF"),
                    Content = "abc",
                    RegUserName = "Long",
                    Created = DateTime.UtcNow
                },
                new MessageChat
                {
                    Id = new Guid("96149786-A9F7-4B6A-A932-5DDB15A84D1A"),
                    ConversationId = new Guid("B73477A4-F61D-46FA-873C-7D71C01DFBDF"),
                    Content = "abc",
                    RegUserName = "Long",
                    RelatedMessagesId = new Guid("B73477A4-F61D-46FA-873C-7D71C01DFBDF"),
                    RelatedTaskId = new Guid("197d0438-e04b-453d-b5de-eca05960c6ae"),
                    Created = DateTime.UtcNow
                }
                );


            context.Planners.AddRange(new[]
            {
                new Planner()
                {
                    Id = new Guid("CBB85A08-ED54-4924-9135-E1F723A2BA6B"),
                    TeamId = new Guid("197d0438-e04b-453d-b5de-eca05960c6ae"),
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
                                new Guid("197d0438-e04b-453d-b5de-eca05960c6ae"),
                                new Guid("DE14A885-71D4-4DA0-BB17-048D74D33ADA"),
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
                    Id = new Guid("5F6FC408-D2A5-4BA2-A73E-A4F9125A26B1"),
                    TeamId = new Guid("197d0438-e04b-453d-b5de-eca05960c6ae"),
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
                    Id = new Guid("744C1463-0BEB-48AD-AB79-865B5183651F"),
                    TeamId = new Guid("7567347E-0580-4807-97F1-8EDAD42C9758"),
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
    }
}