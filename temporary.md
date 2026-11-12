```C#
using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Collections.Generic;

public class PositionDetector : MonoBehaviour
{
    public GameObject head;
    public GameObject handR;
    public GameObject handL;
    private float logTimer = 0f;
    private float logInterval = 1f;
    public static PositionDetector Instance { get; private set; }


    [Header("Tracked Position")]
    [SerializeField] private Vector3 headPosition;
    [SerializeField] private Vector3 handRPosition;
    [SerializeField] private Vector3 handLPosition;

    [SerializeField] private float positionThreshold = 0.01f;

    public event Action OnPositionChanged;

    private void Start()
    {
        SaveCurrentPosition();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        if (head != null)
        {
            headPosition = head.transform.position;
        }
        if (handR != null)
        {
            handRPosition = handR.transform.position;
        }
        if (handL != null)
        {
            handLPosition = handL.transform.position;
        }
        
    }

    public UnityEngine.Vector3 GetHeadData()
    {
        return headPosition;
    }

    public UnityEngine.Vector3 GetHandLData()
    {
        return handLPosition;
    }

    public UnityEngine.Vector3 GetHandRData()
    {
        return handRPosition;
    }

    private bool HasPositionChanged()
    {
        return Vector3.Distance(headPosition, lastHeadPosition) > positionThreshold || Vector3.Distance(handLPosition, lastHandLPosition) > positionThreshold || Vector3.Distance(handRPosition, lastHandRPosition) > positionThreshold
    }

    private void SaveCurrentPositions()
    {
        lastHeadPosition = headPosition;
        lastHandLPosition = handLPosition;
        lastHandRPosition = handRPosition;
    }
}

```
