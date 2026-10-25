using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class FTMController : MonoBehaviour
{
    [SerializeField] float thrust = 40f;
    [SerializeField] float maxSpeed = 35f;
    [SerializeField] float rollSpeed = 200f;
    [SerializeField] float lookSensitivity = 0.08f;
    [SerializeField] float maxPitch = 80f;

    InputAction moveAction;
    InputAction lookAction;
    InputAction attackAction;

    Rigidbody rb;
    float pitch;
    float yaw;
    float roll;

    [HideInInspector] public float recoilPitch;

    void Start()
    {
        InputActionMap player = InputSystem.actions.FindActionMap("Player");
        moveAction = player.FindAction("Move");
        lookAction = player.FindAction("Look");
        attackAction = player.FindAction("Attack");
        moveAction.Enable();
        lookAction.Enable();
        attackAction.Enable();

        rb = GetComponent<Rigidbody>();

        Vector3 euler = transform.eulerAngles;
        pitch = euler.x;
        if (pitch > 180f)
            pitch -= 360f;
        yaw = euler.y;
        roll = euler.z;
    }

    void Update()
    {
        Vector2 look = lookAction.ReadValue<Vector2>();
        yaw += look.x * lookSensitivity;
        pitch -= look.y * lookSensitivity;
        pitch = Mathf.Clamp(pitch, -maxPitch, maxPitch);
    }

    void FixedUpdate()
    {
        Vector2 move = moveAction.ReadValue<Vector2>();

        // A rollt nach links, D nach rechts.
        roll -= move.x * rollSpeed * Time.fixedDeltaTime;
        rb.MoveRotation(Quaternion.Euler(pitch + recoilPitch, yaw, roll));

        // W Schub nach vorne, S nach hinten.
        rb.AddForce(transform.forward * move.y * thrust, ForceMode.Acceleration);

        if (rb.linearVelocity.magnitude > maxSpeed)
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
    }
}
