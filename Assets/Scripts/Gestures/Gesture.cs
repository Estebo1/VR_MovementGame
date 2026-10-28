using UnityEngine;
using UnityEngine.Events;

public abstract class Gesture : MonoBehaviour
{
    [SerializeField] protected GestureCollider []rightColliders;
    [SerializeField] protected GestureCollider []leftColliders;

    [SerializeField] protected float distanceToBodyZ;
    [SerializeField] protected float distanceFromHeadY;

    protected int rightIndex = 0;
    protected int leftIndex = 0;

    protected bool isBeingCasted;

    UnityEvent Cast;
    private void Start()
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
    private void NextCollider(GestureCollider[] colliders, ref int currentIndex, GestureCollider completedNode)
    {
        if (colliders == null || currentIndex >= colliders.Length) return;

        if (colliders[currentIndex] == completedNode)
        {
            currentIndex++;

            if (currentIndex < colliders.Length)
            {
                colliders[currentIndex].isActive = true;
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
        if (!isBeingCasted) return;

        isBeingCasted = false;
        rightIndex = 0;
        leftIndex = 0;
        foreach(GestureCollider col in rightColliders)
        {
            if (col != null) col.ResetNode();
        }
        foreach(GestureCollider col in leftColliders)
        {
            if (col != null) col.ResetNode();
        }
    }
    private void ResetSequence(GestureCollider[] colliders, ref int index)
    {
        index = 0;
        if (colliders == null) return;

        foreach (var col in colliders)
        {
            if (col != null) col.ResetNode();
        }

        if (colliders.Length > 0 && colliders[0] != null)
        {
            colliders[0].isActive = true;
        }
    }

    protected abstract void OnGestureSuccess();
}
