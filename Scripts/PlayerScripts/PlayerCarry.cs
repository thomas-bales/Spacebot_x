using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarry : MonoBehaviour
{
    bool isEPressed;
    [SerializeField] Player player;
    [SerializeField] Animator animator;
    [SerializeField] AudioClipSO carrySound;
    [SerializeField] AudioClipSO throwSound;

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

    void Update()
    {
        CheckForInput();

        if (isEPressed)
        {
            RaiseOnPlayerTriedToPickSomethingUp();
        }
    }

    void CheckForInput()
    {
        if (Input.GetKey(KeyCode.E))
        {
            isEPressed = true;
        }
        else
        {
            isEPressed = false;
        }
    }

    void RaiseOnPlayerTriedToPickSomethingUp()
    {
        EventManager.RaiseOnPlayerTriedToPickSomethingUp(transform);
    }

    void HandleOnPlayerPickedUpItem(Transform item)
    {
        player.carriedItem = item;
        player.canCarryItem = false;

        animator.SetBool("isPlayerCarryingSomething", true);
        AudioManager.instance.PlayAudioClip(carrySound);
    }

    void HandleOnPlayerThrewCarriedObject(Transform unneeded, Vector2 unneeded1)
    {
        player.carriedItem = null;
        player.canCarryItem = true;

        animator.SetBool("isPlayerCarryingSomething", false);
        AudioManager.instance.PlayAudioClip(throwSound);
    }
}
