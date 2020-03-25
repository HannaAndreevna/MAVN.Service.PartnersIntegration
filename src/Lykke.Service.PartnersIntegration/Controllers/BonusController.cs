﻿using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Service.PartnersIntegration.Client.Api;
using Lykke.Service.PartnersIntegration.Client.Models;
using Lykke.Service.PartnersIntegration.Domain.Models;
using Lykke.Service.PartnersIntegration.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.PartnersIntegration.Controllers
{
    [ApiController]
    [Route("/api/bonus/")]
    public class BonusController : Controller, IBonusApi
    {
        private readonly IMapper _mapper;
        private readonly IBonusService _bonusService;

        public BonusController(IMapper mapper, IBonusService bonusService)
        {
            _mapper = mapper;
            _bonusService = bonusService;
        }

        /// <summary>
        /// Used to trigger a bonus to a customer
        /// </summary>
        /// <param name="model">Information about the bonus and customer</param>
        /// <returns>Status of the trigger</returns>
        [HttpPost("customers")]
        public async Task<List<BonusCustomerResponseModel>> TriggerBonusToCustomers(BonusCustomersRequestModel model)
        {
            var responseContracts = await _bonusService.TriggerBonusToCustomersAsync(
                    _mapper.Map<List<BonusCustomerRequest>>(model.BonusCustomers));

            return _mapper.Map<List<BonusCustomerResponseModel>>(responseContracts);
        }
    }
}
