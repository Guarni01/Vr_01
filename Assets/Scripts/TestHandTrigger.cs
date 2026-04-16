using System.Runtime.CompilerServices;
using UnityEngine;

public class TestHandTrigger : MonoBehaviour
{
    public Color SelectedColor;
    public Color UnselectedColor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<Renderer>().material.color = SelectedColor;
    }

    private void OnTriggerExit(Collider other)
    {
        other.gameObject.GetComponent<Renderer>().material.color = UnselectedColor;
    }
}
