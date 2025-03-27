using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UI; 


public class ButtonManager : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   public void ApareceMenuOpciones()
    {
        GameObject popup = GameObject.Find("Menu opciones");
        popup.transform.position = new Vector3(0, 150, -10);
    }

    public void CerrarMenuOpciones()
    {
        GameObject popup = GameObject.Find("Menu opciones");
        popup.transform.position = new Vector3(0, 1000, -10);
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
    public void DestruirBotonCerrar(){
        GameObject popup = GameObject.Find("Pop-up Salir");
        Destroy(popup);
    }



}
