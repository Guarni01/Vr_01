using UnityEngine;
using UnityEngine.InputSystem;

public class TestGrab : MonoBehaviour
{
    public InputAction GrabAction;
    Renderer myRenderer;
    bool doOnce;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GrabAction.Enable();
        myRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GrabAction.IsPressed())
        {
            if(!doOnce)
            {
                
                 myRenderer.material.color = Random.ColorHSV();
                
                doOnce = true;
            }
        }
        else
        {
            if(doOnce)
            {


                doOnce = false;
            }
        }       
    }
}
