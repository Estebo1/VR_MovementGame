using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;


public class GestureManager : MonoBehaviour
{
    [SerializeField] private Gesture startingGesture;
    [SerializeField] private Gesture[] gestures;
    [SerializeField] private float cooldownGestures;

    private enum EGestureState
    {
        Idle,
        StartingGesture,
        WaitingForStartingGesture,
        Casting,
        Cooldown,
    }
    private EGestureState currentState = EGestureState.Idle;

    private bool isCasting;
    private bool hasCastedStartingGesture = false;
    private bool hasCastedGesture = false;
    public bool IsCasting => isCasting;
    void Start()
    {
        startingGesture.Cast.AddListener(IsStartingGestureCasted);

        foreach(Gesture gesture in gestures)
        {
            gesture.Cast.AddListener(IsGestureCasted);
        }
    }

    void Update()
    {
        switch (currentState)
        {
            case EGestureState.Idle:
                IdleState();
                break;
            case EGestureState.StartingGesture:
                StartingGesture();
                break;
            case EGestureState.WaitingForStartingGesture:
                WaitForStartingGestureToComplete();
                break;
            case EGestureState.Casting:
                Casting();
                break;
            case EGestureState.Cooldown:
                Cooldown();
                break;
        }
    }

    #region States
    void IdleState()
    {
        if (IsRTPressed() && IsLTPressed())
        {
            currentState = EGestureState.StartingGesture;
            Debug.LogWarning("Pressed Start");
        }
        else
        {
            if (!isCasting) return;

            startingGesture.StopCasting();
            foreach (Gesture gesture in gestures)
            {
                gesture.StopCasting();
            }
            hasCastedGesture = false;
            hasCastedStartingGesture = false;
            isCasting = false;
        }
    }
    void StartingGesture()
    {
        startingGesture.StartCasting();
        currentState = EGestureState.WaitingForStartingGesture;
    }
    void WaitForStartingGestureToComplete()
    {
        if (hasCastedStartingGesture)
        {
            currentState = EGestureState.Casting;
        }
        else if (!IsRTPressed() || !IsLTPressed())
        {
            startingGesture.StopCasting();
            currentState = EGestureState.Idle;
        }
    }
    void Casting()
    {
        if (hasCastedGesture)
        {
            foreach (Gesture gesture in gestures)
            {
                gesture.StopCasting();
            }
            currentState = EGestureState.Cooldown;
            return;
        }
        if (!IsRTPressed() || !IsLTPressed())
        {
            startingGesture.StopCasting();
            currentState = EGestureState.Idle;
        }

        if (isCasting) return;

        foreach (Gesture gesture in gestures)
        {
            gesture.StartCasting();
        }
        isCasting = true;
    }

    float cooldownTimer = 0f;
    public void Cooldown()
    {
        cooldownTimer += Time.deltaTime;
        if (cooldownTimer >= cooldownGestures)
        {
            cooldownTimer = 0f;
            startingGesture.StopCasting();
            hasCastedStartingGesture = false;
            hasCastedGesture = false;
            isCasting = false;
            currentState = EGestureState.Idle;
        }
    }

    #endregion
    #region Input
    public bool IsRTPressed()
    {
        InputDevice rightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        if (rightHand.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            return triggerValue > 0.5f;
        }
        return false;
    }
    public bool IsLTPressed()
    {
        InputDevice leftHand = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        if (leftHand.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            return triggerValue > 0.5f;
        }
        return false;
    }
    #endregion
    public void IsStartingGestureCasted()
    {
        hasCastedStartingGesture = true;
        Debug.LogWarning("StartCasting");
    }
    public void IsGestureCasted()
    {
        hasCastedGesture = true;
    }
}
 