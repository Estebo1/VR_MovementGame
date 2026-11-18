using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class GestureCollider : MonoBehaviour
{
    private Gesture ownerGesture;
    public EHandType _handType = EHandType.Right;

    public bool isActive;
    public bool hasTriggered;

    private string handTag = "HandLeft";

    private BoxCollider boxCollider;
    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        handTag = (_handType == EHandType.Left) ? "HandLeft" : "HandRight";
        //this.gameObject.SetActive(false);
        DeactivateNode();
    }
    public void Initialize(Gesture owner, EHandType handType)
    {
        ownerGesture = owner;
        _handType = handType;

        handTag = (_handType == EHandType.Left)? "HandLeft": "HandRight";
    }
    public void ActivateNode()
    {
        isActive = true;
        gameObject.SetActive(true);
        boxCollider.enabled = true;
    }
    public void DeactivateNode()
    {
        isActive = false;
        if (boxCollider != null)
            boxCollider.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!isActive) return;

        if (other.CompareTag(handTag))
        {
            if (ownerGesture == null)
            {
                Debug.LogError(
                    $"[{name}] ownerGesture es NULL. No se inicializó correctamente.",
                    this
                );
                return;
            }

            hasTriggered = true;
            DeactivateNode();

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
    private void OnDrawGizmos()
    {
        if (isActive)
        {
            if (boxCollider == null)
                boxCollider = GetComponent<BoxCollider>();

            Gizmos.color = Color.green;

            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(boxCollider.center, boxCollider.size);
        }
    }
}