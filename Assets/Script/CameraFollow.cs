using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 5f;
    public Vector3 offset;

    void LateUpdate()
    {
        if (target == null) return;

        float lookAhead = Input.GetAxis("Horizontal") * 2f;

        Vector3 desiredPosition = target.position + offset + new Vector3(lookAhead, 0, 0);

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );
    }
}