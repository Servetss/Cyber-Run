using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineScript : MonoBehaviour
{
    [Range(0, 30)]
    [SerializeField] private float Angle;   // Trampoline Angle

    private GameObject PlayerObj;
    private Player PlayerScript;
    private Rigidbody RigidB;

    private void Start()
    {
        PlayerObj = GameObject.Find("NewPlayer");
        PlayerScript = PlayerObj.transform.GetChild(0).gameObject.GetComponent<Player>();
        RigidB = PlayerObj.transform.GetChild(0).GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (transform.position.z < PlayerObj.transform.GetChild(0).transform.GetChild(1).transform.position.z - 5 && gameObject.active)
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == PlayerObj.transform.GetChild(0).gameObject)
        {
            float PlayerSpeed = PlayerScript.Speed + PlayerScript.AdditionalSpeed;
            Vector3 ImpulseTO = new Vector3(0, Angle, transform.localPosition.z + 3);

            RigidB.AddForce(ImpulseTO.normalized * (PlayerSpeed / 15), ForceMode.Impulse);
        }
    }
}
