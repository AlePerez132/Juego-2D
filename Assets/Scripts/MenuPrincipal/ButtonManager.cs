using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ButtonManager : MonoBehaviour
{
    public string dificultad;

    public GameObject botonFacil;
    public GameObject botonNormal;
    public GameObject botonDificil;

    Button btnFacil;
    Button btnNormal;
    Button btnDificil;

    public TMP_Text textoDificultad;

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
            PulsarBotonFacil();
            dificultad = "facil";
        }
        else
        {
            int vidaMax = PlayerPrefs.GetInt("vidaMax");
            switch (vidaMax)
            {
                case 400:
                    PulsarBotonFacil();
                    break;
                case 200:
                    PulsarBotonNormal();
                    break;
                case 100:
                    PulsarBotonDificil();
                    break;
            }
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

    public void AparecePopupControles()
    {
        GameObject popup = GameObject.Find("Pop-up controles");
        RectTransform rectTransform = popup.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(-0, 100);
        CerrarMenuOpciones();
    }

    public void CerrarPopupControles()
    {
        GameObject popup = GameObject.Find("Pop-up controles");
        RectTransform rectTransform = popup.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(-3000, 1500);
        ApareceMenuOpciones();
    }

    public void PulsarBotonFacil()
    {
        dificultad = "facil";
        textoDificultad.text = "Has elegido la dificultad fácil, empezarás con 400 de vida";
    }

    public void PulsarBotonNormal()
    {
        dificultad = "normal";
        textoDificultad.text = "Has elegido la dificultad normal, empezarás con 200 de vida";
    }

    public void PulsarBotonDificil()
    {
        dificultad = "dificil";
        textoDificultad.text = "Has elegido la dificultad difícil, empezarás con 100 de vida";
    }

    
}
