using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Serialization;

public class NpcReaction : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)]
    private float suspiciousThreshold;

    [SerializeField]
    private List<GameObject> npcs;

    public Transform player;
    public Transform targetObject;
    public MultiAimConstraint headAimConstraint;
    public MultiAimConstraint neckAimConstraint;

    [SerializeField]
    private float weightLerpSpeed = 2f;

    private Coroutine weightCoroutine;

    private void OnEnable()
    {
        MemoryManager.Instance.EmotionalStateChanged += OnEmotionsChanged;
    }

    private void OnDisable()
    {
        MemoryManager.Instance.EmotionalStateChanged -= OnEmotionsChanged;
    }

    private void OnEmotionsChanged(EmotionalValue obj)
    {
        if (obj.suspicion >= suspiciousThreshold)
        {
            if (player != null && targetObject != null)
            {
                targetObject.position = player.position;
            }

            if (weightCoroutine != null) StopCoroutine(weightCoroutine);
            weightCoroutine = StartCoroutine(SmoothlyChangeWeights(1f)); // Increase weight
        }
        else
        {
            if (weightCoroutine != null) StopCoroutine(weightCoroutine);
            weightCoroutine = StartCoroutine(SmoothlyChangeWeights(0f)); // Return to normal
        }
    }

    private IEnumerator SmoothlyChangeWeights(float targetWeight)
    {
        while (!Mathf.Approximately(headAimConstraint.weight, targetWeight) ||
               !Mathf.Approximately(neckAimConstraint.weight, targetWeight))
        {
            headAimConstraint.weight = Mathf.MoveTowards(headAimConstraint.weight, targetWeight, Time.deltaTime * weightLerpSpeed);
            neckAimConstraint.weight = Mathf.MoveTowards(neckAimConstraint.weight, targetWeight * 0.75f, Time.deltaTime * weightLerpSpeed);
            yield return null;
        }
    }
}
