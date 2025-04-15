using UnityEngine;

[System.Serializable]
public class Record
{
    public string nombre;
    public int tiempo;

    public Record(string nombre, int tiempo)
    {
        this.nombre = nombre;
        this.tiempo = tiempo;
    }
}

