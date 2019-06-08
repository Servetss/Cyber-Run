using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backgrounds : MonoBehaviour
{
    [SerializeField] private List<GameObject> BackGroundsObj;
    [SerializeField] private List<Color32> LevelColors;
    private Color32 CurrentColor;

    [HideInInspector] public List<GameObject> ObstaclesReferences;
    [HideInInspector] public List<GameObject> OtherReferences;


    private void Awake()
    {
        CurrentColorChange();
    }

    public GameObject GetBackGround()
    {
        int BackSelected = Random.RandomRange(0, BackGroundsObj.Count);

        return BackGroundsObj[BackSelected];
    }

    public void CurrentColorChange()
    {
        int ColorSelected = Random.Range(0, LevelColors.Count);
        CurrentColor = LevelColors[ColorSelected];


        StartCoroutine(ChangeLevel());
    }

    private IEnumerator ChangeLevel()
    {
        yield return new WaitForSeconds(0.2f);
        foreach (GameObject Obstacl in OtherReferences)
            Obstacl.GetComponent<ColorChangeScript>().ColorChangeFunc(true);

        foreach (GameObject Obstacl in ObstaclesReferences)
            if (Obstacl.active) Obstacl.GetComponent<ColorChangeScript>().ColorChangeFunc(true);
    }

    public Color32 GetLevelColor()
    {
        return CurrentColor;
    }
}
