//using System.Text;
//using Application.Settings;
//using BPMaster.Services;
//using Common.Controllers;
//using DotNetTraining.Domains.Dtos;
//using DotNetTraining.Domains.Entities;
//using DotNetTraining.Requests;
//using DotNetTraining.Services;
//using iText.Commons.Actions.Data;
//using Microsoft.AspNetCore.Mvc;

//[Route("api/login")]
//[ApiController]
//public class LoginController : BaseV1Controller<ProductService, ApplicationSetting>
//{
//    private readonly ProductService _productService;

//    public LoginController(IServiceProvider services, IHttpContextAccessor httpContextAccessor) : base(services, httpContextAccessor)
//    {
//        this._productService = services.GetService<ProductService>()!;
//    }

//}