using UnityEngine;
using UnityEngine.InputSystem;
public class GunGrabBehaviourScript : MonoBehaviour
{
    [Header("Grab Parameters")]
    private GameObject temporaryGunObject;
    public InputAction GrabAction;
    private Transform grabbedObject;
    public Transform GunSocketTransform;
    private bool grabOnce = false;
    [Header("Shooting Parameters")]
    public Transform ShootPoint;
    public Rigidbody BulletPrefab;
    public float ShootForce = 100f;
    public InputAction ShootAction;
    bool shootOnce = false;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GrabAction.Enable();
        ShootAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if(temporaryGunObject != null && GrabAction.IsPressed())
        {
            if(!grabOnce)
            {
                grabbedObject = temporaryGunObject.transform;
                grabbedObject.parent = transform;
                grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
                grabbedObject.localPosition = GunSocketTransform.localPosition;
                grabbedObject.localRotation = GunSocketTransform.localRotation;
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
            temporaryGunObject = other.transform.gameObject;
        }
        
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Gun")
        {
            temporaryGunObject = null;
        }
    }
    void FixedUpdate()
    {
        if (grabbedObject != null && ShootAction.IsPressed())
        {
            if (!shootOnce)
            {
                
                ShootBullet();
                shootOnce = true;
                
            } 
        }
        else
        {
            shootOnce = false;
        }
    }
    void ShootBullet()
    {  
            GameObject spawnedBullet = Instantiate(BulletPrefab.gameObject);
            spawnedBullet.transform.position = ShootPoint.position; //spawnedBullet è un GameObject, quindi accediamo alla sua posizione tramite transform.position
            spawnedBullet.transform.rotation = ShootPoint.rotation; // ShootPoint è un Transform, quindi accediamo a lui anche senza bisogno di .transform                                   
            spawnedBullet.GetComponent<Rigidbody>().AddForce(ShootPoint.forward * ShootForce, ForceMode.Impulse); // ShootPoint.forward è un Vector3 che rappresenta la direzione in cui il proiettile deve essere sparato
        
    }
}
