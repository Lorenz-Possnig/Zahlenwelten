using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class DataSaver
{
    private static DataSaver _instance;
    public static DataSaver Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new DataSaver();
            }
            return _instance;
        }
    }

    public DataEntry Entry { get; private set; } = null;

    public DataSaver()
    {
    }

    public void CreateEntry()
    {
        if (Entry == null) // do not overwrite
            Entry = new DataEntry();
    }

    public void Save()
    {
        if (Entry != null)
        {
            Entry.TimestampEnd = DateTime.Now;
            var path = $"{Application.persistentDataPath}/{Entry.Guid}";
            Directory.CreateDirectory(path);
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(Entry);
            File.WriteAllText($"{path}/{Entry.Guid}.json", json);
            File.WriteAllText($"{path}/Timestamps.txt", $"From: {Entry.TimestampStart}\nTo: {Entry.TimestampEnd}\nTime: {Entry.TotalTimeInSeconds} seconds");
            File.WriteAllText($"{path}/zahlenlegen_{Entry.Guid}.csv", Entry.ZahlenlegenCSV());
            File.WriteAllText($"{path}/zahlensagen_{Entry.Guid}.csv", Entry.ZahlensagenCSV());
        }
    }

    public void Reset()
    {
        Entry = null;
    }
}

[Serializable]
public class DataEntry
{
    public Guid Guid { get; } = Guid.NewGuid();
    public DateTime TimestampStart { get; } = DateTime.Now;
    private DateTime end;
    public DateTime TimestampEnd { get {
            return end;
        } set {
            end = value;
            TotalTimeInSeconds = (end - TimestampStart).Seconds;
        } }
    public int TotalTimeInSeconds { get; private set; }
    public List<DataEntryItem> ItemsZahlenlegen { get; set; } = new();
    public List<DataEntryItem> ItemsZahlensagen { get; set; } = new();

    private static string AsCSV(List<DataEntryItem> dataEntryItems)
    {
        if (!dataEntryItems.Any())
        {
            return "";
        }
        return "Start,End,TimeInSeconds,Item,Correct,Comment\n" +
            dataEntryItems.Select(entryItem => $"{entryItem.Start},{entryItem.End},{entryItem.TimeInSeconds},{entryItem.Item},{entryItem.Correct},{entryItem.Comment}")
            .Aggregate("", (a, b) => a + "\n" + b);
    }

    public string ZahlenlegenCSV() => AsCSV(ItemsZahlenlegen);

    public string ZahlensagenCSV() => AsCSV(ItemsZahlensagen);
}

[Serializable]
public class DataEntryItem
{
    public DateTime Start { get; } = DateTime.Now;
    private DateTime end;
    public DateTime End { get { return end; }
        set {
            end = value;
            TimeInSeconds = (end - Start).Seconds;
        }
    }
    public int TimeInSeconds { get; private set; }
    public string Item { get; set; }
    public bool Correct { get; set; }
    public string Comment { get; set; }
}
