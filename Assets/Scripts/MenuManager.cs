using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Text newGameText; 

    void Start()
    {
        if (newGameText != null)
        {
            newGameText.text = "Nueva Partida";
        }
        else
        {
            Debug.LogError("No se encontró el componente Text.");
        }
    }
}
