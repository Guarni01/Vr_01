using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Singleton;
    public List <GameObject> OggettiSulTavolo = new List<GameObject>();

    

    void OnEnable()
    {
        if (Singleton != null && Singleton != this)
        {
            Debug.LogError("There is more than one GameManager in the scene. Destroying the new one.");
            Destroy(this);
        }
        else
        {
            Singleton = this;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
