using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector] public Transform carriedItem;
    [HideInInspector] public bool canCarryItem;

    private void Start()
    {
        canCarryItem = true;
    }
}
