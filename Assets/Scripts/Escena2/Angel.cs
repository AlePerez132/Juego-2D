using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;

public class Angel : MonoBehaviour
{
    public GameObject vallas;
    private Vector3 posicionObjetivoVallas = new Vector3(0, -4, 10f);
    public GameObject escaleras;
    private Vector3 posicionObjetivoEscaleras = new Vector3(0, 0, 100f);

    public CameraScript camara;

    public GameObject Trigger;
    public bool bossFight = false;
    private bool corrutinaActiva = false;
    private bool esperaInicialCompletada = false;
    private bool plataformasActivas = false;
    private bool muerto = false;

    public GameObject[] swordPrefabs;  // Array con los 3 prefabs de espadas

    public GameObject plataformas;

    public Vector3 plataformasAbajo;
    public Vector3 plataformasArriba;
    public float duracionMovimiento = 4f;

    private Trigger triggerScript;
    private Animator anim;
    private int contador = 0;

    int vida = 30;
    AudioManager audioManager;

    private bool procesoMuerteActivo = false;
    private bool corrutinaVallasTerminada = false;
    private bool corrutinaEscalerasTerminada = false;

    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        plataformasAbajo = new Vector3(6.7f, -10f, 0f);
        plataformasArriba = new Vector3(6.7f, -2.3f, 0f);
        triggerScript = Trigger.GetComponent<Trigger>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        
        bossFight = triggerScript.bossFight;

        if (bossFight && !corrutinaActiva && !plataformasActivas && !muerto)
        {
            StartCoroutine(BossFightSequence());
        }
        else if (muerto && !procesoMuerteActivo)
        {
            PostMuerte();
            procesoMuerteActivo = true;
        }
    }

    private IEnumerator BossFightSequence()
    {
        corrutinaActiva = true;

        if (!esperaInicialCompletada)
        {
            yield return new WaitForSeconds(2.5f);
            esperaInicialCompletada = true;
        }

        int randomIndex = Random.Range(0, swordPrefabs.Length);
        GameObject swordPattern = Instantiate(swordPrefabs[randomIndex], new Vector3(24.6f, -1, 0f), Quaternion.identity);

        yield return new WaitForSeconds(0.75f);

        anim.SetTrigger("Attack");

        Rigidbody2D[] swords = swordPattern.GetComponentsInChildren<Rigidbody2D>();
        foreach (Rigidbody2D sword in swords)
        {
            sword.gravityScale = 1.8f;
        }

        yield return new WaitForSeconds(2f);

        Destroy(swordPattern);
        contador++;
        if (contador == 4)
        {
            plataformasActivas = true;
            contador = 0;
            StartCoroutine(MoverPlataformas());
        }

        corrutinaActiva = false;
    }

    private IEnumerator MoverPlataformas()
    {
        plataformasActivas = true; 

        float duracion = duracionMovimiento;
        float elapsed = 0f;

        // Movimiento de subida
        while (elapsed < duracion)
        {
            float t = elapsed / duracion;
            plataformas.transform.position = Vector3.Lerp(plataformasAbajo, plataformasArriba, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        plataformas.transform.position = plataformasArriba;

        // Esperar 10 segundos arriba
        yield return new WaitForSeconds(8);

        // Movimiento de bajada
        elapsed = 0f;
        while (elapsed < duracion)
        {
            float t = elapsed / duracion;
            plataformas.transform.position = Vector3.Lerp(plataformasArriba, plataformasAbajo, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        plataformas.transform.position = plataformasAbajo;

        plataformasActivas = false; // Desactivamos el estado de plataformas al finalizar el movimiento
    }

    public void RecibirDanio(int cantidad)
    {
        vida -= cantidad;
        if (vida <= 0)
        {
            Morir();
        }
        else
        {
            //audioManager.reproducirEfecto(audioManager.angelRecibirDaño); 
            //aun no esta puesto este efecto de sonido
        }
    }

    void Morir()
    {
        anim.SetTrigger("Death");
        audioManager.reproducirEfecto(audioManager.angelMuerte);
        if (corrutinaVallasTerminada && corrutinaEscalerasTerminada)
        {
            Destroy(gameObject, 0.8f);
            muerto = true;
        }
    }

    public void PostMuerte()
    {
        //para mover las vallas a su posicion original
        MoverObjeto(vallas, vallas.transform.position, posicionObjetivoVallas, 0.5f);
        //para subir las escaleras
        MoverObjeto(escaleras, escaleras.transform.position, posicionObjetivoEscaleras, 0.5f);

        if (camara != null)
        {
            camara.AjustarAlturaYZoom(0f, 0f, 0.5f); //para devolver la camara a su estado original
        } else
        {
            Debug.LogWarning("Camara no asignada en el script Angel.");
        }
    }

    IEnumerator MoverObjeto(GameObject objeto, Vector3 inicio, Vector3 destino, float duracion)
    {
        float tiempo = 0;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            objeto.transform.position = Vector3.Lerp(inicio, destino, tiempo / duracion);
            yield return null; // Espera al siguiente frame
        }

        objeto.transform.position = destino; // Asegura la posición final
        if (objeto == vallas)
        {
            corrutinaVallasTerminada = true;
        }
        else if (objeto == escaleras)
        {
            corrutinaEscalerasTerminada = true;
        }
    }
}
