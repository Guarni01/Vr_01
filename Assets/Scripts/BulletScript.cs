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
        Destroy(collision.gameObject); // Distrugge l'oggetto con cui il proiettile collide
        }   
    }
}
