using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class DataComparer : IDataComparer
    {
        public bool compareValues(List<List<string>> values1, List<List<string>> values2)
        {
            if (values1.Count() != values2.Count()) { return false; }

            int rowCount = values1[0].Count();

            for (int i = 0; i < values1.Count(); i++)
            {
                for (int j = 0; j < rowCount; j++)
                {
                    if (values1[i][j] != values2[i][j]) { return false; }
                }
            }
            return true;
        }
    }
}
