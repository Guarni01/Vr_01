using UnityEngine;
using UnityEngine.AI;

public class TestUnityNav : MonoBehaviour
{ 
    public Transform TargetObject; 
    public NavMeshAgent ZombieAgent;
    public Animator ZombieAnimator;
    public float MovementThreshold = 0.1f;
    public float AttackDistanceThreshold = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ZombieAgent.SetDestination(TargetObject.position);
        if(ZombieAgent.velocity.magnitude > MovementThreshold)
        {
            ZombieAnimator.SetBool("Walk", true);
        }
        else
        {
            ZombieAnimator.SetBool("Walk", false);
        }
       
        Vector3 TargetPositionAtZombieHeight = new Vector3 (TargetObject.position.x, ZombieAgent.transform.position.y, TargetObject.position.z);

        if (Vector3.Distance(ZombieAgent.transform.position, TargetPositionAtZombieHeight) < ZombieAgent.stoppingDistance + AttackDistanceThreshold)
        {
            ZombieAnimator.SetTrigger("Attack");
        }   

    }
    public void Colpito()
    {
        GameManager.Singleton.PlayerLifePoints -= 10;
        Debug.Log("Player Life Points: " + GameManager.Singleton.PlayerLifePoints);
        
    }

}
