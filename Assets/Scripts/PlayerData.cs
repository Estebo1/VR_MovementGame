using UnityEngine;
using UnityEngine.Android;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance { get; private set; }

    [SerializeField] private Transform headXR;
    [SerializeField] private Transform leftHandXR;
    [SerializeField] private Transform rightHandXR;

    private Transform headPos;
    private Transform rightHandPos;
    private Transform rightHandPosDefault;
    private Transform leftHandPos;
    private Transform leftHandPosDefault;

    private float distanceFromHeadY;
    private float distanceFromHeadZ;
    private float distanceFromHeadToHands;
    private float distanceFromHeadYDefault = 10.0f;

    private float maxDistanceFromRightHandToHead;   
    private float maxDistanceFromLeftHandToHead;   
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        rightHandPos = rightHandXR;
        leftHandPos = leftHandXR;
        headPos = headXR;
        if (headXR)
        {
            rightHandPosDefault.position = new Vector3(Instance.headXR.position.x, Instance.headXR.position.y - Instance.distanceFromHeadYDefault, Instance.headXR.position.z);
        }
    }
    public static void SaveHandsPosition()
    {
        if (Instance.headXR == null) return;

        Instance.headPos = Instance.headXR;
        Instance.rightHandPos = Instance.rightHandXR;
        Instance.leftHandPos = Instance.leftHandXR;

        if(Instance.rightHandXR.position.y > Instance.headXR.position.y)
        {
            Instance.rightHandPos = Instance.rightHandPosDefault;
        }
        if (Instance.leftHandXR.position.y > Instance.headXR.position.y)
        {
            Instance.leftHandPos = Instance.leftHandPosDefault;
        }
    }
    public static void SetUpGesture(Transform leftTransform, Transform rightTransform,Gesture gesture)
    {
        float leftMaxValueY = Instance.headPos.position.y - Instance.maxDistanceFromLeftHandToHead;
        float rightMaxValueY = Instance.headPos.position.y - Instance.maxDistanceFromRightHandToHead;
        
        float leftValueY = leftMaxValueY * gesture.LeftHandDistancePercentage;
        float rightValueY = rightMaxValueY * gesture.RightHandDistancePercentage;

        leftTransform.position = new Vector3(leftTransform.position.x, leftValueY, leftTransform.position.z);
        rightTransform.position = new Vector3(rightTransform.position.x, rightValueY, rightTransform.position.z);
    }
    public static void SetHeadPos(Transform head)
    {
        Instance.headPos = head;
    }
    public static void SetRightHandPos(Transform rightHand)
    {
        Instance.rightHandPos = rightHand;
    }
    public static void SetLeftHandPos(Transform leftHand)
    {
        Instance.leftHandPos = leftHand;
    }

}
