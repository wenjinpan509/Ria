using System;
using ATM.Config;

namespace ATM
{
    public class PayoutCalculator
    {
        public static List<List<DenominationCount>> GetCombinations(int amount)
        {
            var results = new List<List<DenominationCount>>();
            int[] currentCounts = new int[Config.Config.Denominations.Length];
            
            void FindCombinations(int index, int remaining)
            {
                if (remaining == 0)
                {
                    var combination = new List<DenominationCount>();
                    for (int i = 0; i < Config.Config.Denominations.Length; i++)
                    {
                        if (currentCounts[i] > 0)
                        {
                            combination.Add(new DenominationCount
                            {
                                Denomination = Config.Config.Denominations[i],
                                Count = currentCounts[i]
                            });
                        }
                    }
                    results.Add(combination);
                    return;
                }

                if (index >= Config.Config.Denominations.Length || remaining < 0)
                    return;

                int denom = Config.Config.Denominations[index];
                int maxCount = remaining / denom;

                for (int count = 0; count <= maxCount; count++)
                {
                    currentCounts[index] = count;
                    FindCombinations(index + 1, remaining - count * denom);
                    currentCounts[index] = 0;
                }
            }

            FindCombinations(0, amount);
            return results;
        }

     }
}
