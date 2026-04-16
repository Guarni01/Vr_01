using UnityEngine;

public class TestGrab : MonoBehaviour
{
    Renderer myRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        OVRInput.Update();


        if(OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {

            myRenderer.material.color = Random.ColorHSV();

        }
    }
}
