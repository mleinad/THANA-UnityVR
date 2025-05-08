using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPosition : MonoBehaviour
{
    [SerializeField]
    private Transform hand;

    [SerializeField]
    private Animator animator;

    [Header("Hand Position Range")]
    
    [SerializeField]
    private float maxDistance = 1.5f; // Y position for fully standing

    [SerializeField]
    private string blendParameter = "Blend"; // Animator parameter name


    private Vector3 sittingPosition;
    
    void Start()
    {
        sittingPosition = hand.position;
    }
    void Update()
    {
        
        Vector3 offset = hand.position - sittingPosition;
        float distance = offset.magnitude;
        
        if(distance ==0) return;
        
        float verticalFactor = Vector3.Dot(offset.normalized, Vector3.up);
        

        float blendValue = Mathf.Clamp01((distance/maxDistance)*verticalFactor);

        animator.SetFloat(blendParameter, blendValue);
    }
}
