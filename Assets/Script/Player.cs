using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5.0f;
    public float turnSpeed = 120.0f; // degrees per second

    Vector3 startPos;
    Quaternion startRotation;

    private void Start()
    {
        startPos = transform.position;
        startRotation = transform.rotation;
    }

    void Update()
    {
        var turn = Input.GetAxisRaw("Horizontal");
        var forward = Input.GetAxisRaw("Vertical");

        transform.Rotate(0, turn * turnSpeed * Time.deltaTime, 0);
        transform.position += transform.forward * forward * speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Respawn")
        {
            print("Game over");
            transform.position = startPos;
            transform.rotation = startRotation;
        }
    }
}