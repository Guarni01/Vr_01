using UnityEngine;
using UnityEngine.AI;

public class TestUnityNav : MonoBehaviour
{ 
    public Transform targetObject; 
    public NavMeshAgent zombieAgent;
    public Animator zombieAnimator;
    public float movementThreshold = 0.1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        zombieAgent.SetDestination(targetObject.position);
        if(zombieAgent.velocity.magnitude > movementThreshold)
        {
            zombieAnimator.SetBool("Walk", true);
        }
        else
        {
            zombieAnimator.SetBool("Walk", false);
        } 
    }
}
