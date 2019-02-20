﻿using System.Collections;
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
    public void ResetScore()
    {
        mInternalPoints = 0;
        SetLines(mInternalPoints);
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
