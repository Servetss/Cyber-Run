using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChangeScript : MonoBehaviour
{
    [SerializeField] private float Emission = 0.1f;
    [Tooltip("IF this is the Box - click True")]
    [SerializeField] private bool Obstacles;

    private Backgrounds BackgroundSettings;
    [HideInInspector] public Color32 CurrentColor;
    private Renderer render;



    private Color32 ColorSave; // From this Save Color to New Color
    private Color32 NewColor;
    private bool ChangeColor = false;

    private GameObject Player;
    



    // Start is called before the first frame update
    void Start()
    {
        render = gameObject.transform.GetChild(0).GetComponent<Renderer>();
        BackgroundSettings = GameObject.Find("Backgrounds").GetComponent<Backgrounds>();
        CurrentColor = BackgroundSettings.GetLevelColor();
        ColorSave = CurrentColor;
        render.material.SetVector("_EmissionColor", new Vector4(CurrentColor.r, CurrentColor.g, CurrentColor.b) * Emission);

        if (Obstacles) BackgroundSettings.OtherReferences.Add(gameObject);
        else BackgroundSettings.ObstaclesReferences.Add(gameObject);

        Player = GameObject.Find("character");

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ColorChange();
        AlphaChange();
    }


    public void ColorChangeFunc(bool Change)
    {
        lerp = 0;
        ChangeColor = Change;
        if (ChangeColor) NewColor = BackgroundSettings.GetLevelColor();
        else ColorSave = CurrentColor;
    }

    private float lerp = 0;
    private void ColorChange()
    {
        if (ChangeColor)
        {
            lerp += (Time.deltaTime * 0.5f);
            CurrentColor = Color32.Lerp(ColorSave, NewColor, lerp);
            render.material.SetVector("_EmissionColor", new Vector4(CurrentColor.r, CurrentColor.g, CurrentColor.b) * Emission);
            if (lerp >= 1) ColorChangeFunc(false);
        }
    }

    private void AlphaChange()
    {
        render.material.SetVector("_PlayerPos", Player.transform.position);
    }
}
