```C#

using UnityEngine;
using System;

public class PositionDetector : MonoBehaviour
{
    public static PositionDetector Instance { get; private set; }

    [Header("Tracked Objects")]
    public GameObject head;
    public GameObject handR;
    public GameObject handL;

    [Header("Tracked Position")]
    [SerializeField] private Vector3 headPosition;
    [SerializeField] private Vector3 handRPosition;
    [SerializeField] private Vector3 handLPosition;

    [Header("Detection Settings")]
    [SerializeField] private float positionThreshold = 0.01f;

    private Vector3 lastHeadPosition;
    private Vector3 lastHandRPosition;
    private Vector3 lastHandLPosition;

    public event Action OnPositionChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        UpdateTrackedPositions();
        SaveCurrentPositions();
    }

    private void Update()
    {
        UpdateTrackedPositions();

        if (HasPositionChanged())
        {
            SaveCurrentPositions();
            OnPositionChanged?.Invoke();
        }
    }

    private void UpdateTrackedPositions()
    {
        if (head != null)
            headPosition = head.transform.position;

        if (handR != null)
            handRPosition = handR.transform.position;

        if (handL != null)
            handLPosition = handL.transform.position;
    }

    private bool HasPositionChanged()
    {
        return
            (head != null && Vector3.Distance(headPosition, lastHeadPosition) > positionThreshold) ||
            (handR != null && Vector3.Distance(handRPosition, lastHandRPosition) > positionThreshold) ||
            (handL != null && Vector3.Distance(handLPosition, lastHandLPosition) > positionThreshold);
    }

    private void SaveCurrentPositions()
    {
        if (head != null)
            lastHeadPosition = headPosition;

        if (handR != null)
            lastHandRPosition = handRPosition;

        if (handL != null)
            lastHandLPosition = handLPosition;
    }

    public Vector3 GetHeadData()
    {
        return headPosition;
    }

    public Vector3 GetHandLData()
    {
        return handLPosition;
    }

    public Vector3 GetHandRData()
    {
        return handRPosition;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}

```
