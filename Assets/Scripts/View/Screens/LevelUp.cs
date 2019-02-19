using UnityEngine;
using UnityEngine.UI;
using System.Timers;
using System;
public class LevelUp : BaseScreen<LevelUp>
{
    [SerializeField]
    protected Text title;
    private Timer timer;
    private int level;
    public void SetLevel(int current)
    {
        level = current;
        title.text = "STAGE " + (level + 1);
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