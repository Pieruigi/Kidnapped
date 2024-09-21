using Kidnapped;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped
{



}
public class CatActivator : MonoBehaviour
{
    [SerializeField]
    int state = 0;

    [SerializeField]
    Transform target;

    [Header("ScaredAndRunAway Behaviour Only")]
    [SerializeField]
    Transform destination; 

    [SerializeField]
    bool jumpDisabled = false; 

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(Tags.Player))
            return;

        CatController.Instance.ResetAll();

        CatController.Instance.gameObject.SetActive(true);

        switch (state)
        {
            case 0:
                CatController.Instance.StandAndPlayRandom(target.position, target.rotation);
                break;
            case 1:
                CatController.Instance.ScaredAndRunAway(destination.position, target.position, target.rotation, jumpDisabled);
                break;
        }

        gameObject.SetActive(false);
    }
}
