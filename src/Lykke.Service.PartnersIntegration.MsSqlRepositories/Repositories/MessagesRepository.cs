﻿using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Common.MsSql;
using Lykke.Service.PartnersIntegration.Domain.Models;
using Lykke.Service.PartnersIntegration.Domain.Repositories;
using Lykke.Service.PartnersIntegration.MsSqlRepositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lykke.Service.PartnersIntegration.MsSqlRepositories.Repositories
{
    public class MessagesRepository : IMessagesRepository
    {
        private readonly MsSqlContextFactory<PartnersIntegrationContext> _msSqlContextFactory;
        private readonly IMapper _mapper;

        public MessagesRepository(MsSqlContextFactory<PartnersIntegrationContext> msSqlContextFactory,
            IMapper mapper)
        {
            _msSqlContextFactory = msSqlContextFactory;
            _mapper = mapper;
        }

        public async Task<Guid> InsertAsync(MessagesPostRequest contract)
        {
            using (var context = _msSqlContextFactory.CreateDataContext())
            {
                var entity = _mapper.Map<MessageEntity>(contract);

                entity.CreationTimestamp = DateTime.UtcNow;

                context.Add(entity);

                await context.SaveChangesAsync();

                return entity.Id;
            }
        }

        public async Task<MessageGetResponse> GetByIdAsync(string partnerMessageId)
        {
            using (var context = _msSqlContextFactory.CreateDataContext())
            {
                var entity = await context.Messages.FirstOrDefaultAsync(m => m.Id.ToString() == partnerMessageId);

                return _mapper.Map<MessageGetResponse>(entity);
            }
        }

        public async Task DeleteMessageAsync(string partnerMessageId)
        {
            using (var context = _msSqlContextFactory.CreateDataContext())
            {
                var entity = await context.Messages.FirstOrDefaultAsync(m => m.Id.ToString() == partnerMessageId);

                if (entity != null)
                {
                    context.Remove(entity);

                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
