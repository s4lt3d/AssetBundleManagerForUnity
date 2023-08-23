using UnityEngine;

/// <summary>
///     Bounces a sprite like an old fashioned dvd screen saver.
/// </summary>
public class DVDBounce : MonoBehaviour
{
    public float speed = 1f;
    private float objectHeight;
    private float objectWidth;
    private Vector2 screenBounds;
    private Vector2 velocity = new(1f, 1f);

    private void Start()
    {
        screenBounds.x = Camera.main.aspect * Camera.main.orthographicSize;
        screenBounds.y = Camera.main.orthographicSize;

        objectWidth = transform.GetComponent<SpriteRenderer>().bounds.extents.x;
        objectHeight = transform.GetComponent<SpriteRenderer>().bounds.extents.y;
    }

    private void Update()
    {
        transform.position += (Vector3)velocity * speed * Time.deltaTime;

        if (transform.position.x > screenBounds.x - objectWidth || transform.position.x < -screenBounds.x + objectWidth)
        {
            velocity.x = -velocity.x;
        }
        if (transform.position.y > screenBounds.y - objectHeight || transform.position.y < -screenBounds.y + objectHeight)
        {
            velocity.y = -velocity.y;
        }
    }
}