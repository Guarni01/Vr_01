using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public static GameManager Singleton;
    public int PlayerLifePoints= 100;
    public List <GameObject> OggettiSulTavolo = new List<GameObject>();
    public List <GameObject> AllTables = new List<GameObject>();

    

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
    public void SearchForTables()
    {
        GameObject currentTable = GameObject.Find("TABLE");
        NavMeshObstacle myObstacle = currentTable.AddComponent<NavMeshObstacle>();
        myObstacle.carving = true;
    }
}
