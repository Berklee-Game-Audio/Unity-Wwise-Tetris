using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Score : ScoreScreenType<Score>
{
    public int PlayerScore
    {
        get
        {
            return mInternalPoints;
        }
    }

    private int mInternalPoints = 0;

    public void AddPoints(int points)
    {
        mInternalPoints += points;
        SetLines(mInternalPoints);
    }
    public void ResetScore(int[] stages)
    {
        mInternalPoints = 0;
        SetStages(stages);
        SetLines(mInternalPoints);
        ShowScreen(1f);
    }

    public override void ShowScreen(float timeToTween = 1f)
    {
        base.ShowScreen(timeToTween);
        StopCoroutine(WaitAndHide());
        // StartCoroutine(WaitAndHide());
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
