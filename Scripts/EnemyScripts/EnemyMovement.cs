using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    [SerializeField] float obstructionDetectionOffset = 0.1f;
    [SerializeField] Rigidbody2D rb;
    int movementDirection = 1;

    private void OnEnable()
    {
        EventManager.OnSpawnObject += HandleOnSpawnObject;
    }

    private void OnDisable()
    {
        EventManager.OnSpawnObject -= HandleOnSpawnObject;
    }

    private void FixedUpdate()
    {
        if (Mathf.Abs(rb.velocity.x) != speed)
        {
            Vector2 velocity = movementDirection * Vector2.right * speed;
            SetHorizontalVelocity(velocity);
        }

        if (CheckForObstruction())
        {
            SetMovementDirection(-movementDirection);
        }
        
    }

    bool CheckForObstruction()
    {
        LayerMask wallMask = LayerMask.GetMask("Wall");
        RaycastHit2D obstructionHit = Physics2D.Raycast(transform.position, Vector2.right * movementDirection, obstructionDetectionOffset, wallMask);

        if (obstructionHit)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void SetHorizontalVelocity(Vector2 velocity)
    {
        rb.velocity = new Vector2(velocity.x, rb.velocity.y);
    }

    public void SetMovementDirection(int direction)
    {
        movementDirection = (int)Mathf.Clamp(direction, -1, 1);
    }

    void HandleOnSpawnObject(Transform _object, Vector2 direction)
    {
        if (_object == transform)
        {
            SetMovementDirection((int)direction.x);
        }
    }
}
