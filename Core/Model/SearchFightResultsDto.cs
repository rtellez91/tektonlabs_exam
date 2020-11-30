using System;
using System.Collections.Generic;
using System.Text;

namespace SearchFight.Core.Model
{
    public class SearchFightResultsDto
    {
        public IReadOnlyDictionary<string, IReadOnlyDictionary<string, SearchResult>> ResultsBySearchTerm { get; set; }
        public IReadOnlyDictionary<string, string> WinnerByProvider { get; set; }
        public string SearchFightWinner { get; set; }
    }
}
