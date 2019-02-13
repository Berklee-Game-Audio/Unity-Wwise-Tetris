using UnityEngine;
using UnityEngine.UI;

public abstract class ScoreScreenType<T> : BaseScreen<T> where T : Component
{
    [SerializeField]
    protected Text scoreText;

    protected string mScorePrefix = "SCORE: ";

    protected void SetScoreText(int value)
    {
        scoreText.text = mScorePrefix + value;
    }
}
