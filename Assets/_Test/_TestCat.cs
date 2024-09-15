using Kidnapped;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _TestCat : MonoBehaviour
{
    [SerializeField]
    Transform runTarget;

    [SerializeField]
    Transform startTarget;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            CatController.Instance.StandAndPlayRandom(startTarget.position, startTarget.rotation);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            CatController.Instance.ScaredAndRunAway(runTarget.position);   
        }

    }
}
