using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Score : ScoreScreenType<Score>
{

    private int lastStage;
    public int PlayerScore
    {
        get
        {
            return mInternalPoints;
        }
    }

    private int mInternalPoints = 0;


    public void SetLastStage(int num)
    {
        lastStage = num;
    }
    public void AddPoints(int points)
    {
        mInternalPoints += points;
        SetScoreText(mInternalPoints, lastStage);
        ShowScreen();
    }
    public void ResetScore()
    {
        mInternalPoints = 0;
        SetScoreText(mInternalPoints, lastStage);
    }

    public override void ShowScreen(float timeToTween = 1f)
    {
        base.ShowScreen(timeToTween);
        StopCoroutine(WaitAndHide());
        StartCoroutine(WaitAndHide());
    }

    private IEnumerator WaitAndHide()
    {
        yield return new WaitForSeconds(2f);
        HideScreen();
    }

    protected override void InternalAlphaScreen(float timeToTween, float alpha, TweenCallback callback)
    {
        base.InternalAlphaScreen(timeToTween, Mathf.Clamp(alpha, 1f, 1f), callback);
    }
}
