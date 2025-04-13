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
        // Verifica si el objeto que colisiona es el jugador y si aun no ha hecho daño
        if (other.CompareTag("Player") && !yaHizoDanio)
        {
            WizardMovement wizardScript = Wizard.GetComponent<WizardMovement>();
            if (wizardScript != null)
            {
                wizardScript.RecibirDanio(10); // Llama al método de recibir daño en el script del jugador
                yaHizoDanio = true; // Marca que ya se ha hecho daño
            }
        }
    }
    
    /*{
        WizardMovement wizardScript = Wizard.GetComponent<WizardMovement>();
        // Verifica si el objeto que colisiona es el jugador y si aun no ha hecho daño
        if (Wizard != null)
        {
            
            if (wizardScript != null && !yaHizoDanio)
            {
                wizardScript.RecibirDanio(2); 
                yaHizoDanio = true;  
            }
        }
    }
    */
}
