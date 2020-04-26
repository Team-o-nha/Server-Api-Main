using ColabSpace.Application.Common.Models;
using ColabSpace.Application.Teams.Models;
using ColabSpace.Application.Teams.Queries.GetTeam;
using ColabSpace.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ColabSpace.WebAPI.Controllers
{
    [Authorize]
    public class DirectoryController : ApiController
    {
        internal static class RouteNames
        {
            public const string Users = nameof(Users);
            public const string UserById = nameof(UserById);
            public const string Groups = nameof(Groups);
            public const string GroupById = nameof(GroupById);
        }

        [HttpGet("users/{id}", Name = RouteNames.UserById)]
        public async Task<IActionResult> GetUser(string id)
        {
            UserDto objUser;
            try
            {
                if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id))
                {
                    return BadRequest();
                }


                // Initialize the GraphServiceClient.
                GraphServiceClient client = await MicrosoftGraphClient.GetGraphServiceClient();

                // Load user profile.
                var user = await client.Users[id].Request().GetAsync();

                // Copy Microsoft-Graph User to DTO User
                objUser = CopyHandler.UserProperty(user);

                return Ok(objUser);
            }
            catch (ServiceException ex)
            {
                if (ex.StatusCode == HttpStatusCode.BadRequest)
                {
                    return BadRequest();
                }
                else
                {
                    return NotFound();
                }
            }
        }

        [HttpPut("users/{id}", Name = RouteNames.UserById)]
        public async Task<IActionResult> UpdateUser([FromRoute] string id, [FromBody] UserDto userRequest)
        {
            try
            {
                if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id))
                {
                    return BadRequest();
                }

                // Initialize the GraphServiceClient.
                GraphServiceClient client = await MicrosoftGraphClient.GetGraphServiceClient();

                var user = new Microsoft.Graph.User
                {
                    DisplayName = userRequest.DisplayName,
                    GivenName = userRequest.GivenName,
                    Surname = userRequest.Surname,
                };

                await client.Users[id].Request().UpdateAsync(user);

                return NoContent();
            }
            catch (ServiceException ex)
            {
                if (ex.StatusCode == HttpStatusCode.BadRequest)
                {
                    return BadRequest();
                }
                else
                {
                    return NotFound();
                }
            }
        }

        [HttpGet("users/")]
        public async Task<IActionResult> GetUsers()
        {
            UsersDto users = new UsersDto();
            try
            {
                users.Resources = new List<UserDto>();

                // Initialize the GraphServiceClient.
                GraphServiceClient client = await MicrosoftGraphClient.GetGraphServiceClient();

                // Load users profiles.
                var userList = await client.Users.Request().GetAsync();

                // Copy Microsoft User to DTO User
                foreach (var user in userList)
                {
                    var objUser = CopyHandler.UserProperty(user);
                    users.Resources.Add(objUser);
                }
                users.TotalResults = users.Resources.Count;

                return Ok(users);
            }
            catch (ServiceException ex)
            {
                if (ex.StatusCode == HttpStatusCode.BadRequest)
                {
                    return BadRequest();
                }
                else
                {
                    return NotFound();
                }
            }
        }

        [HttpGet("users/notInTeam/{teamId}")]
        public async Task<IActionResult> GetUsersNotInTeam(string teamId)
        {
            UsersDto users = new UsersDto();
            try
            {
                users.Resources = new List<UserDto>();

                // Initialize the GraphServiceClient.
                GraphServiceClient client = await MicrosoftGraphClient.GetGraphServiceClient();

                // Load users profiles.
                var userList = await client.Users.Request().GetAsync();

                TeamModel team = await Mediator.Send(new GetTeamQuery { TeamId = new Guid(teamId) });

                // Copy Microsoft User to DTO User
                foreach (var user in userList)
                {
                    if (!team.Users.Any(x => x.UserId.ToString().Equals(user.Id)))
                    {
                        var objUser = CopyHandler.UserProperty(user);
                        users.Resources.Add(objUser);
                    }
                }
                users.TotalResults = users.Resources.Count;

                return Ok(users);
            }
            catch (ServiceException ex)
            {
                if (ex.StatusCode == HttpStatusCode.BadRequest)
                {
                    return BadRequest();
                }
                else
                {
                    return NotFound();
                }
            }
        }

        [HttpGet("groups/{id}", Name = RouteNames.GroupById)]
        public async Task<IActionResult> GetGroup(string id)
        {
            Application.Common.Models.Group objGroup;
            try
            {

                // Initialize the GraphServiceClient.
                GraphServiceClient client = await MicrosoftGraphClient.GetGraphServiceClient();

                // Load group profile.
                var group = await client.Groups[id].Request().GetAsync();

                // Copy Microsoft-Graph Group to DTO Group
                objGroup = CopyHandler.GroupProperty(group);

                return Ok(objGroup);
            }
            catch (ServiceException ex)
            {
                if (ex.StatusCode == HttpStatusCode.BadRequest)
                {
                    return BadRequest();
                }
                else
                {
                    return NotFound();
                }
            }
        }

        [HttpGet("groups/")]
        public async Task<IActionResult> GetGroups()
        {
            Groups groups = new Groups();
            try
            {
                groups.resources = new List<Application.Common.Models.Group>();

                // Initialize the GraphServiceClient.
                GraphServiceClient client = await MicrosoftGraphClient.GetGraphServiceClient();

                // Load groups profiles.
                var groupList = await client.Groups.Request().GetAsync();

                // Copy Microsoft-Graph Group to DTO Group
                foreach (var group in groupList)
                {
                    var objGroup = CopyHandler.GroupProperty(group);
                    groups.resources.Add(objGroup);
                }
                groups.totalResults = groups.resources.Count;

                return Ok(groups);
            }
            catch (ServiceException ex)
            {
                if (ex.StatusCode == HttpStatusCode.BadRequest)
                {
                    return BadRequest();
                }
                else
                {
                    return NotFound();
                }
            }
        }
    }
}
