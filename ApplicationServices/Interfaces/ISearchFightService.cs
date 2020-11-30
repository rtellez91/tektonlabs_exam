﻿using SearchFight.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SearchFight.ApplicationServices.Interfaces
{
    public interface ISearchFightService
    {
        Task<SearchFightResultsDto> RunFight(IEnumerable<string> searchTerms);
    }
}
