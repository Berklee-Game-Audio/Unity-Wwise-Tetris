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

    [SerializeField]
    protected GameObject playAgain;

    [SerializeField]
    protected GameObject playNextLevel;

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

    public void SetLines(int value, bool isWin, int stages, bool isDone)
    {
        Debug.Log("isDone = " + isDone);
        Debug.Log("isWin = " + isWin);

        if(isDone && isWin)
        {
            titleText.text = "GAME WON - ALL LEVELS COMPLETED" ;
            playNextLevel.SetActive(false);
            playAgain.SetActive(true);
        }

        if (isDone && !isWin)
        {
            titleText.text = "LEVEL LOST";
            playNextLevel.SetActive(false);
            playAgain.SetActive(true);
        }


        if (!isDone && !isWin)
        {
            titleText.text = "LEVEL LOST";
            playNextLevel.SetActive(false);
            playAgain.SetActive(true);
        }


        //: "LEVEL COMPLETED"

        scoreText.text = isWin ? "You cleared all " + stages + " stages!" : linesPrefix + value;
        //scoreText.text = (isWin && isDone) ? "You cleared all " + stages + " stages!" : linesPrefix + value;
        
        scoreText.text = isDone ? "" : scoreText.text;

        Debug.Log("WIN LOSE STUFF");
    }

    public void SetStage(int value)
    {
        stageText.text = stagePrefix + (value + 1) + "/" + stages.Length;
    }
}
