using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MostrarRecords : MonoBehaviour
{
    public GameObject contenedor;
    public GameObject prefabEntrada;

    void Start()
    {
        // Obtener los registros y ordenarlos de menor a mayor según el tiempo
        List<Record> records = RecordManager.ObtenerRecords();
        records.Sort((x, y) => x.tiempo.CompareTo(y.tiempo));  // Ordenar por tiempo (ascendente)

        // Limitar a los 10 primeros
        int maxRecords = Mathf.Min(records.Count, 10);

        for (int i = 0; i < maxRecords; i++)
        {
            Record r = records[i];
            GameObject entrada = Instantiate(prefabEntrada, contenedor.transform);
            entrada.GetComponentInChildren<TMP_Text>().text = (i + 1) + ". " + r.nombre + " - " + r.tiempo + "s";
        }
    }
}
