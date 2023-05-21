using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SPL.Domain;
using SPL.WebApp.Domain.DTOs;
using SPL.WebApp.Domain.Enums;
using SPL.WebApp.Domain.Services;
using SPL.WebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPL.WebApp.Controllers
{
    public class PageConstructionController : Controller
    {



        public PageConstructionController()
        {

        }

        public IActionResult Index()
        {

            return this.View();
        }
         
        public IActionResult PermissionDenied()
        {

            return this.View();
        }

        public IActionResult Error()
        {

            return this.View();
        }

    }
       
}
