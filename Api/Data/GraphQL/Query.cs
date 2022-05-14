using ESI.Data.CosmosDb;
using ESI.Data.Dto;
using ESI.Data.Models;

namespace ESI.Data.GraphQL
{
    public class Query
    {
        //[Serial]
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public async Task<IQueryable<BusinessCapability>> GetCapabilities(

            [Service] ICosmosContext context)
            => await context.GetItems<BusinessCapability>();

        public async Task<ApiResult<BusinessCapabilityDto>> GetCapabilitiesApiResult(
            [Service] ICosmosContext context,
            int pageIndex = 0,
            int pageSize = 10,
            string? sortColumn = null,
            string? sortOrder = null,
            string? filterColumn = null,
            string? filterQuery = null)
        {
            return await ApiResult<BusinessCapabilityDto>.CreateAsync((await context.GetItems<BusinessCapability>()).Select(_ => new BusinessCapabilityDto { Id = _.Id, Name = _.Name, Description = _.Description }),
                    pageIndex,
                    pageSize,
                    sortColumn,
                    sortOrder,
                    filterColumn,
                    filterQuery);
        }

        public async Task<BusinessCapability> GetCapabilityById([Service] ICosmosContext context, string id) =>
            await context.GetById<BusinessCapability>(id);
    }
}
