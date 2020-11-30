using SearchFight.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace SearchFight.Domain
{
    public class SearchFight
    {
        private const int MinimumProviderSearchResults = 2;

        public Guid Id { get; } = Guid.NewGuid();

        private readonly IDictionary<string, SearchResultsTotals> providersSearchResults = new Dictionary<string, SearchResultsTotals>();
        public IReadOnlyDictionary<string, SearchResultsTotals> ResultsTotals => providersSearchResults.ToImmutableDictionary();

        public void AddProviderSearchResultsTotals(string provider, SearchResultsTotals providerSearchResults)
        {
            if (string.IsNullOrWhiteSpace(provider)) throw new ArgumentException(nameof(provider));
            if (providerSearchResults is null) throw new ArgumentException(nameof(providerSearchResults));

            provider = provider.ToLower();
            
            if (!providersSearchResults.TryAdd(provider, providerSearchResults))
            {
                throw new ProviderResultsTotalsAlreadyAddedException($"There is already a provider added with the name {provider}.");
            }

        }

        public void AddProviderSearchResultsTotal(string provider, string searchTerm, long resultsTotal)
        {
            if (string.IsNullOrWhiteSpace(provider)) throw new ArgumentException(nameof(provider));
            if (string.IsNullOrWhiteSpace(searchTerm)) throw new ArgumentException(nameof(searchTerm));
                        
            if(!providersSearchResults.TryGetValue(provider, out var providerSearchResults))
            {
                providerSearchResults = new SearchResultsTotals();
                providersSearchResults.Add(provider, providerSearchResults);
            }

            searchTerm = searchTerm.ToLower();
            if (providerSearchResults.ContainsKey(searchTerm))
            {
                throw new SearchTermAlreadyAddedException($"There is already a result record for {searchTerm}.");
            }

            providerSearchResults.Add(searchTerm, resultsTotal);
        }

        public string RunSearchFightByProvider(string provider)
        {
            if (string.IsNullOrWhiteSpace(provider)) throw new ArgumentException(nameof(provider));
            
            var providerResults = ResultsTotals[provider];

            return providerResults.Aggregate((maximum, next) => maximum.Value > next.Value ? maximum : next).Key;
        }

        public string RunSearchFight()
        {
            if (providersSearchResults.Count < MinimumProviderSearchResults)
            {
                throw new NotEnoughProvidersResultsException();
            }

            return ResultsTotals
                    .Values
                    .SelectMany(sr => sr)
                    .GroupBy(kp => kp.Key)
                    .Select(g => new
                    {
                        SearchTerm = g.Key,
                        Total = g.Sum(st => st.Value)
                    })
                    .Aggregate((max, next) => max.Total > next.Total ? max : next)
                    .SearchTerm;
        }
    }
}
