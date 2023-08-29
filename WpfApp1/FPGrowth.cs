using Google.Protobuf.WellKnownTypes;
using MySqlX.XDevAPI.Common;
using MySqlX.XDevAPI.CRUD;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Documents;


public class FPGrowth
{
    public static (List<List<double>>, List<double>, int ,long) firstSelections(List<double>[] csvData, double minSupport)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        int step = 0;
        int transactionCount = csvData.Count();
        List<double> uniqueItem = new List<double>();
        foreach (var item in csvData)
        {
            foreach (var value in item)
            {
                step++;
                if (!uniqueItem.Contains(value))
                {
                    uniqueItem.Add(value);
                }
            }
        }
        var calculateSupportValue = calculateSupport(uniqueItem, csvData,step);
        List<Tuple<double, double>> uniqueItemWithSupport = calculateSupportValue.Item1;
        step = calculateSupportValue.Item2;
        var cleanBaseItemValue = cleanBaseItem(uniqueItemWithSupport, minSupport,step);
        List<Tuple<double, double>> tempBaseItem = cleanBaseItemValue.Item1;
        step = cleanBaseItemValue.Item2;
        List<Tuple<double, double>> baseItem = sortBaseItem(tempBaseItem);
        var cleanCsvDataValue = cleaningCsvData(csvData, baseItem,step);
        List<List<double>> cleanCsvData = cleanCsvDataValue.Item1;
        step = cleanCsvDataValue.Item2;
        var sortCleanCsvDataValue = sortingCleanCsvData(cleanCsvData, baseItem,step);
        List<List<double>> sortCleanCsvData = sortCleanCsvDataValue.Item1;
        step = sortCleanCsvDataValue.Item2;
        var baseForRuleValue = basisForRule(sortCleanCsvData,baseItem, step);
        List<List<Tuple<double, double>>> baseForRule = baseForRuleValue.Item1;
        step = baseForRuleValue.Item2;
        var calculatedDataValue = calculateData(baseForRule,step);
        List<List<Tuple<double, double>>> calculatedData = calculatedDataValue.Item1;
        step = calculatedDataValue.Item2;
        List<List<double>> associationRule = new List<List<double>>();
        List<double> supportRule = new List<double>();
        for (int a = baseItem.Count - 1; a >= 0; a--)
        {
            step++;
            var result2Value = searchForItem(calculatedData, baseItem[a].Item1, transactionCount, minSupport,step);
            List<List<Tuple<double, double>>> result2 = result2Value.Item1;
            step = result2Value.Item2;
            var result4Value = uniqueElementFromBase(result2,step);
            List<Tuple<double, double>> result4 = result4Value.Item1;
            step= result4Value.Item2;
            var rs1 = CalcSupport(result4, baseItem[a],transactionCount, minSupport,step);
            List<double> rule = rs1.Item1;
            double tempSupportRule = rs1.Item2;
            step = rs1.Item3;
            if (rule.Count > 1)
            {   associationRule.Add(rule);
                supportRule.Add(tempSupportRule);}}
        stopwatch.Stop();
        long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
        return (associationRule, supportRule, step, elapsedMilliseconds);
    }


    public static (List<Tuple<double, double>>,int) calculateSupport(List<double> uniqueItem, List<double>[] csvData,int step)
    {
        List <Tuple<double, double>> uniqueItemWithSupport = new List<Tuple<double, double>>();
        
        for (int i=0; i<uniqueItem.Count; i++)
        {
            double temporaryCount = 0;
            foreach (var item in csvData)
            {
                step++;
                if (item.Contains(uniqueItem[i]))
                {
                    temporaryCount +=1;
                }
            }
            uniqueItemWithSupport.Add(Tuple.Create(uniqueItem[i], Math.Round(temporaryCount / csvData.Count(),3)));
        }
       
        return (uniqueItemWithSupport, step);
    }

    public static (List<Tuple<double, double>>,int) cleanBaseItem(List<Tuple<double, double>> uniqueItemWithSupport, double support,int step)
    {
        List<Tuple<double, double>> baseItem = new List<Tuple<double, double>>();
        foreach(var item in uniqueItemWithSupport)
        {
            step++;
            if(item.Item2 >= support)
            {
                baseItem.Add(item);
            }

        }
        return (baseItem,step);
    }

    public static List<Tuple<double, double>> sortBaseItem(List<Tuple<double, double>> uniqueItemWithSupport)
    {
        uniqueItemWithSupport.Sort((x, y) => y.Item2.CompareTo(x.Item2));

        return uniqueItemWithSupport;
    }

    public static List<List<double>> convertCsvData(List<double>[] array)
    {
        List<List<double>> convertCsvData = new List<List<double>>();
        foreach (List<double> sublist in array)
        {
            convertCsvData.Add(sublist);
        }
        return convertCsvData;
    }

    public static (List<List<double>>,int) cleaningCsvData(List<double>[] csvData, List<Tuple<double, double>> uniqueItemWithSupport, int step)
    {
        List<List<double>> newCsvData = convertCsvData(csvData);

        List<List<double>> cleanCsvData = new List<List<double>>();
        bool isRemove = false;

        foreach(var item in newCsvData)
        {
            List<double> cleanedItem = new List<double>();
            foreach (var value in item)
            {
                step++;
                if (uniqueItemWithSupport.Any(tuple => tuple.Item1 == value))
                {
                    cleanedItem.Add(value);
                    isRemove = true;
                }
            }
            if (isRemove)
            {
                cleanCsvData.Add(cleanedItem);
                isRemove= false;
            }
        }


        return (cleanCsvData,step);
    }

    public static (List<List<double>>, int) sortingCleanCsvData(List<List<double>> csvData, List<Tuple<double, double>> uniqueItemWithSupport,int step)
    {
        foreach (var list in csvData)
        {
            step++;
            list.Sort((a, b) =>
            {
                double aValue = uniqueItemWithSupport.FindIndex(t => t.Item1 == a);
                double bValue = uniqueItemWithSupport.FindIndex(t => t.Item1 == b);
                return aValue.CompareTo(bValue);
            });
        }
        return (csvData.OrderByDescending(list => list.Count).ToList(), step);
    }


    public static (List<List<Tuple<double, double>>>, int) basisForRule(List<List<double>> csvData, List<Tuple<double, double>> baseItem, int step)
    {

        List<List<Tuple<double, double>>> result = new List<List<Tuple<double, double>>>();
        List<Tuple<double, double>> tempTransaction = new List<Tuple<double, double>>();
        foreach(var value in csvData[0])
        {
            tempTransaction.Add(Tuple.Create(value,1.0));
            step++;
        }
        result.Add(tempTransaction);
        tempTransaction = new List<Tuple<double, double>>();
        bool t= false;
        int b2=0;
        for (int y = 1;y < csvData.Count;y++)
        {for (int i=0; i < csvData[y].Count;i++)
            {
                step++;
                if (csvData[y][i] == result[0][i].Item1){
                    result[0][i] = Tuple.Create(result[0][i].Item1, result[0][i].Item2+1);}
                else if (csvData[y][i] != result[0][i].Item1)
                {for (int c = 1; c < result.Count; c++)
                    {for (int b = 0; b <= i; b++)
                        {if (csvData[y][b] != result[c][b].Item1){
                                t = false;
                                break;
                            }else{
                                t= true;
                                b2 = b;
                            }}
                        if (t){
                            result[c][b2] = Tuple.Create(result[c][b2].Item1, result[c][b2].Item2+1); 
                            break;
                        }}
                    if (t){
                        t = false;
                    }else{
                        for (int x = 0; x < csvData[y].Count; x++)
                        {if (x < i)
                            {
                                tempTransaction.Add(Tuple.Create(csvData[y][x], 0.0));
                            }
                            else
                            {
                                tempTransaction.Add(Tuple.Create(csvData[y][x], 1.0));
                            }
                        }
                        result.Add(tempTransaction);
                        tempTransaction = new List<Tuple<double, double>>();
                        break;
                    }}}}
        return (result,step);
    }

    public static (List<List<Tuple<double, double>>>,int) searchForItem(List<List<Tuple<double, double>>> csvData, double baseItem, int transactionCount, double support, int step)
    {
        List<List<Tuple<double, double>>> result = new List<List<Tuple<double, double>>>();
        List<Tuple<double, double>> temp = null;
        foreach (var elem in csvData)
        {
            for(int i=0;i<elem.Count;i++)
            {
                step++;
                if (elem[i].Item1 == baseItem)
                {
                    temp = new List<Tuple<double, double>>();
                    for (int y = 0; y != i+1; y++)
                    {
                        temp.Add(Tuple.Create(elem[y].Item1, elem[y].Item2));
                    }
                    
                    break;
                }
            }
            if (temp != null)
            {
                result.Add(temp);
                temp = null;
            }
        }
            return (result,step);
    }


    public static (List<Tuple<double, double>>, int) uniqueElementFromBase(List<List<Tuple<double, double>>> csvData, int step)
    {
        List<Tuple<double, double>> result = new List<Tuple<double, double>>();

        foreach (var sublist in csvData)
        {
            for (int i=0;i<sublist.Count;i++)
            {
                if (!result.Any(t => t.Item1 == sublist[i].Item1))
                {
                    result.Add(Tuple.Create(sublist[i].Item1, sublist[i].Item2));
                }
            }
        }


        return (result, step);
    }


    public static (List<List<Tuple<double, double>>>,int) calculateData(List<List<Tuple<double, double>>> csvData,int step)
    {
        int b = 0;
        double count = 0;

        for (int f = 0; f < csvData[0].Count; f++)
        {
            for (int a = 0; a < csvData.Count - 1; a++)
            {
                step++;
                if (csvData[a].Count > f && csvData[a+1].Count > f)
                {
                    if (csvData[a][f].Item1 == csvData[a + 1][f].Item1)
                    {
                        count += csvData[a][f].Item2;
                    }
                    else
                    {
                        count += csvData[a][f].Item2;
                        for (int y = b; y <= a; y++)
                        {
                            csvData[y][f] = Tuple.Create(csvData[y][f].Item1, count);
                        }
                        b = a;
                        b++;
                        count = 0;
                    }
                }
            }
            b = 0;
        }
        return (csvData,step);
    }

    public static (List<double>, double,int) CalcSupport(List<Tuple<double, double>> csvData,Tuple<double,double> baseItem,int transactionCount, double support, int step)
    {
        List<double> rule = new List<double>();

        foreach (var elem in csvData)
        {
            step++;
            if (elem.Item2/(double)transactionCount>=support)
            {
                rule.Add(elem.Item1);
            }
        }
        return (rule, baseItem.Item2,step);
    }



    public static List<double> calcSupportForAll(List<List<Tuple<double, double>>> csvData, double baseItem, int transactionCount, double minSupport)
    {
        List<Tuple<List<double>, double>> result = new List<Tuple<List<double>, double>>();

        List<double> uniqueItem = new List<double>();

        foreach (var item in csvData)
        {
            foreach (var value in item)
            {
                if (!uniqueItem.Contains(value.Item1))
                {
                    uniqueItem.Add(value.Item1);
                }
            }
        }
        List<Tuple<double,double>> values = new List<Tuple<double,double>>();
        
        foreach(var item in uniqueItem)
        {
            values.Add(Tuple.Create(item,0.0));
        }

        for (int i = 0; i < values.Count; i++)
        {
            foreach (var value in csvData)
            {
                foreach (var elem in value)
                {
                    if (elem.Item1 == values[i].Item1)
                    {
                        if (elem.Item2!=0) {
                            values[i] = Tuple.Create(values[i].Item1, values[i].Item2 + 1);
                        }
                    }
                }

            }
        }
        List<double> associationRule = new List<double>();
        associationRule.Add(baseItem);
        for (int a=0; a<values.Count; a++)
        {
            if (values[a].Item2/transactionCount>=minSupport)
            {
                associationRule.Add(values[a].Item1);
            }
        }

        return associationRule;
    }
}
