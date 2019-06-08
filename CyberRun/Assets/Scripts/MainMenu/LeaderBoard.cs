using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoard : MonoBehaviour
{
    public int[] LeaderBoardTab = new int[10];

    [Space]
    [Header("Leader Board Score Text")]
    [SerializeField] private GameObject ScoreTextPref;
    [Range(0, 40)]
    [SerializeField] private float DistanceBetweenScoreText;
    [SerializeField] private Vector2 ScoreTextPosition;


    private void Awake()
    {
        for (int i = 0; i < LeaderBoardTab.Length; i++)
        {
            LeaderBoardTab[i] = PlayerPrefs.GetInt(i.ToString());
        }
    }


    private void Start()
    {
        int count = 0;
        foreach (float Score in LeaderBoardTab)
        {
            GameObject TextObj = Instantiate(ScoreTextPref, transform);
            TextObj.transform.localPosition = new Vector3(ScoreTextPosition.x, ScoreTextPosition.y - (count * DistanceBetweenScoreText), 0);
            TextObj.GetComponent<Text>().text = (count + 1) + ". " + Score;
            //TextObj.GetComponent<>()
            count++;
        }
    }
}
