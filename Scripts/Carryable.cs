using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carryable : MonoBehaviour
{
    public Vector2 positionOffset;

    bool isCarried;
    Transform carrier;

    public bool IsCarried
    {
        get {return isCarried;}
    }
    public Transform Carrier
    {
        get
        {
            if (carrier)
            {
                return carrier;
            }
            else
            {
                Debug.LogWarning("Carrier property on " + name + " was accessed, but Carrier is null!");
                return null;
            }
        }
    }

    private void Update()
    {
        if (isCarried)
        {
            transform.position = (Vector2)carrier.position + positionOffset;
        }
    }

    internal void Carry(Transform _carrier)
    {
        if (!isCarried)
        {
            carrier = _carrier;
            isCarried = true;
        }
        else
        {
            Debug.LogWarning(_carrier.name + " tried to carry " + name + ", but it is already being carried!");
        }
    }

    internal void Drop()
    {
        if (isCarried)
        {
            carrier = null;
            isCarried = false;
        }
        else
        {
            Debug.LogWarning("Tried to drop " + name + ", but it has already been dropped!");
        }
    }
}
