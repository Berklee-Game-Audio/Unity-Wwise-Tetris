using UnityEngine;
using UnityEngine.UI;

public abstract class ScoreScreenType<T> : BaseScreen<T> where T : Component
{
    [SerializeField]
    protected Text scoreText;

    [SerializeField]
    protected Text title;

    protected string mScorePrefix = "LINES: ";

    public void DisplayText(string str)
    {
        scoreText.text = str;
    }
    public void SetScoreText(int value)
    {
        scoreText.text = mScorePrefix + value;
    }

    public void SetScoreText(int value, int lastStage)
    {
        scoreText.text = mScorePrefix + value + "/" + lastStage + "";
    }

    public void SetScoreText(int value, bool isWin, int levels)
    {
        scoreText.text = isWin ? "You cleared all " + levels + " levels!" : mScorePrefix + value;
        title.text = isWin ? "GAME WON" : "GAME OVER";
    }
}
