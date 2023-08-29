using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Windows;

public class AprioriAlgorithm
{
    private static int StepCounter(int step)
    {
        step++;
        return step;
    }

    public static Tuple<List<List<double>>, List<List<double>>, int, long> RunApriori(List<double>[] transactions, double minSupport, double minConfidence)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        int step = 0;
        var result = GenerateUniqueItems(transactions, step);
        List<double> uniqueItems = result.Item1;
        step = result.Item2;

        List<List<double>> frequentItemsets = new List<List<double>>();
        var result2 = GenerateFrequentItemsetsOfLength1(uniqueItems, transactions, minSupport, step);
        List<List<double>> frequentItemsetsOfLength1 = result2.Item1;
        step = result2.Item2;

        frequentItemsets.AddRange(frequentItemsetsOfLength1);

        int k = 2;

        while (frequentItemsetsOfLength1.Any())
        {
            step++;
            var result3 = GenerateCandidateItemsets(frequentItemsetsOfLength1, k, step);
            List<List<double>> candidateItemsets = result3.Item1;
            step = result3.Item2;
            var result4 = PruneInfrequentItemsets(candidateItemsets, transactions, minSupport, step);
            List<List<double>> frequentItemsetsOfLengthK = result4.Item1;
            step = result4.Item2;
            frequentItemsets.AddRange(frequentItemsetsOfLengthK);
            frequentItemsetsOfLength1 = frequentItemsetsOfLengthK;
            k++;
        }

        var result5 = GenerateAssociationRules(frequentItemsets, transactions, minConfidence, step);

        stopwatch.Stop();
        long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

        return Tuple.Create(result5.Item1, result5.Item2, result5.Item3, elapsedMilliseconds);
    }

    private static Tuple<List<double>, int> GenerateUniqueItems(List<double>[] transactions, int step)
    {
        List<double> uniqueItems = new List<double>();

        foreach (var transaction in transactions)
        {
            foreach (var item in transaction)
            {
                step = StepCounter(step);
                if (!uniqueItems.Contains(item))
                {
                    uniqueItems.Add(item);
                }
            }
        }

        return Tuple.Create(uniqueItems, step);
    }

    private static Tuple<List<List<double>>, int> GenerateFrequentItemsetsOfLength1(List<double> uniqueItems, List<double>[] transactions, double minSupport, int step)
    {
        List<List<double>> frequentItemsets = new List<List<double>>();

        foreach (var item in uniqueItems)
        {
            double support = CalculateSupport(new List<double> { item }, transactions, step).Item1;
            step = StepCounter(step);
            if (support >= minSupport)
            {
                frequentItemsets.Add(new List<double> { item });
            }
        }


        return Tuple.Create(frequentItemsets, step);
    }

    private static Tuple<List<List<double>>, int> GenerateCandidateItemsets(List<List<double>> frequentItemsets, int k, int step)
    {
        List<List<double>> candidateItemsets = new List<List<double>>();

        int numFrequentItemsets = frequentItemsets.Count;

        for (int i = 0; i < numFrequentItemsets; i++)
        {
            for (int j = i + 1; j < numFrequentItemsets; j++)
            {
                var itemset1 = frequentItemsets[i];
                var itemset2 = frequentItemsets[j];
                step = StepCounter(step);
                if (AreItemsetsJoinable(itemset1, itemset2, k - 1, step).Item1)
                {
                    step = AreItemsetsJoinable(itemset1, itemset2, k - 1, step).Item2;
                    var joinedItemset = JoinItemsets(itemset1, itemset2, step).Item1;
                    candidateItemsets.Add(joinedItemset);
                }
            }
        }

        return Tuple.Create(candidateItemsets, step);
    }

    private static Tuple<bool, int> AreItemsetsJoinable(List<double> itemset1, List<double> itemset2, int k, int step)
    {
        for (int i = 0; i < k - 1; i++)
        {
            step = StepCounter(step);
            if (itemset1[i] != itemset2[i])
            {
                return Tuple.Create(false, step);
            }
        }

        return Tuple.Create(true, step);
    }

    private static Tuple<List<double>, int> JoinItemsets(List<double> itemset1, List<double> itemset2, int step)
    {
        List<double> joinedItemset = new List<double>(itemset1);
        joinedItemset.Add(itemset2.Last());

        return Tuple.Create(joinedItemset, step);
    }

    private static Tuple<List<List<double>>, int> PruneInfrequentItemsets(List<List<double>> candidateItemsets, List<double>[] transactions, double minSupport, int step)
    {
        List<List<double>> frequentItemsets = new List<List<double>>();

        foreach (var candidateItemset in candidateItemsets)
        {
            step = StepCounter(step);
            double support = CalculateSupport(candidateItemset, transactions, step).Item1;
            if (support >= minSupport)
            {
                frequentItemsets.Add(candidateItemset);
            }
        }

        return Tuple.Create(frequentItemsets, step);
    }

    public static Tuple<double, int> CalculateSupport(List<double> itemset, List<double>[] transactions, int step)
    {
        int count = 0;

        foreach (var transaction in transactions)
        {
            step = StepCounter(step);
            if (IsItemsetContainedInTransaction(itemset, transaction, step).Item1)
            {
                count++;
            }
        }

        double support = (double)count / transactions.Length;

        return Tuple.Create(support, step);
    }

    private static Tuple<bool, int> IsItemsetContainedInTransaction(List<double> itemset, List<double> transaction, int step)
    {
        foreach (var item in itemset)
        {
            step = StepCounter(step);
            if (!transaction.Contains(item))
            {
                return Tuple.Create(false, step);
            }
        }

        return Tuple.Create(true, step);
    }

    private static Tuple<List<List<double>>, List<List<double>>, int> GenerateAssociationRules(List<List<double>> frequentItemsets, List<double>[] transactions, double minConfidence, int step)
    {
        List<List<double>> antecedent = new List<List<double>>();
        List<List<double>> consequent = new List<List<double>>();

        foreach (var itemset in frequentItemsets)
        {
            int length = itemset.Count;

            if (length > 1)
            {
                List<List<double>> subsets = GenerateSubsets(itemset, step).Item1;

                foreach (var subset in subsets)
                {
                    step = StepCounter(step);
                    List<double> remainingItems = itemset.Except(subset).ToList();
                    double confidence = CalculateConfidence(subset, remainingItems, transactions, step);
                    if (confidence >= minConfidence)
                    {
                        antecedent.Add(subset);
                        consequent.Add(remainingItems);
                    }
                }
            }
        }

        List<Tuple<List<double>, List<double>>> tuples = consequent.Zip(antecedent, (x, y) => Tuple.Create(x, y)).ToList();
        tuples.RemoveAll(tuple => tuple.Item1.Count == 0);
        consequent = tuples.Select(tuple => tuple.Item1).ToList();
        antecedent = tuples.Select(tuple => tuple.Item2).ToList();
        RemoveDuplicates(antecedent, consequent);
        for(int i = consequent.Count - 1; i >= 0; i--)
        {
            if (consequent[i].Contains(0.0) || antecedent[i].Contains(0.0))
            {
                consequent.RemoveAt(i);
                antecedent.RemoveAt(i);
            }
        }


        return Tuple.Create(antecedent, consequent, step);
    }

    static void RemoveDuplicates(List<List<double>> a1, List<List<double>> b2)
    {
        HashSet<string> set = new HashSet<string>();

        for (int i = 0; i < a1.Count; i++)
        {
            List<double> list = a1[i];
            string key = GetKey(list);

            if (set.Contains(key))
            {
                a1.RemoveAt(i);
                b2.RemoveAt(i);
                i--;
            }
            else
            {
                set.Add(key);
            }
        }
    }

    static string GetKey(List<double> list)
    {
        list.Sort(); 
        return string.Join(",", list);
    }


private static Tuple<List<List<double>>, int> GenerateSubsets(List<double> itemset, int step)
    {
        List<List<double>> subsets = new List<List<double>>();

        int length = itemset.Count;

        foreach (var item in itemset)
        {
            subsets.Add(new List<double> { item });
        }

        for (int i = 2; i <= length; i++)
        {
            step = StepCounter(step);
            var combs = Combinations(itemset, i, step).Item1;
            subsets.AddRange(combs);
        }

        return Tuple.Create(subsets, step);
    }

    private static Tuple<IEnumerable<List<double>>, int> Combinations(List<double> itemset, int k, int step)
    {
        if (k == 1)
        {
            var combinations2 = itemset.Select(item => new List<double> { item });
            return Tuple.Create(combinations2, step);
        }

        IEnumerable<List<double>> result = new List<List<double>>();

        for (int i = 0; i < itemset.Count; i++)
        {
            step = StepCounter(step);
            double current = itemset[i];

            List<double> rest = new List<double>(itemset);
            rest.RemoveAt(i);

            var subCombinations = Combinations(rest, k - 1, step);
            result = result.Concat(subCombinations.Item1.Select(comb => new List<double> { current }.Concat(comb).ToList()));
        }

        return Tuple.Create(result, step);
    }

    public static double CalculateConfidence(List<double> antecedent, List<double> consequent, List<double>[] transactions, int step)
    {
        List<double> combined = new List<double>(antecedent);
        combined.AddRange(consequent);

        double supportAntecedent = CalculateSupport(antecedent, transactions, step).Item1;
        double supportCombined = CalculateSupport(combined, transactions, step).Item1;

        double confidence = supportCombined / supportAntecedent;

        return confidence;
    }
}
