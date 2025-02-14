﻿using middleware_d26.Models.DTOs;
using middleware_d26.Services;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Http;

namespace middleware_d26.Controllers
{
    [RoutePrefix("api/somiod/{applicationName}/{containerName}")]
    public class ResourceController : ApiController
    {
        private readonly SubscriptionService subscriptionService;
        private readonly DataService dataService;

        public ResourceController(SubscriptionService subscriptionService, DataService dataService)
        {
            this.subscriptionService = subscriptionService ?? throw new ArgumentNullException(nameof(subscriptionService));
            this.dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
        }

        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateResource(string applicationName, string containerName, [FromBody] EntityRequestDTO createDTO)
        {
            if (createDTO == null || string.IsNullOrEmpty(createDTO.ResType))
            {
                //debug res type

                Debug.WriteLine("creatDTO");
                Debug.WriteLine(createDTO);
                return BadRequest("Invalid or missing res_type (Resource)");
            }

            switch (createDTO.ResType.ToLower())
            {
                case "subscription":
                    //if (createDTO.Subscription == null ||
                    //    string.IsNullOrEmpty(createDTO.Subscription.Event) ||
                    //    string.IsNullOrEmpty(createDTO.Subscription.Endpoint) ||
                    //    string.IsNullOrEmpty(createDTO.Subscription.Name))
                    //{
                    //    return BadRequest("Invalid or missing fields for subscription creation");
                    //}

                    await subscriptionService.CreateSubscription(applicationName, containerName, createDTO.Subscription);
                    return Created(Request.RequestUri, createDTO.Subscription.Endpoint);

                case "data":
                    //if (createDTO.Data == null ||
                    //    string.IsNullOrEmpty(createDTO.Data.Content) ||
                    //    string.IsNullOrEmpty(createDTO.Data.Name))
                    //{
                    //    return BadRequest("Invalid or missing fields for data creation");
                    //}

                    await dataService.CreateData(applicationName, containerName, createDTO.Data);
                    return Created(Request.RequestUri, createDTO.Data.Content);

                default:
                    return BadRequest("Invalid res_type");
            }
        }
    }
}
