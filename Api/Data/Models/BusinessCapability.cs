using Newtonsoft.Json;

namespace ESI.Data.Models
{
    public abstract class CosmosModel
    {
        [GraphQLType(typeof(NonNullType<IdType>))]
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("modelType")]
        public string ModelType => GetType().Name;
    }

    public class AuditTrail : CosmosModel
    {
        public string EntityType { get; set; }

        public object Command { get; set; }
    }

    public class BusinessCapability : CosmosModel
    {

        [GraphQLNonNullType]
        public string Name { get; set; }

        [GraphQLNonNullType]
        public string Description { get; set; }
    }

    public class EnterpriseSystem : CosmosModel
    {
        [GraphQLNonNullType]
        public string Name { get; set; }

        [GraphQLNonNullType]
        public string Description { get; set; }

        [GraphQLNonNullType]
        public string BusinessCapabilityId { get; set; }

        [GraphQLNonNullType]
        public string BusinessCapabilityName { get; set; }
    }

    public class ApplicationService : CosmosModel
    {
        [GraphQLNonNullType]
        public string Name { get; set; }

        [GraphQLNonNullType]
        public string Description { get; set; }

        [GraphQLNonNullType]
        public string Alias { get; set; }

        [GraphQLNonNullType]
        public string EnterpriseSystemId { get; set; }

        [GraphQLNonNullType]
        public string EnterpriseSystemName { get; set; }
    }
}
