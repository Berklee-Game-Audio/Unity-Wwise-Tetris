using UnityEngine;
using UnityEngine.UI;
using System.Timers;
using System;
public class StageUp : BaseScreen<StageUp>
{
    [SerializeField]
    protected Text title;
    private Timer timer;
    private int stage;
    public void SetStage(int current)
    {
        //Debug.Log("SetStage: " + current);
        stage = current;
        title.text = "STAGE " + (stage + 1);
        base.ShowScreen(1f);

        timer = new Timer(2000);
        timer.Elapsed += OnTimedEvent;
        timer.AutoReset = true;
        timer.Enabled = true;

    }

    private void OnTimedEvent(System.Object source, ElapsedEventArgs e)
    {
        timer.Enabled = false;
        base.HideScreen(1f);
    }
}