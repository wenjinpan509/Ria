namespace ATM
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] amounts = { 30, 50, 60, 80, 100, 140, 230, 370, 610, 980 };

            foreach (var amount in amounts)
            {
                Console.WriteLine($"Avaliable combinations for {amount}:");
                var results = PayoutCalculator.GetCombinations(amount);
                PrintResults(results);
                Console.WriteLine();
            }

        }
        static void PrintResults(List<List<DenominationCount>> results)
        {
            foreach (var combo in results)
            {
                Console.WriteLine(string.Join(" + ", combo));
            }
        }
    }
}
