using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Date
{
    public int year;
    public int month;
    public int day;
    public int hour;
    public int minute;
    public int second;
}
[SerializeField]
public class DateData : ILoader<int, Date>
{
    public List<Date> stats = new List<Date>();

    public Dictionary<int, Date> MakeDict()
    {
        Dictionary<int, Date> dict = new Dictionary<int, Date>();
        foreach (Date stat in stats)
            dict.Add(stat.year, stat);
        return dict;
    }
}
