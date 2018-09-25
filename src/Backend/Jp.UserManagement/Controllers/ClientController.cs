﻿using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Jp.Application.Interfaces;
using Jp.Application.ViewModels.ClientsViewModels;
using Jp.Domain.Core.Bus;
using Jp.Domain.Core.Notifications;
using Jp.Infra.CrossCutting.Tools.Model;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Jp.Management.Controllers
{
    [Route("[controller]"), 
    // Authorize(Policy = "IS4-ReadOnly")
    ]
    public class ClientsController : ApiController
    {
        private readonly IClientAppService _clientAppService;

        public ClientsController(
            INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediator,
                IClientAppService clientAppService) : base(notifications, mediator)
        {
            _clientAppService = clientAppService;
        }

        [HttpGet, Route("list")]
        public async Task<ActionResult<DefaultResponse<IEnumerable<ClientListViewModel>>>> List()
        {
            var clients = await _clientAppService.GetClients();
            return Response(clients);
        }

        [HttpGet, Route("details")]
        public async Task<ActionResult<DefaultResponse<Client>>> Details(string clientId)
        {
            var clients = await _clientAppService.GetClientDetails(clientId);
            return Response(clients);
        }

        [HttpGet, Route("secrets")]
        public async Task<ActionResult<DefaultResponse<IEnumerable<Secret>>>> Secrets(string clientId)
        {
            var clients = await _clientAppService.GetSecrets(clientId);
            return Response(clients);
        }

        [HttpPost, Route("update")]
        public async Task<ActionResult<DefaultResponse<bool>>> Update([FromBody] Client client)
        {
            var teste = client.ToEntity();
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(false);
            }
            await _clientAppService.Update(client);
            return Response(true);
        }
    }
}
