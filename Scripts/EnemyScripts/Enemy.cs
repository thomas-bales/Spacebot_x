using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [HideInInspector] public Transform carriedItem;
    [HideInInspector] public bool canCarryItem = true;
    [SerializeField] AudioClipSO spawnSound;

    private void Start()
    {
        canCarryItem = true;
        //AudioManager.instance.PlayAudioClip(spawnSound);
    }
}
