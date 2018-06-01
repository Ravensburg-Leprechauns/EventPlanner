﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Planner.Models;
using Planner.Models.Helper;
using Planner.Models.Repository;
using Planner.ViewModels;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BaseballPlanner.Controllers
{
    [Authorize(Roles = RoleNames.ROLE_ADMIN)]
    public class UserController : Controller
    {
        private UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly ITeamAssociationRepository _teamAssociationRepository;

        public UserController(UserManager<User> userManager, IUserRepository userRepository, ITeamRepository teamRepository, ITeamAssociationRepository teamAssociationRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _teamRepository = teamRepository;
            _teamAssociationRepository = teamAssociationRepository;
        }

        public IActionResult Index()
        {
            UserIndexViewModel viewModel = new UserIndexViewModel();
            viewModel.Users = _userRepository.GetAll();
            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            UserEditViewModel viewModel = new UserEditViewModel();

            if (id == null)
                return StatusCode((int)HttpStatusCode.BadRequest);

            viewModel.CurrentUser = _userRepository.Find(x => x.UserId == id).FirstOrDefault();
            viewModel.AllTeams = _teamRepository.GetAll().ToList();

            var associations = _teamAssociationRepository.Find(x => x.UserId == id);
            foreach(var team in viewModel.AllTeams)
            {
                team.Selected = associations.FirstOrDefault(x => x.TeamId == team.Id) != null;
            }
            viewModel.IsAdmin = await _userManager.IsInRoleAsync(viewModel.CurrentUser, RoleNames.ROLE_ADMIN);

            if (viewModel.CurrentUser == null)
                return StatusCode((int)HttpStatusCode.NotFound);
            else
                return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, UserEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            if (id == null)
                return StatusCode((int)HttpStatusCode.BadRequest);

            var found = _userRepository.Find(x => x.UserId == id).FirstOrDefault();

            if (found == null)
                return StatusCode((int)HttpStatusCode.NotFound);

            // Save Teams
            _teamAssociationRepository.Update((int)id, viewModel.AllTeams);

            // Save / Remove Role
            bool userHasRole = await _userManager.IsInRoleAsync(found, RoleNames.ROLE_ADMIN);
            if (viewModel.IsAdmin && !userHasRole)
                await _userManager.AddToRoleAsync(found, RoleNames.ROLE_ADMIN);
            else if (!viewModel.IsAdmin && userHasRole)
                await _userManager.RemoveFromRoleAsync(found, RoleNames.ROLE_ADMIN);

            await _userManager.UpdateAsync(found);

            return RedirectToAction("Index", "User");
        }
    }
}