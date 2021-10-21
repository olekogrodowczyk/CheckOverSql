using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IDataComparer
    {
        bool compareValues(List<List<string>> values1, List<List<string>> values2);
    }
}
