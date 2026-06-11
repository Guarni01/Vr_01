using UnityEngine;

public class ShootBulletTest : MonoBehaviour
{
    public Transform ShootPoint;
    public Rigidbody BulletPrefab;
    public float ShootForce = 100f;
    public bool ShootBullet;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        if (ShootBullet)
        {
            GameObject spawnedBullet = Instantiate(BulletPrefab.gameObject);
            spawnedBullet.transform.position = ShootPoint.position; //spawnedBullet è un GameObject, quindi accediamo alla sua posizione tramite transform.position
            spawnedBullet.transform.rotation = ShootPoint.rotation; // ShootPoint è un Transform, quindi accediamo a lui anche senza bisogno di .transform                                   
            spawnedBullet.GetComponent<Rigidbody>().AddForce(ShootPoint.forward * ShootForce, ForceMode.Impulse); // ShootPoint.forward è un Vector3 che rappresenta la direzione in cui il proiettile deve essere sparato
            ShootBullet = false;
        }
    }
}
