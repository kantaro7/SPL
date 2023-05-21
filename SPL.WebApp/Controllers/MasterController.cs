namespace SPL.WebApp.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using SPL.WebApp.Domain.Services;
    using System.Threading.Tasks;

    public class MasterController: Controller
    {
        private readonly IMasterHttpClientService _masterHttpClientService;
        private readonly IMapper _mapper;
        public MasterController(
            IMasterHttpClientService masterHttpClientService,
            IMapper mapper)
        {
            _masterHttpClientService = masterHttpClientService;
            _mapper = mapper;
        }

       
    }

 
}
