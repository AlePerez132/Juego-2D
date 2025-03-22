using UnityEngine;
using UnityEngine.UI; 


public class ButtonManager : MonoBehaviour
{
    public Transform canvas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void mostrarPopUpSalir(GameObject popup){
        GameObject nuevoPopup = Instantiate(popup, canvas);
        Button[] botones = nuevoPopup.GetComponentsInChildren<Button>();

        foreach (Button boton in botones)
        {
            if (boton.name == "No" || boton.name == "Salir")
                boton.onClick.AddListener(() => DestruirBotónCerrar());
            else if (boton.name == "Si")
                boton.onClick.AddListener(() => ExitGame());
        }
    }

    public void mostrarPopUpVolumen(GameObject popup){
        GameObject nuevoPopup = Instantiate(popup, canvas);
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

    //se usa para cerrar las ventanas emergentes
    public void DestruirBotónCerrar(){
        GameObject popup = GameObject.Find("Pop-up Salir(Clone)");
        Destroy(popup);
    }
}
