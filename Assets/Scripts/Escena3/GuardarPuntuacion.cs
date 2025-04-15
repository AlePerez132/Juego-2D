using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class GuardarPuntuacion : MonoBehaviour
{
    public TMP_InputField inputNombre;
    public TMP_Text textoTiempo;

    private int tiempoFinal;

    void Start()
    {
        tiempoFinal = PlayerPrefs.GetInt("TiempoFinal", 9999);
        textoTiempo.text = "Tu tiempo: " + tiempoFinal + " segundos";
    }

    public void Guardar()
    {
        string nombre = inputNombre.text;

        if (string.IsNullOrEmpty(nombre)) return;

        Record nuevo = new Record(nombre, tiempoFinal);
        RecordManager.AgregarRecord(nuevo);

        SceneManager.LoadScene("TablaRecords");
    }
}
