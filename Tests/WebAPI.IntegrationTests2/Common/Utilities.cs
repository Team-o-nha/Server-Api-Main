using ColabSpace.Domain.Entities;
using ColabSpace.Infrastructure.Persistence;
using System;
using System.Collections.Generic;

namespace ColabSpace.WebAPI.IntegrationTests2.Common
{
    public class Utilities
    {
        public static void InitializeDbForTests(ColabSpaceDbContext context)
        {
            string loginUserId = "020cdee0-8ecd-408a-b662-cd4d9cdf0100";
            string teamId = "ffa8a3e7-50ab-4b10-8e49-96c9f837169d";
            string loginUserId2 = "9c7ff9c5-90bd-4207-9dff-01da2ceece21";

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
                    Id = new Guid("7567347e-0580-4807-97f1-8edad42c9758"),
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
                    Id = Guid.Parse("66edb7c7-11bf-40a5-94ab-75a3364fef61"),
                    Name = "Team 4",
                    Description = "Des4",
                    Users = new List<User>
                    {
                        new User()
                        {
                            UserOid = Guid.NewGuid().ToString(),
                            DisplayName = "TestUser1",
                            TeamRole = "Leader"
                        },new User()
                        {
                            UserOid = "66edb7c7-11bf-40a5-94ab-75a3364fef60",
                            DisplayName = "TestUser2",
                            TeamRole = "Member"
                        }
                    }
                },
                new Team
                {
                    Id = Guid.Parse("de14a885-71d4-4da0-bb17-048d74d33adc"),
                    Name = "Team 5",
                    Description = "Des5",
                    Users = new List<User>
                    {
                        new User()
                        {
                            UserOid = Guid.NewGuid().ToString(),
                            DisplayName = "TestUser ",
                            TeamRole = "Leader"
                        },
                        new User()
                        {
                            UserOid = loginUserId2,
                            DisplayName = "TestUser123 ",
                            TeamRole = "member"
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
                    Id = new Guid("de14a885-71d4-4da0-bb17-048d74d33ada"),
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
                    Id = Guid.Parse("de14a885-71d4-4da0-bb17-048d74d33adb"),
                    Name = "Task 4",
                    Description = "Des4",
                    CreatedBy = new User() { UserOid = loginUserId, DisplayName = "TestUser1" },
                    Status = 1,
                    TeamId = Guid.NewGuid(),
                    IsPin = false
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
                            DisplayName = "TestUser2"
                        }
                    },
                    Type = "pair",
                    TeamId = null,
                    Id = new Guid("b73477a4-f61d-46fa-873c-7d71c01dfbdf")
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
                },
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
                            UserOid = loginUserId2,
                            DisplayName = "TestUser3"
                        }
                    },
                    Type = "pair",
                    TeamId = null,
                    Id = new Guid("b73477a4-f61d-46fa-873c-7d71c01dfbd2")
                },
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
                            UserOid = loginUserId2,
                            DisplayName = "TestUser3"
                        },
                        new User
                        {
                            UserOid = Guid.NewGuid().ToString(),
                            DisplayName = Guid.NewGuid().ToString()
                        }
                    },
                    Type = "group",
                    TeamId = null,
                    Id = new Guid("dcdb9146-32e5-4cc3-ad1c-d10e05745f02")
                },
                new Conversation
                {
                    Name = "Team channel 1",
                    isPublic = false,
                    Members = new List<User>
                    {
                        new User
                        {
                            UserOid = loginUserId,
                            DisplayName = "TestUser1-2"
                        },
                        new User
                        {
                            UserOid = loginUserId2,
                            DisplayName = "TestUser3"
                        },
                        new User
                        {
                            UserOid = Guid.NewGuid().ToString(),
                            DisplayName = Guid.NewGuid().ToString()
                        }
                    },
                    Type = "channel",
                    TeamId = teamId,
                    Id = new Guid("dcdb9146-32e5-4cc3-ad1c-d10e05745f05")
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
                    ToUser = new User {UserOid =new Guid("66edb7c7-11bf-40a5-94ab-75a3364fef60").ToString(), DisplayName = "USER2"},
                    Type = "Mention",
                    TeamId = "66edb7c7-11bf-40a5-94ab-75a3364fef61"
                }
            });

            context.MessageChats.AddRange(
                new MessageChat
                {
                    Id = new Guid("b73477a4-f61d-46fa-873c-7d71c01dfbdf"),
                    ConversationId = new Guid("b73477a4-f61d-46fa-873c-7d71c01dfbdf"),
                    Content = "abc",
                    RegUserName = "Long",
                    Created = DateTime.UtcNow
                },
                new MessageChat
                {
                    Id = new Guid("96149786-a9f7-4b6a-a932-5ddb15a84d1a"),
                    ConversationId = new Guid("b73477a4-f61d-46fa-873c-7d71c01dfbdf"),
                    Content = "abc",
                    RegUserName = "Long",
                    RelatedMessagesId = new Guid("b73477a4-f61d-46fa-873c-7d71c01dfbdf"),
                    RelatedTaskId = new Guid("197d0438-e04b-453d-b5de-eca05960c6ae"),
                    Created = DateTime.UtcNow.AddDays(1)
                },
                new MessageChat
                {
                    Id = new Guid("a9dc2523-a426-4d4a-9d64-0f3d440ae989"),
                    ConversationId = new Guid("b73477a4-f61d-46fa-873c-7d71c01dfbd2"),
                    Content = "abc",
                    RegUserName = "Long",
                    RelatedMessagesId = new Guid("b73477a4-f61d-46fa-873c-7d71c01dfbdf"),
                    RelatedTaskId = new Guid("197d0438-e04b-453d-b5de-eca05960c6ae"),
                    Created = DateTime.UtcNow
                }
                );


            context.Planners.AddRange(new[]
            {
                new Planner()
                {
                    Id = new Guid("cbb85a08-ed54-4924-9135-e1f723a2ba6b"),
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