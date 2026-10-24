using Unity.VisualScripting;
using UnityEngine;

public class RunScript : MonoBehaviour
{
    [SerializeField] private CharacterController body;
    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;
    private Camera mainCamera;
    [SerializeField] private float speed = 0f;
    [SerializeField] private float maxSpeed = 10f;

    [SerializeField] private float acceleration = 2f;
    [SerializeField] private float deceleration = 4f;
    [SerializeField] private float handsDistanceThreshold = 0.1f;

    private Vector3 currentVelocity;
    private Vector3 rightCurrentPos;
    private Vector3 leftCurrentPos;

    private Vector3 rightPrevPos;
    private Vector3 leftPrevPos;
    private void Start()
    {
        speed = 0f;
        rightCurrentPos = rightHand.transform.position;
        leftCurrentPos = leftHand.transform.position;
        leftPrevPos = leftCurrentPos;
        rightPrevPos = rightCurrentPos;
        mainCamera = Camera.main;
    }
    void Update()
    {
        rightCurrentPos = rightHand.transform.position;
        leftCurrentPos = leftHand.transform.position;

        CalculateMoveHands();
        Vector3 forward = Vector3.ProjectOnPlane(
    mainCamera.transform.forward,
    Vector3.up
);
        forward = forward.normalized;

        speed = Mathf.Clamp(speed, 0f, maxSpeed);
        body.Move(speed * forward * Time.deltaTime);

        rightPrevPos = rightCurrentPos;
        leftPrevPos = leftCurrentPos;
    }
    private void CalculateMoveHands()
    {
        float rightDist = Vector3.Distance(rightCurrentPos, rightPrevPos);
        float leftDist = Vector3.Distance(leftCurrentPos, leftPrevPos);
        if ((rightDist > handsDistanceThreshold) || (leftDist > handsDistanceThreshold))
        {
            speed += acceleration * Time.deltaTime;
        }
        else
        {
            speed -= deceleration * Time.deltaTime;
        }
    }
}
