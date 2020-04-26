using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.Common.Models;
using ColabSpace.Application.Conversations.Models;
using ColabSpace.Application.Conversations.Queries.GetConversationsByName;
using ColabSpace.Application.Teams.Models;
using ColabSpace.Application.Teams.Queries.GetTeam;
using ColabSpace.Application.Teams.Queries.GetTeamsList;
using ColabSpace.WebAPI.Controllers;
using ColabSpace.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Authorize]
    public class SearchController : ApiController
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly ISearchService _searchService;

        public SearchController(ICurrentUserService currentUserService, ISearchService searchService)
        {
            _currentUserService = currentUserService;
            _searchService = searchService;
        }

        [HttpGet("userAndTeam")]
        public async Task<IActionResult> SearchUsersAndTeams([FromQuery]string keyword)
        {
            List<SearchModel> result = new List<SearchModel>();
            try
            {
                if (String.IsNullOrEmpty(keyword) || keyword == "undefined")
                {
                    return Ok(result);
                }
                // Initialize the GraphServiceClient.
                GraphServiceClient client = await MicrosoftGraphClient.GetGraphServiceClient();

                // Load users profiles.
                var userList = await client.Users.Request().Filter($"startswith(DisplayName,'{keyword}') " +
                    $"or startswith(UserPrincipalName,'{keyword}') " +
                    $"or startswith(GivenName,'{keyword}')").Top(5).GetAsync();

                // Copy Microsoft User to Search Model
                foreach (var user in userList)
                {
                    if (!_currentUserService.UserId.Equals(user.Id))
                    {
                        var objUser = new SearchModel { Id = new Guid(user.Id), Name = user.DisplayName, isTeam = false };
                        result.Add(objUser);
                    }
                }

                // Copy Team to Search Model
                IEnumerable<TeamModel> teams = await Mediator.Send(new GetTeamsQuery { TeamName = keyword });
                foreach (var team in teams)
                {
                    var objTeam = new SearchModel
                    {
                        Id = team.Id,
                        Name = team.Name,
                        isTeam = true
                    };
                    result.Add(objTeam);
                }

                result = result.OrderBy(x => x.Name).ToList();

                return Ok(result);
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

        [HttpGet("userInTeam/{teamId}")]
        public async Task<IActionResult> SearchUsersInTeams(string teamId, [FromQuery] string keyword)
        {
            List<UserModel> result;
            try
            {
                TeamModel teams = await Mediator.Send(new GetTeamQuery { TeamId = new Guid(teamId) });
                // search User
                if (!String.IsNullOrEmpty(keyword) && keyword != "undefined")
                {
                    result = teams.Users.Where(x => x.DisplayName.ToUpper().Contains(keyword.ToUpper())).ToList();
                }
                else
                {
                    result = teams.Users.ToList();
                }

                result = result.OrderBy(x => x.DisplayName).ToList();

                return Ok(result);
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
        [HttpGet("users-groups")]
        public async Task<IActionResult> SearchUsersAndGroups([FromQuery]string keyword)
        {
            List<SearchModel> result = new List<SearchModel>();
            try
            {
                if (String.IsNullOrEmpty(keyword) || keyword == "undefined")
                {
                    return Ok(result);
                }
                // Initialize the GraphServiceClient.
                GraphServiceClient client = await MicrosoftGraphClient.GetGraphServiceClient();

                // Load users profiles.
                var userList = await client.Users.Request().Filter($"startswith(DisplayName,'{keyword}') " +
                    $"or startswith(UserPrincipalName,'{keyword}') " +
                    $"or startswith(GivenName,'{keyword}')").Top(5).GetAsync();

                // Copy Microsoft User to Search Model
                foreach (var user in userList)
                {
                    if (!_currentUserService.UserId.Equals(user.Id))
                    {
                        var objUser = new SearchModel { Id = new Guid(user.Id), Name = user.DisplayName, isTeam = false };
                        result.Add(objUser);
                    }
                }

                // Copy Conversation to Search Model
                IEnumerable<ConversationModel> conversations = await Mediator.Send(new GetConversationsByNameQuery
                {
                    ConversationName = keyword,
                    LoginUserId = Guid.Parse(_currentUserService.UserId)
                });
                foreach (var conversation in conversations)
                {
                    var objConversation = new SearchModel
                    {
                        Id = conversation.Id,
                        Name = conversation.Name,
                        isTeam = true
                    };
                    result.Add(objConversation);
                }

                result = result.OrderBy(x => x.Name).ToList();

                return Ok(result);
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
        [HttpGet("users")]
        public async Task<IActionResult> SearchUsers([FromQuery]string keyword)
        {
            List<SearchModel> result = new List<SearchModel>();
            try
            {
                if (String.IsNullOrEmpty(keyword) || keyword == "undefined")
                {
                    return Ok(result);
                }
                // Initialize the GraphServiceClient.
                GraphServiceClient client = await MicrosoftGraphClient.GetGraphServiceClient();

                // Load users profiles.
                var userList = await client.Users.Request().Filter($"startswith(DisplayName,'{keyword}') " +
                    $"or startswith(UserPrincipalName,'{keyword}') " +
                    $"or startswith(GivenName,'{keyword}')").Top(5).GetAsync();

                // Copy Microsoft User to Search Model
                foreach (var user in userList)
                {
                    if (!_currentUserService.UserId.Equals(user.Id))
                    {
                        var objUser = new SearchModel { Id = new Guid(user.Id), Name = user.DisplayName, isTeam = false };
                        result.Add(objUser);
                    }
                }

                result = result.OrderBy(x => x.Name).ToList();

                return Ok(result);
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

        [HttpGet("messages")]
        public IActionResult SearchMessages([FromQuery]string keyword)
        {
            try
            {
                var result = _searchService.SearchMessages(_currentUserService.UserId, keyword);

                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

    }
}