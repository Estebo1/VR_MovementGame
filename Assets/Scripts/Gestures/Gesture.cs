using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms;

public abstract class Gesture : MonoBehaviour
{
    [SerializeField] protected string gestureName;
    [SerializeField] protected GestureCollider []rightColliders;
    [SerializeField] protected GestureCollider []leftColliders;

    [SerializeField] protected float distanceToBodyZ;
    [SerializeField] protected float distanceFromHeadY;
    [Header("Distance Percentages")]
    [Range(0f, 1f)]
    [SerializeField] protected float porcentHandDistanceRight;
    [Range(0f, 1f)]
    [SerializeField] protected float porcentHandDistanceLeft;

    protected int rightIndex = 0;
    protected int leftIndex = 0;

    protected bool isBeingCasted;
    protected bool hasCasted;

    public UnityEvent Cast;

    public float LeftHandDistancePercentage => porcentHandDistanceLeft;
    public float RightHandDistancePercentage => porcentHandDistanceRight;
    protected void Start()
    {
        InitArray(rightColliders, GestureCollider.EHandType.Right);
        InitArray(leftColliders, GestureCollider.EHandType.Left);
        CalculateColliderDistance();
    }
    protected abstract void CalculateColliderDistance();
    private void InitArray(GestureCollider[] colliders, GestureCollider.EHandType handType)
    {
        if (colliders == null) return;

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i] != null)
            {
                colliders[i].Initialize(this, handType);
            }
        }
    }
    public void StartCasting()
    {
        if (isBeingCasted) return;

        isBeingCasted = true;

        ResetSequence(rightColliders, ref rightIndex);
        ResetSequence(leftColliders, ref leftIndex);
    }

    public void OnNodeCompleted(GestureCollider completedNode, GestureCollider.EHandType hand)
    {
        if (!isBeingCasted) return;

        if (hand == GestureCollider.EHandType.Right)
        {
            NextCollider(rightColliders, ref rightIndex, completedNode);
        }
        else
        {
            NextCollider(leftColliders, ref leftIndex, completedNode);
        }

        CheckGestureCompletion();
    }
    private void NextCollider(GestureCollider[] colliders,ref int currentIndex,GestureCollider completedNode)
    {
        if (colliders == null || currentIndex >= colliders.Length)
            return;

        if (colliders[currentIndex] == completedNode)
        {
            completedNode.DeactivateNode();

            currentIndex++;

            if (currentIndex < colliders.Length &&
                colliders[currentIndex] != null)
            {
                colliders[currentIndex].ActivateNode();
            }
        }
        else
        {
            StopCasting();
        }
    }
    private void CheckGestureCompletion()
    {
        bool rightDone = (rightColliders == null || rightColliders.Length == 0 || rightIndex >= rightColliders.Length);
        bool leftDone = (leftColliders == null || leftColliders.Length == 0 || leftIndex >= leftColliders.Length);

        if (rightDone && leftDone)
        {
            isBeingCasted = false;
            OnGestureSuccess();
            Cast?.Invoke();
        }
    }
    public void StopCasting()
    {
        isBeingCasted = false;

        rightIndex = 0;
        leftIndex = 0;

        if (rightColliders != null)
        {
            foreach (GestureCollider col in rightColliders)
            {
                if (col != null)
                    col.DeactivateNode();
            }
        }

        if (leftColliders != null)
        {
            foreach (GestureCollider col in leftColliders)
            {
                if (col != null)
                    col.DeactivateNode();
            }
        }
    }
    private void ResetSequence(GestureCollider[] colliders, ref int index)
    {
        index = 0;

        if (colliders == null || colliders.Length == 0)
            return;

        foreach (var col in colliders)
        {
            if (col != null)
                col.DeactivateNode();
        }

        if (colliders[0] != null)
            colliders[0].ActivateNode();
    }

    protected virtual void OnGestureSuccess() {
        Debug.Log($"Gesture {gestureName} completed successfully!");
    }
}
