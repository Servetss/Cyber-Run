using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMainMenu : MonoBehaviour
{

    float PosY = 0;
    float FlyRange;
    Vector3 position;

    private BackgroundMainManu BMM;
    [HideInInspector] public Color32 CurrentColor;
    private Renderer render;


    [SerializeField] private bool Fly = false;
    [Range(0.1f, 50)]
    [SerializeField] private float Speed = 1;
    [SerializeField] private Vector3 StartPosition;
    [SerializeField] private Vector3 EndPosition;
    // Total distance between the markers.
    private float journeyLength;
    private bool CanFly = true;

    private void Start()
    {
        FlyRange = Random.RandomRange(0.005f, 0.01f);
        BMM = gameObject.transform.parent.GetComponent<BackgroundMainManu>();
        CurrentColor = BMM.GetColor();

        render = gameObject.transform.GetChild(0).GetComponent<Renderer>();
        render.material.SetVector("_EmissionColor", new Vector4(CurrentColor.r, CurrentColor.g, CurrentColor.b) * BMM.Emission);

        if (Fly)
        {
            SetRandomPosYFly();
            journeyLength = Vector3.Distance(StartPosition, EndPosition);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!Fly)
        {
            PosY += Time.deltaTime;
            float Y = transform.localPosition.y + (Mathf.Sin(PosY) * FlyRange);
            transform.localPosition = new Vector3(transform.localPosition.x, Y, transform.localPosition.z);

            transform.Rotate(new Vector3(0.5f, 0, 0));
        }
        else
            BoxMove();
    }

    #region BoxFly

    float fracJourney = 0;
    float distCovered = 0;
    private void BoxMove()
    {
        if (CanFly)
        {
            // Distance moved = time * speed.
            distCovered += Time.deltaTime * Speed;

            // Fraction of journey completed = current distance divided by total distance.
            fracJourney = distCovered / journeyLength;

            // Set our position as a fraction of the distance between the markers.
            transform.localPosition = Vector3.Lerp(StartPosition, EndPosition, fracJourney);
            if (fracJourney >= 1)
                StartCoroutine(Delay());
        }
    }


    private void SetRandomPosYFly()
    {
        StartPosition.y = Random.RandomRange(1, 40);
        EndPosition.y = Random.RandomRange(-4, 20);
    }

    private IEnumerator Delay()
    {
        CanFly = false;
        fracJourney = 0;
        distCovered = 0;
        SetRandomPosYFly();
        yield return new WaitForSeconds(5);
        CanFly = true;
    }
    #endregion
}
