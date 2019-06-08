using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcellerationParticle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody bomb = GetComponent<Rigidbody>();
        bomb.AddTorque(0, 0, 20);
    }

}
