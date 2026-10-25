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

    [SerializeField] private Vector3 moveDirection;
    [SerializeField] private float acceleration = 2f;
    [SerializeField] private float deceleration = 4f;
    [SerializeField] private float walkingSpeed = 2f;
    [SerializeField] private float runningSpeed = 5f;
    [SerializeField] private float sprintSpeed = 7f;
    [SerializeField] private float walkingDistance = 0.07f;
    [SerializeField] private float runningDistance = 0.1f;
    [SerializeField] private float sprintDistance = 0.12f;

    private Vector3 rightCurrentPos;
    private Vector3 leftCurrentPos;

    private Vector3 rightPrevPos;
    private Vector3 leftPrevPos;
    private void Start()
    {
        mainCamera = Camera.main;

        rightPrevPos = rightHand.transform.position;
        leftPrevPos = leftHand.transform.position;

        moveDirection = Vector3.ProjectOnPlane(
            mainCamera.transform.forward,
            Vector3.up
        ).normalized;
    }

    private void Update()
    {

        rightCurrentPos = rightHand.transform.position ;
        leftCurrentPos = leftHand.transform.position;

        bool isMoving = CalculateMoveHands();

        if (isMoving)
        {
            Vector3 cameraForward = Vector3.ProjectOnPlane(
                mainCamera.transform.forward,
                Vector3.up
            );

            if (cameraForward.sqrMagnitude > 0.001f)
            {
                moveDirection = cameraForward.normalized;
            }

            speed = Mathf.MoveTowards(
                speed,
                maxSpeed,
                acceleration * Time.deltaTime
            );
        }
        else
        {
            speed = Mathf.MoveTowards(
                speed,
                0f,
                deceleration * Time.deltaTime
            );
        }

        body.Move(moveDirection * speed * Time.deltaTime);

        rightPrevPos = rightCurrentPos;
        leftPrevPos = leftCurrentPos;
    }

    private bool CalculateMoveHands()
    {
        float rightDist = Vector3.Distance(
            rightCurrentPos, rightPrevPos
        );

        float leftDist = Vector3.Distance(
            leftCurrentPos, leftPrevPos
        );

        bool rightMoving = rightDist > walkingDistance;
        bool leftMoving = leftDist > walkingDistance;

        Debug.Log(
            $"Right: {rightDist:F4} ({rightMoving}) | " +
            $"Left: {leftDist:F4} ({leftMoving}) | " +
            $"Speed: {speed:F2}"
        );
        if (rightDist > sprintDistance || leftDist > sprintDistance)
        {
            acceleration = sprintSpeed;
        }
        else if (rightDist > runningDistance || leftDist > runningDistance)
        {
            acceleration = runningSpeed;
        }
        else
        {
            acceleration = walkingSpeed;
        }
        return rightMoving || leftMoving;
    }
}