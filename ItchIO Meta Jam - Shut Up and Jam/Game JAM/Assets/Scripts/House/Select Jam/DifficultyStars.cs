using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyStars : MonoBehaviour
{
    public GameObject[] FullStars;
    public GameObject[] HalfStars;

    public void SetDifficulty(int value)
    {
        FullStars.DoOnAll(new System.Func<GameObject, GameObject>((obj) => { obj.SetActive(false); return obj; }));
        HalfStars.DoOnAll(new System.Func<GameObject, GameObject>((obj) => { obj.SetActive(false); return obj; }));

        int fulls = value / 2;
        int halves = value % 2;

        for(int i = 0; i < fulls; i++)
        {
            FullStars[i].SetActive(true);
        }

        if (value < 10 && halves == 1)
        {
            HalfStars[fulls].SetActive(true);
        }
    }
}
