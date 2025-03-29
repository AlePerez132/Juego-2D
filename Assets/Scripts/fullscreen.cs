using UnityEngine;
using TMPro;

public class fullscreen : MonoBehaviour
{
    GameObject textoBoton;
    TextMeshProUGUI texto;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Screen.fullScreen = true;
        textoBoton = GameObject.Find("Texto modo ventana");
        
        texto = textoBoton.GetComponent<TextMeshProUGUI>();
        if (texto == null)
        {
            Debug.Log("No se encontro el texto");
        }
        texto.SetText("MODO VENTANA");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleFullscreen(){
        if(Screen.fullScreen){
            texto.SetText("Fullscreen");
            Screen.SetResolution(1920, 1080, false);
        } else
        {
            texto.SetText("Modo Ventana");
            Screen.SetResolution(1920, 1080, true);
        }
    }
}
