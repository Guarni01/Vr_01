using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float DestroyTime = 3f; 
    void Start()
    {
        Invoke("DestroyBullet", DestroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void DestroyBullet()
    {
        Destroy(gameObject); 
    }
     void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Zombie") 
        {
             collision.gameObject.GetComponent<ZombieNav>().ZombieColpito(); // Chiama la funzione ZombieColpito sull'oggetto con cui il proiettile collide
        }   
    }
}
