using UnityEngine;

public class CameraScript : MonoBehaviour
{   
    public GameObject Wizard;


    void Update()
    {
        Vector3 position = transform.position;
        position.x = Wizard.transform.position.x;
        position.y = Wizard.transform.position.y;
        transform.position = position;
    }
}
