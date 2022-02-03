using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrow : MonoBehaviour
{
    [SerializeField] float throwForceMultiplier = 1f;
    [SerializeField] float maxThrowForce = 10f;
    [SerializeField] int trajectorySteps = 500;
    [SerializeField] Player player;

    LineRenderer carriedItemLineRenderer;
    Rigidbody2D carriedItemRb;


    Vector2 startClickPosition;
    Vector2 endClickPosition;
    Vector2 dragDistance;

    bool mouseDownThisFrame = true;

    private void OnEnable()
    {
        EventManager.OnPlayerPickedUpItem += HandleOnPlayerPickedUpItem;
        EventManager.OnPlayerThrewCarriedObject += HandleOnPlayerThrewCarriedObject;
    }

    private void OnDisable()
    {
        EventManager.OnPlayerPickedUpItem -= HandleOnPlayerPickedUpItem;
        EventManager.OnPlayerThrewCarriedObject -= HandleOnPlayerThrewCarriedObject;
    }

    private void Update()
    {
        if (player.carriedItem)
        {
            if (Input.GetMouseButton(0))
            {
                if (mouseDownThisFrame)
                {
                    carriedItemLineRenderer.enabled = true;
                    startClickPosition = Input.mousePosition;
                    mouseDownThisFrame = false;
                }

                CalculateDragDistance();
                DrawTrajectory();
            }
            else if (!mouseDownThisFrame)
            {
                endClickPosition = Input.mousePosition;
                ThrowCarriedObject();
                mouseDownThisFrame = true;
            }
        }
    }

    void DrawTrajectory()
    {
        Vector2 throwForce = dragDistance * throwForceMultiplier;
        if (throwForce.magnitude > maxThrowForce)
        {
            throwForce = ClampThrowForce(throwForce);
        }

        Vector2 velocity = GetVelocityFromForce(throwForce, carriedItemRb);

        Vector2[] trajectory = Plot(carriedItemRb, (Vector2)player.carriedItem.position, velocity, trajectorySteps);
        carriedItemLineRenderer.positionCount = trajectory.Length;

        //convert Vector2[] to Vector3[]
        Vector3[] positions = new Vector3[trajectory.Length];
        for (int i = 0; i < trajectory.Length; i++)
        {
            positions[i] = trajectory[i];
        }

        carriedItemLineRenderer.SetPositions(positions);
    }

    Vector2[] Plot(Rigidbody2D rigidbody, Vector2 pos, Vector2 velocity, int steps)
    {
        Vector2[] results = new Vector2[steps];

        float timestep = Time.fixedDeltaTime / Physics2D.velocityIterations;
        Vector2 gravityAccel = Physics2D.gravity * rigidbody.gravityScale * timestep * timestep;

        float drag = 1f - timestep * rigidbody.drag;
        Vector2 movestep = velocity * timestep;

        for (int i = 0; i < steps; i++)
        {
            movestep += gravityAccel;
            movestep *= drag;
            pos += movestep;
            results[i] = pos;
        }

        return results;
    }

    Vector2 GetVelocityFromForce(Vector2 force, Rigidbody2D rigidbody)
    {
        float velocityMagnitude = (force.magnitude / rigidbody.mass) * 0.02f;
        return force.normalized * velocityMagnitude;
    }

    void CalculateDragDistance()
    {
        dragDistance = startClickPosition - (Vector2)Input.mousePosition;
    }

    void ThrowCarriedObject()
    {
        Vector2 throwForce = throwForceMultiplier * (startClickPosition - endClickPosition);
        if (throwForce.magnitude > maxThrowForce)
        {
            throwForce = ClampThrowForce(throwForce);
        }

        EventManager.RaiseOnPlayerThrewCarriedObject(transform, throwForce);
    }

    Vector2 ClampThrowForce(Vector2 force)
    {
        return Vector2.ClampMagnitude(force, maxThrowForce);
    }

    void HandleOnPlayerPickedUpItem(Transform transform)
    {
        StartCoroutine(co_HandlePlayerPickedUpItem());
    }

    IEnumerator co_HandlePlayerPickedUpItem()
    {
        yield return null;

        carriedItemRb = player.carriedItem.GetComponent<Rigidbody2D>();
        carriedItemLineRenderer = player.carriedItem.GetComponent<LineRenderer>();
    }

    void HandleOnPlayerThrewCarriedObject(Transform unneeded, Vector2 unneeded1)
    {
        carriedItemRb = null;
        carriedItemLineRenderer.enabled = false;
        carriedItemLineRenderer = null;
    }

}
