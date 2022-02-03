using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixAnimator : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.animator = GetComponent<Animator>();
    }
}
