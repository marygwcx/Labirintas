using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;

    public float distance = 7f;
    public float height = 4f;
    public float smoothTime = 0.2f;

    public float cameraRadius = 0.3f;
    public float collisionOffset = 0.2f;
    public LayerMask collisionLayers;

    private float currentDistance;
    private float velocity;

    void Start()
    {
        currentDistance = Mathf.Sqrt(
            distance * distance + height * height
        );
    }

    void LateUpdate()
    {
        if (target == null)
            return;

        Quaternion rotation = Quaternion.Euler(
            0, target.eulerAngles.y, 0
        );

        Vector3 origin = target.position + Vector3.up;

        Vector3 desiredPosition = target.position +
            rotation * new Vector3(0, height, -distance);

        Vector3 direction = desiredPosition - origin;

        float desiredDistance = direction.magnitude;

        direction.Normalize();

        // Check for objects between player and camera
        if (Physics.SphereCast(
            origin,
            cameraRadius,
            direction,
            out RaycastHit hit,
            desiredDistance,
            collisionLayers,
            QueryTriggerInteraction.Ignore))
        {
            desiredDistance = Mathf.Max(
                0, hit.distance - collisionOffset
            );
        }

        // Move closer immediately when hitting an object
        if (desiredDistance < currentDistance)
        {
            currentDistance = desiredDistance;
            velocity = 0;
        }
        else
        {
            // Smoothly return to normal distance
            currentDistance = Mathf.SmoothDamp(
                currentDistance,
                desiredDistance,
                ref velocity,
                smoothTime
            );
        }

        transform.position =
            origin + direction * currentDistance;

        transform.LookAt(origin);
    }
}