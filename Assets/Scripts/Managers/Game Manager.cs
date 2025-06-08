using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool vrEnabled;

    [SerializeField]
    GameObject vrController;
    
    [SerializeField]
    GameObject defaultController;
    
    [SerializeField]
    GameObject spawnPoint;

    public void Initialize()
    {
        
    }
    
    
    void Start()
    {
        vrEnabled = false;
    }

    // Update is called once per frame
    void Update()
    {   
        vrController.SetActive(vrEnabled);
        defaultController.SetActive(!vrEnabled);
    }
    
    
    public Vector3 GetSpawnPoint() => spawnPoint.transform.position;
    
}
