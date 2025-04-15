using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class RecordManager
{
    const string KEY = "records";

    public static void AgregarRecord(Record nuevo)
    {
        List<Record> records = ObtenerRecords();
        records.Add(nuevo);
        records = records.OrderBy(r => r.tiempo).Take(10).ToList(); // Solo los 10 mejores

        string json = JsonUtility.ToJson(new ListaRecords(records));
        PlayerPrefs.SetString(KEY, json);
        PlayerPrefs.Save();
    }

    public static List<Record> ObtenerRecords()
    {
        if (!PlayerPrefs.HasKey(KEY)) return new List<Record>();

        string json = PlayerPrefs.GetString(KEY);
        return JsonUtility.FromJson<ListaRecords>(json).records;
    }

    [System.Serializable]
    private class ListaRecords
    {
        public List<Record> records;

        public ListaRecords(List<Record> records)
        {
            this.records = records;
        }
    }
}
