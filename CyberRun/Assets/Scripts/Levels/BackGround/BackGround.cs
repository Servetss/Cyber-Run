using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    [Tooltip("Flying Box for background")]
    [SerializeField] private bool FlyingBox;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (FlyingBox) Flying();  // Background Flying Box
    }

   

    private void Flying()
    {
        transform.rotation *= Quaternion.Euler(0, 0, 0.5f);
    }
}
