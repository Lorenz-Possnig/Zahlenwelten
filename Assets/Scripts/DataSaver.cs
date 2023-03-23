using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
        if (Entry == null)
            Entry = new DataEntry();
    }

    public void Save()
    {
        Entry.TimestampEnd = DateTime.Now;
        var path = $"{Application.streamingAssetsPath}/{Entry.Guid}.json";
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(Entry);
        File.WriteAllTextAsync(path, json);
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
