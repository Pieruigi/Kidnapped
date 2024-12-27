using EvolveGames;
using Kidnapped;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JinxInTheBathroom : MonoBehaviour
{
    [SerializeField]
    GameObject jinxPrefab;

    [SerializeField]
    Transform jinxTarget;

    [SerializeField]
    PlayerWalkInAndLookTrigger jinxTrigger;

    [SerializeField]
    Transform teleportTarget;

    GameObject jinx;

    // Start is called before the first frame update
    void Start()
    {
        // Spawn Jinx
        jinx = Instantiate(jinxPrefab, jinxTarget.position, jinxTarget.rotation);

        // Disable collider and set kinematic
        jinx.GetComponent<Collider>().enabled = false;
        jinx.GetComponent<Rigidbody>().isKinematic = true;

        // Set animation
        jinx.GetComponent<SimpleCatController>().Lick();

    }

    private void OnEnable()
    {
        jinxTrigger.OnEnter += HandleOnTriggerEnter;
    }

    private void OnDisable()
    {
        jinxTrigger.OnEnter -= HandleOnTriggerEnter;
    }

    private void HandleOnTriggerEnter(PlayerWalkInAndLookTrigger t)
    {
        // Disable trigger
        t.gameObject.SetActive(false);

        // Fliker
        FlashlightFlickerController.Instance.FlickerToDarkeness(onLightOffCallback: HandleOnLightOff);
    }

    private void HandleOnLightOff(float duration)
    {
        // Unspawn Jinx
        Destroy(jinx);

        // Teleport player
        PlayerController.Instance.ForcePositionAndRotation(teleportTarget.position, teleportTarget.rotation);

        // Move next
        GetComponent<GameplayGroup>().MoveToNextElement();
    }
}
