using UnityEngine;

public class Espada : MonoBehaviour
{
    public bool yaHizoDanio = false;
    public GameObject Wizard;

    private void Start()
    {
        if (Wizard == null)
        {
            GameObject wizardEncontrado = GameObject.FindWithTag("Player");
            if (wizardEncontrado != null)
            {
                Wizard = wizardEncontrado;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica si el objeto que colisiona es el jugador y si aún no ha hecho daño
        if (Wizard != null)
        {
            WizardMovement wizardScript = Wizard.GetComponent<WizardMovement>();
            if (wizardScript != null && !yaHizoDanio)
            {
                wizardScript.RecibirDanio(1); //deberia ser 8, lo cambio pq da problemas
                yaHizoDanio = true;  
            }
        }
    }
}
