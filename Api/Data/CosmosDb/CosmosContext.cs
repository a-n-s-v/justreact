using System.Linq.Expressions;
using ESI.Data.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace ESI.Data.CosmosDb
{
    public static class Extensions
    {
        public static IAsyncEnumerable<T> AsAsyncQueryable<T>(this IQueryable<T> queryable)
        {
            return (IAsyncEnumerable<T>)queryable;
        }
    }

    internal static class CosmosAsyncQueryableExtensions
    {
        internal static IQueryable<T> ToCosmosAsyncQueryable<T>(this IOrderedQueryable<T> source)
        {
            return new CosmosAsyncQueryable<T>(source);
        }
    }

    internal class CosmosAsyncQueryable<TResult> : IQueryable<TResult>, IAsyncEnumerable<TResult>
    {
        private readonly IQueryable<TResult> _queryable;

        public CosmosAsyncQueryable(IQueryable<TResult> queryable)
        {
            _queryable = queryable;
            Provider = new CosmosAsyncQueryableProvider(queryable.Provider);
        }

        public Type ElementType => typeof(TResult);

        public Expression Expression => _queryable.Expression;

        public IQueryProvider Provider { get; }

        Expression IQueryable.Expression => Expression;

        public IEnumerator<TResult> GetEnumerator() => _queryable.GetEnumerator();

        public async IAsyncEnumerator<TResult> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            var iterator = _queryable.ToFeedIterator();

            while (iterator.HasMoreResults)
            {
                foreach (var item in await iterator.ReadNextAsync(cancellationToken))
                {
                    yield return item;
                }
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    internal class CosmosAsyncQueryableProvider : IQueryProvider
    {
        private readonly IQueryProvider _provider;

        public CosmosAsyncQueryableProvider(IQueryProvider provider) => _provider = provider;

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression) =>
            new CosmosAsyncQueryable<TElement>(_provider.CreateQuery<TElement>(expression));

        public IQueryable CreateQuery(Expression expression) => CreateQuery<object>(expression);

        public object Execute(Expression expression) => _provider.Execute(expression);

        public TResult Execute<TResult>(Expression expression) => _provider.Execute<TResult>(expression);
    }

    public interface ICosmosContext
    {
        Task Test();
        Task<IQueryable<TItem>> GetItems<TItem>() where TItem : CosmosModel;

        Task<TItem> Save<TItem>(TItem item) where TItem : CosmosModel;

        Task<TItem> GetById<TItem>(string id) where TItem : CosmosModel;
    }

    public class CosmosContext : ICosmosContext
    {
        string endpointUrl;
        string authorizationKey;
        string databaseId;
        string collectionId;
        const string partitionKey = "/modelType";
        CosmosClient cosmosClient;

        public CosmosContext(IConfiguration configuration)
        {
            endpointUrl = configuration["CosmosDB:endpointUrl"];
            authorizationKey = configuration["CosmosDB:authorizationKey"];
            databaseId = configuration["CosmosDB:databaseId"];
            collectionId = configuration["CosmosDB:collectionId"];
            cosmosClient = new CosmosClient(endpointUrl, authorizationKey);
        }

        public async Task Test()
        {
            using (var client = new CosmosClient(endpointUrl, authorizationKey))
            {
                var database = await client.CreateDatabaseIfNotExistsAsync(databaseId);

                // Delete the existing container to prevent create item conflicts
                var container = await database.Database.CreateContainerIfNotExistsAsync(collectionId, partitionKey);
                dynamic testItem = new { id = "MyTestItemId", modelType = "test", details = "it's working", status = "done" };
                await container.Container.UpsertItemAsync(testItem);
            }
        }

        public async Task<IQueryable<TItem>> GetItems<TItem>() where TItem : CosmosModel
        {
            var database = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
            var container = await database.Database.CreateContainerIfNotExistsAsync(collectionId, partitionKey);
            return container.Container.GetItemLinqQueryable<TItem>().ToCosmosAsyncQueryable().Where(_ => _.ModelType == typeof(TItem).Name);
        }

        public async Task<TItem> Save<TItem>(TItem item) where TItem : CosmosModel
        {
            var database = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
            var container = await database.Database.CreateContainerIfNotExistsAsync(collectionId, partitionKey);
            var response = await container.Container.UpsertItemAsync(item);
            return response.Resource;
        }

        public async Task<TItem> GetById<TItem>(string id) where TItem : CosmosModel
        {
            var database = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
            var container = await database.Database.CreateContainerIfNotExistsAsync(collectionId, partitionKey);
            var response = await container.Container.ReadItemAsync<TItem>(id, new PartitionKey(typeof(TItem).Name));
            return response.Resource;
        }
    }
}