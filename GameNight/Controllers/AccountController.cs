﻿using GameNight.Models;
using GameNight.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameNight.Controllers
{
    public class AccountController : Controller
    {
        private IRepository<User> userRepo;
        public AccountController(IRepository<User> userRepo)
        {
            this.userRepo = userRepo;
        }

        public ViewResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User model)
        {
            var response = userRepo.CheckLogin(model.Username, model.Password);
            if (response.Result)
            {
                HttpContext.Session.SetString("Username", response.User.Username);
                return RedirectToAction("Details", "User", new { id = response.User.Id});
            }
            else
            {
                ViewBag.ResultMessage = response.Message;
                return View(model);
            }
        }

        public ViewResult Register()
        {
            return View(new User() { });
        }

        [HttpPost]
        public ActionResult Register(User model)
        {
            if (!userRepo.CheckDuplicate(model.Username))
            {
                userRepo.Create(model);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}