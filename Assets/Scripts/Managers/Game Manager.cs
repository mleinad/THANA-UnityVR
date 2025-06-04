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
}
