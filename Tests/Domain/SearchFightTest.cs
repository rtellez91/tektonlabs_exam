using SearchFight.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SearchFight.Tests.Domain
{
    public class SearchFightTest
    {
        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData("    ", "     ")]
        [InlineData("google", null)]
        [InlineData("", "searchTerm")]
        public void Should_Throw_ArgumentException_When_AddingProviderResultsTotal_With_Invalid_Parameters(string providerName, string searchTerm)
        {
            var searchFight = new SearchFight.Domain.SearchFight();

            Assert.Throws<ArgumentException>(() =>
            {
                searchFight.AddProviderSearchResultsTotal(providerName, searchTerm, 0);
            });
        }

        [Fact]
        public void Should_Throw_SearchTermAlreadyAddedException_When_AddingResult_With_Repeated_SearchTerm()
        {
            var providerName = "Google";
            var searchFight = new SearchFight.Domain.SearchFight();

            var searchTerm = ".net";
            searchFight.AddProviderSearchResultsTotal(providerName, searchTerm, 10);

            Assert.Throws<SearchTermAlreadyAddedException>(() =>
            {
                searchFight.AddProviderSearchResultsTotal(providerName, searchTerm, 10);
            });
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", null)]
        [InlineData("    ", null)]
        [InlineData("google", null)]
        public void Should_Throw_ArgumentException_When_AddingProviderResultsTotals_With_Invalid_Parameters(string providerName, SearchFight.Domain.SearchResultsTotals resultsTotal)
        {
            var searchFight = new SearchFight.Domain.SearchFight();

            Assert.Throws<ArgumentException>(() =>
            {
                searchFight.AddProviderSearchResultsTotals(providerName, resultsTotal);
            });
        }

        [Fact]
        public void Should_Throw_SearchTermAlreadyAddedException_When_AddingResult_With_Repeated_ProviderName()
        {
            var providerName = "Google";
            var searchFight = new SearchFight.Domain.SearchFight();

            var resultsTotal = new SearchFight.Domain.SearchResultsTotals();
            searchFight.AddProviderSearchResultsTotals(providerName, resultsTotal);

            Assert.Throws<ProviderResultsTotalsAlreadyAddedException>(() =>
            {
                searchFight.AddProviderSearchResultsTotals(providerName, resultsTotal);
            });
        }

        [Fact]
        public void Should_AddResult_With_Different_SearchTerms()
        {
            var providerName = "Google";

            var searchTerm = ".net";
            var numberOfResults = 10;
            var searchTerm2 = "java";
            var numberOfResults2 = 20;

            var searchFight = new SearchFight.Domain.SearchFight();


            searchFight.AddProviderSearchResultsTotal(providerName, searchTerm, numberOfResults);
            searchFight.AddProviderSearchResultsTotal(providerName, searchTerm2, numberOfResults2);

            var providerResults = searchFight.ResultsTotals[providerName];
            Assert.True(providerResults.ContainsKey(searchTerm));
            Assert.Equal(numberOfResults, providerResults[searchTerm]);
        }

        [Fact]
        public void Should_GetWinnerByProvider()
        {
            var providerName = "Google";
            var searchFight = new SearchFight.Domain.SearchFight();

            var searchTerm1 = ".net";
            var numberOfResults1 = 20;
            searchFight.AddProviderSearchResultsTotal(providerName, searchTerm1, numberOfResults1);

            var searchTerm2 = "java";
            var numberOfResults2 = 60;
            searchFight.AddProviderSearchResultsTotal(providerName, searchTerm2, numberOfResults2);

            var searchTerm3 = "ruby";
            var numberOfResults3 = 50;
            searchFight.AddProviderSearchResultsTotal(providerName, searchTerm3, numberOfResults3);

            var winner = searchFight.RunSearchFightByProvider(providerName);

            Assert.Equal(searchTerm2, winner);
        }

        [Fact]
        public void Should_Return_SearchFight_Winner()
        {
            var googleProvider = new SearchFight.Domain.SearchResultsTotals();
            var bingProvider = new SearchFight.Domain.SearchResultsTotals();

            var searchTerm1 = ".net";
            googleProvider.Add(searchTerm1, 30);
            bingProvider.Add(searchTerm1, 20);

            var searchTerm2 = "java";
            googleProvider.Add(searchTerm2, 10);
            bingProvider.Add(searchTerm2, 10);

            var searchTerm3 = "ruby";
            googleProvider.Add(searchTerm3, 15);
            bingProvider.Add(searchTerm3, 25);

            var searchFight = new SearchFight.Domain.SearchFight();

            searchFight.AddProviderSearchResultsTotals("google", googleProvider);
            searchFight.AddProviderSearchResultsTotals("bing", bingProvider);

            var searchFightWinner = searchFight.RunSearchFight();

            Assert.Equal(searchTerm1, searchFightWinner);
        }
    }
}
