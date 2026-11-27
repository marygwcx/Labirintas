using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    public float speed = 5f;
    public float turnSpeed = 120f;

    private Rigidbody rb;

    private Vector3 startPos;
    private Quaternion startRotation;

    private float turn;
    private float forward;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        startPos = transform.position;
        startRotation = transform.rotation;

        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    void Update()
    {
        turn = Input.GetAxisRaw("Horizontal");
        forward = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        // Rotate the player
        Quaternion rotation = rb.rotation * Quaternion.Euler(
            0, turn * turnSpeed * Time.fixedDeltaTime, 0
        );

        rb.MoveRotation(rotation);

        // Move the player
        Vector3 movement = rotation * Vector3.forward
            * forward * speed;

        // Preserve gravity
        movement.y = rb.linearVelocity.y;

        rb.linearVelocity = movement;

        // Prevent unwanted spinning
        rb.angularVelocity = Vector3.zero;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Respawn"))
        {
            print("Game over");

            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            rb.position = startPos;
            rb.rotation = startRotation;
        }
    }
}