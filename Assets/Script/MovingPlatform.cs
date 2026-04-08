using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float moveDistance = 2f;
    public float speed = 2f;

    private Vector3 startPos;
    private bool movingUp = true;

    void Start()
    {
        startPos = transform.position;

        speed += Random.Range(-1f, 1f);
        moveDistance += Random.Range(-1f, 1f);
    }

    void Update()
    {
        float movement = speed * Time.deltaTime;

        if (movingUp)
        {
            transform.position += Vector3.up * movement;

            if (transform.position.y >= startPos.y + moveDistance)
                movingUp = false;
        }
        else
        {
            transform.position -= Vector3.up * movement;

            if (transform.position.y <= startPos.y - moveDistance)
                movingUp = true;
        }
    }
}