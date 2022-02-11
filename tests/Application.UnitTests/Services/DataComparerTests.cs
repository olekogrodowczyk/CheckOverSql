using Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTests.Services
{
    public class DataComparerTests
    {

        [Theory]
        [InlineData(1,1,true)]
        [InlineData(1,2,false)]
        [InlineData(9,10,false)]
        [InlineData(20,20,true)]
        public async Task compareValues_TheSameDataRows_shouldReturnValidResult(int count1, int count2, bool expectedResult)
        {
            //Arrange
            List<List<string>> list = new List<List<string>>();
            List<List<string>> list2 = new List<List<string>>();

            for(int i = 0; i < count1; i++)
            {
                List<string> subList = returnDummyList(count1, "Thesamedata");
                list.Add(subList);
            }
            for(int i = 0; i < count2; i++)
            {
                List<string> subList = returnDummyList(count1, "Thesamedata");
                list2.Add(subList);
            }
            
            //Act
            DataComparerService dataComparer = new DataComparerService();
            bool result = await dataComparer.compareValues(list,list2);

            //Assert
            Assert.Equal(expectedResult, result);          
        }

        [Theory]
        [InlineData(1, 1, false)]
        [InlineData(1, 2, false)]
        public async Task compareValues_DifferentData_shouldReturnValidResult(int count1, int count2, bool expectedResult)
        {
            //Arrange
            List<List<string>> list = new List<List<string>>();
            List<List<string>> list2 = new List<List<string>>();

            for (int i = 0; i < count1; i++)
            {
                List<string> subList = returnDummyList(count1, Guid.NewGuid().ToString());
                list.Add(subList);
            }
            for (int i = 0; i < count2; i++)
            {
                List<string> subList = returnDummyList(count1, Guid.NewGuid().ToString());
                list2.Add(subList);
            }

            //Act
            DataComparerService dataComparer = new DataComparerService();
            bool result = await dataComparer.compareValues(list, list2);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        private List<string> returnDummyList(int count, string seed)
        {
            List<string> tmp = new List<string>();
            for(int i=0;i<count;i++)
            {
                tmp.Add(seed);
            }
            return tmp;
        }
    }
}
