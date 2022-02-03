using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    public void ThrowObject(Vector2 throwForce)
    {
        rb.AddForce(throwForce);
    }
}
