using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;

public class GestureManager : MonoBehaviour
{
    [SerializeField] private Gesture[] gestures;
    [SerializeField] private float cooldownGestures;

    private bool isCasting;
    public bool IsCasting => isCasting;
    void Start()
    {
        
    }

    void Update()
    {
        if (IsRTPressed() && IsLTPressed())
        {
            if (isCasting) return;
            foreach (Gesture gesture in gestures)
            {
                gesture.StartCasting();
            }
            isCasting = true;
        }
        else
        {
            if (!isCasting) return;

            foreach (Gesture gesture in gestures)
            {
                gesture.StopCasting();
            }
            isCasting = false;
        }
    }
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
}
