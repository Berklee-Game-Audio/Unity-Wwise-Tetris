using UnityEngine;

public class GameOver : ScoreScreenType<GameOver>
{

    public override void ShowScreen(float timeToTween = TIME_TO_TWEEN)
    {
        SetScoreText(Score.instance.PlayerScore);
        base.ShowScreen(timeToTween);
    }

    public void ShowScreen(bool isWin, int levels, float timeToTween = TIME_TO_TWEEN)
    {
        SetScoreText(Score.instance.PlayerScore, isWin, levels);
        base.ShowScreen(timeToTween);
    }
}