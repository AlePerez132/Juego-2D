using UnityEngine;
using UnityEngine.UI; 


public class ButtonManager : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PlayerPrefs.HasKey("vidaMago"))
        {
            PlayerPrefs.SetInt("vidaMago", 100); //ESTO SE DEBERIA MODIFICAR EN FUNCION DE LA DIFICULTAD
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ExitGame()
    {
        Application.Quit();
        // Esto solo funciona en el juego compilado.
        // En el editor de Unity, puedes usar:
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void ApareceMenuOpciones()
    {
        GameObject popup = GameObject.Find("Menu opciones");
        RectTransform rectTransform = popup.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0, 150);
    }

    public void CerrarMenuOpciones()
    {
        GameObject popup = GameObject.Find("Menu opciones");
        RectTransform rectTransform = popup.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0, 1000);
    }

    public void AparecePopupSalir()
    {
        GameObject popup = GameObject.Find("Pop-up salir");
        RectTransform rectTransform = popup.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0, 50);
    }

    public void CerrarPopupSalir(){
        GameObject popup = GameObject.Find("Pop-up salir");
        RectTransform rectTransform = popup.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(1000, 1000);
    }

    public void AparecePopupVolumen()
    {
        GameObject popup = GameObject.Find("Pop-up volumen");
        RectTransform rectTransform = popup.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0, 120);
        CerrarMenuOpciones();
    }

    public void CerrarPopupVolumen()
    {
        GameObject popup = GameObject.Find("Pop-up volumen");
        RectTransform rectTransform = popup.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(-1500, 1500);
        ApareceMenuOpciones();
    }

    public void AparecePopupDificultad()
    {
        GameObject popup = GameObject.Find("Pop-up dificultad");
        RectTransform rectTransform = popup.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(-0, 80);
        CerrarMenuOpciones();
    }

    public void CerrarPopupDificultad()
    {
        GameObject popup = GameObject.Find("Pop-up dificultad");
        RectTransform rectTransform = popup.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(-1500, 1500);
        ApareceMenuOpciones();
    }
}
