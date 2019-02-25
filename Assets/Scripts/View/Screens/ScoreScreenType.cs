using UnityEngine;
using UnityEngine.UI;

public abstract class ScoreScreenType<T> : BaseScreen<T> where T : Component
{
    [SerializeField]
    protected Text scoreText;
    [SerializeField]
    protected Text speedText;

    [SerializeField]
    protected Text titleText;

    [SerializeField]
    protected Text stageText;

    [HideInInspector]
    public int speed = 1;
    protected string linesPrefix = "LINES: ";
    protected string speedPrefix = "SPEED: ";
    protected string stagePrefix = "STAGE: ";

    protected int[] stages;

    public void SetStages(int[] arr)
    {
        stages = arr;
        SetStage(0);
    }

    public void SetSpeed()
    {
        speed++;
        speedText.text = speedPrefix + speed + "x";
    }
    public void SetLines(int value)
    {
        scoreText.text = linesPrefix + value + "/" + stages[stages.Length - 1];
    }

    public void SetLines(int value, bool isWin, int stages)
    {
        scoreText.text = isWin ? "You cleared all " + stages + " stages!" : linesPrefix + value;
        titleText.text = isWin ? "GAME WON" : "GAME OVER";
    }

    public void SetStage(int value)
    {
        stageText.text = stagePrefix + (value + 1) + "/" + stages.Length;
    }
}
