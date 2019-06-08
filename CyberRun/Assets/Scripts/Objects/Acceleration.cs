using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acceleration : MonoBehaviour
{
    [SerializeField] private float AccelerationSpeed;
    private GameObject Player;
    private GameObject PlayerObj;

    private void Start()
    {
        PlayerObj = GameObject.Find("NewPlayer");
    }

    private void FixedUpdate()
    {
        if (transform.position.z < PlayerObj.transform.GetChild(0).transform.GetChild(1).transform.position.z - 200 && gameObject.active)
            gameObject.SetActive(false);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //print("Collider");
            Player = other.gameObject;
            Player.GetComponent<Player>().AdditionalSpeed += AccelerationSpeed;
            StartCoroutine("Fade");
            Player.GetComponent<PlayerParticles>().SpeedParticleActivate(true); // Activate Speed Particle
        }
    }


    IEnumerator Fade()
    {
        yield return new WaitForSeconds(5);
        Player.GetComponent<Player>().AdditionalSpeed -= AccelerationSpeed;
        Player.GetComponent<PlayerParticles>().SpeedParticleActivate(false); // Activate Speed Particle
    }
}
