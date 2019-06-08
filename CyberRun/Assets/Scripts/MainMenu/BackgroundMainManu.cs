using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMainManu : MonoBehaviour
{
    [SerializeField] private List<Color32> Colors;
    [Range(0,5)]
    public float Emission;
    int ColorSelected = 0;

    // Start is called before the first frame update
    void Awake()
    {
        ColorSelected = Random.RandomRange(0, Colors.Count);
    }


    public Color32 GetColor()
    {
        return Colors[ColorSelected];
    }

}
