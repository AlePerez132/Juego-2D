using UnityEngine;
using UnityEngine.UI;


public class ButtonManager : MonoBehaviour
{
    public string dificultad;

    public GameObject botonFacil;
    public GameObject botonNormal;
    public GameObject botonDificil;

    Button btnFacil;
    Button btnNormal;
    Button btnDificil;

    //200 de vida en facil, 100 en normal y 50 en dificil
    void Start()
    {

        btnFacil = botonFacil.GetComponent<Button>();
        btnNormal = botonNormal.GetComponent<Button>();
        btnDificil = botonDificil.GetComponent<Button>();

        btnFacil.interactable = true;
        btnNormal.interactable = true;
        btnDificil.interactable = true;


        if (!PlayerPrefs.HasKey("vidaMax"))
        {
            dificultad = "facil";
        }
        else
        {
            int vidaMax = PlayerPrefs.GetInt("vidaMax");
            switch (vidaMax)
            {
                case 200:
                    dificultad = "facil";
                    break;
                case 100:
                    dificultad = "normal";
                    break;
                case 50:
                    dificultad = "dificil";
                    break;
            }
        }


        switch (dificultad)
        {
            case "facil":
                btnFacil.interactable = false;
                break;
            case "normal":
                btnNormal.interactable = false;
                break;
            case "dificil":
                btnDificil.interactable = false;
                break;
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

    public void CerrarPopupSalir()
    {
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

    public void PulsarBotonFacil()
    {
        dificultad = "facil";
        btnNormal.interactable = true;
        btnFacil.interactable = false;
        btnDificil.interactable = true;
    }

    public void PulsarBotonNormal()
    {
        dificultad = "normal";
        btnNormal.interactable = false;
        btnFacil.interactable = true;
        btnDificil.interactable = true;
    }

    public void PulsarBotonDificil()
    {
        dificultad = "dificil";
        btnNormal.interactable = true;
        btnFacil.interactable = true;
        btnDificil.interactable = false;
    }
}
