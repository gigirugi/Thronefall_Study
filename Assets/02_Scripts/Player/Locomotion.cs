using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Locomotion : MonoBehaviour
{
    [Header("Input Action References")]
    [SerializeField] InputActionReference moveRef;
    [SerializeField] InputActionReference runRef;

    NavMeshAgent agent;

    [Header("Movement Settings")]
    [SerializeField] float walkSpeed = 3.5f;
    [SerializeField] float runSpeed = 7f;

    [SerializeField] float rotationSpeed = 720f;
    Vector3 lookDirection;
    void Awake()
    {
        if (!agent)
            TryGetComponent(out agent);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (moveRef.action.IsPressed())
        {
            Vector2 input = moveRef.action.ReadValue<Vector2>();
            Move(input);
            Rotate(input);
        }
    }

    private void Rotate(Vector2 input)
    {
        lookDirection = new(input.x, 0, input.y);
        Quaternion toRotation =Quaternion.LookRotation(lookDirection, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation,toRotation,
        rotationSpeed * Time.deltaTime);
    }

    private void Move(Vector2 input)
    {
        bool isRunning = runRef.action.IsPressed();
        float speed = isRunning ? runSpeed : walkSpeed;
        Vector3 direction = new(input.x, 0, input.y);
        agent.Move(speed * Time.deltaTime * direction);
    }
}
