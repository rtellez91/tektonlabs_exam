using SearchFight.ApplicationServices.Interfaces;
using SearchFight.Core.Builders;
using SearchFight.Core.Model;
using SearchFight.Infrastructure.SearchProviders;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SearchFight.ApplicationServices.Services
{
    public class SearchFightService : ISearchFightService
    {
        private readonly IEnumerable<ISearchProvider> searchProviders;
        private readonly ISearchFightResultsBuilder searchFightResultsBuilder;

        public SearchFightService(IEnumerable<ISearchProvider> searchProviders, ISearchFightResultsBuilder searchFightResultsBuilder)
        {
            this.searchProviders = searchProviders;
            this.searchFightResultsBuilder = searchFightResultsBuilder;
        }

        public async Task<SearchFightResultsDto> RunFight(IEnumerable<string> searchTerms)
        {
            searchTerms = searchTerms.Select(st => st.ToLower()).Distinct();
            var searchFight = new Domain.SearchFight();

            foreach (var searchProvider in searchProviders)
            {
                var providerName = searchProvider.Name;
                foreach (var searchTerm in searchTerms)
                {
                    var searchResult = await searchProvider.Search(new SearchRequest { SearchTerm = searchTerm });
                    searchFight.AddProviderSearchResultsTotal(providerName, searchTerm, searchResult.NumberOfResults);
                    searchFightResultsBuilder.AddResultBySearchTerm(searchTerm, searchProvider.Name, searchResult.NumberOfResults);
                }

                searchFightResultsBuilder.AddWinnerByProvider(providerName, searchFight.RunSearchFightByProvider(providerName));
            }

            var searchTermWinner = searchFight.RunSearchFight();
            searchFightResultsBuilder.SetSearchFightWinner(searchTermWinner);

            return searchFightResultsBuilder.Build();
        }
    }
}
