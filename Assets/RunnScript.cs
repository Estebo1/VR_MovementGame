using Unity.VisualScripting;
using UnityEngine;

public class RunScript : MonoBehaviour
{
    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;

    [SerializeField] private float speed = 5f;
    [SerializeField] private float maxSpeed = 10f;

    [SerializeField] private float acceleration = 2f;
    [SerializeField] private float deceleration = 2f;


    private Vector3 rightPrevPos;
    private Vector3 leftPrevPos;
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
