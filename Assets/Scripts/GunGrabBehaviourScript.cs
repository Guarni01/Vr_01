using UnityEngine;
using UnityEngine.InputSystem;
public class GunGrabBehaviourScript : MonoBehaviour
{
    public GameObject TemporaryGunObject;
    public InputAction GrabAction;
    private Transform grabbedObject;
    private bool grabOnce = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GrabAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if(TemporaryGunObject != null && GrabAction.IsPressed())
        {
            if(!grabOnce)
            {
                grabbedObject.parent = transform;
                grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
                grabbedObject.localPosition = Vector3.zero;
                grabbedObject.localRotation = Quaternion.identity;
                grabOnce = true;  
            }
         
        }
        if (grabbedObject != null && !GrabAction.IsPressed())
        {
            if(grabOnce)
            {
                grabbedObject.parent = null;
                grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
                grabbedObject = null;
                grabOnce = false;
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
       if(other.gameObject.tag == "Gun")
        {
            TemporaryGunObject = other.transform.parent.gameObject;
        }
        
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Gun")
        {
            TemporaryGunObject = null;
        }
    }
}
