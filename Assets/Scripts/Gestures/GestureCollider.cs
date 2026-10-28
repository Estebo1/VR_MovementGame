using UnityEngine;

public class GestureCollider : MonoBehaviour
{
    private Gesture ownerGesture;
    public EHandType _handType = EHandType.Right;

    public bool isActive;
    public bool hasTriggered;

    private string handTag = "";

    public void Initialize(Gesture owner, EHandType handType)
    {
        ownerGesture = owner;
        _handType = handType;
        handTag = (_handType == EHandType.Left) ? "HandLeft" : "HandRight";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActive) return;

        if (other.CompareTag(handTag))
        {
            hasTriggered = true;
            isActive = false; 

            ownerGesture.OnNodeCompleted(this, _handType);
        }
    }

    public void ResetNode()
    {
        hasTriggered = false;
        isActive = false;
    }

    public enum EHandType
    {
        Left,
        Right
    }
}