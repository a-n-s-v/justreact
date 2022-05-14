using ESI.Data.CosmosDb;
using ESI.Data.Dto;
using ESI.Data.Models;

namespace ESI.Data.GraphQL
{
    public class Mutation
    {
        public async Task<BusinessCapability> AddBusinessCapability([Service] ICosmosContext context, BusinessCapabilityDto dto)
        {
            var item = new BusinessCapability
            {
                Id = Guid.NewGuid().ToString("N"),
                Name = dto.Name,
                Description = dto.Description,
            };
            var result = await context.Save(item);
            var trail = new AuditTrail
            {
                Id = Guid.NewGuid().ToString("N"),
                Command = dto,
                EntityType = result.ModelType
            };
            await context.Save(trail);
            return result;
        }
        public async Task<BusinessCapability> UpdateBusinessCapability([Service] ICosmosContext context, BusinessCapabilityDto dto)
        {
            var item = await context.GetById<BusinessCapability>(dto.Id);
            item.Name = dto.Name;
            item.Description = dto.Description;
            var result = await context.Save(item);
            var trail = new AuditTrail
            {
                Id = Guid.NewGuid().ToString("N"),
                Command = dto,
                EntityType = result.ModelType
            };
            await context.Save(trail);
            return result;
        }
    }
}
