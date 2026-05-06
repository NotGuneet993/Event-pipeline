using Consumer.Models;
using Elastic.Clients.Elasticsearch;

namespace Consumer.Services
{
    public class ElasticsearchService
    {
        private readonly ElasticsearchClient _client;
        private readonly ILogger<ElasticsearchService> _logger;

        public ElasticsearchService(ElasticsearchClient client, ILogger<ElasticsearchService> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task IndexAsync(EtwRecordEntity evt, CancellationToken ct = default)
        {
            var respose = await _client.IndexAsync(evt, ct);

            if (!respose.IsSuccess())
            {
                _logger.LogError("Failed to index event {EventName}", evt.EventName);
            }
        }

        public async Task<IReadOnlyList<EtwRecordEntity>> SearchAsync(string query)
        {
            var response = await _client.SearchAsync<EtwRecordEntity>(s => s
                .Size(100)
                .Query(q => q
                    .MultiMatch(m =>m 
                        .Query(query)
                        .Fields(new[] { "eventName", "processName", "providerName", "machineName"})
                    )
                )
                .Sort(so => so
                    .Field(f => f
                        .Field("time")
                        .Order(SortOrder.Desc)
                    )
                )
            );

            if (!response.IsSuccess())
            {
                _logger.LogError("Search failed for query {Query}", query);
                return [];
            }

            return (IReadOnlyList<EtwRecordEntity>)response.Documents;
        }
    }
}
