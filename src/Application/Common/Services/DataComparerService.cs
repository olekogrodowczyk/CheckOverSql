using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class DataComparerService : IDataComparerService
    {
        public Task<bool> compareValues(List<List<string>> values1, List<List<string>> values2)
        {
            if (values1.Count() != values2.Count()) { return Task.FromResult(false); }

            int rowCount = values1[0].Count();

            for (int i = 0; i < values1.Count(); i++)
            {
                if (!values1[i].SequenceEqual(values2[i])) { return Task.FromResult(false); }
            }

            return Task.FromResult(true);
        }
    }
}
