using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;

    public float trackSpeed = 10;
    [Range(0.1f, 0.9f)]
    public float smoothness = 0.125f;

    private Vector3 offset;

    void Start()
    {
        offset = Quaternion.Inverse(target.rotation) * (transform.position - target.position);
    }

    void LateUpdate()
    {
        var desiredPosition = target.position + target.rotation * offset;

        var targetPosition = Vector3.MoveTowards(transform.position, desiredPosition, trackSpeed * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothness);

        transform.LookAt(target);
    }
}