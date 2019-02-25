using UnityEngine;
using UnityEngine.UI;
using System.Timers;
using System;
public class StartGame : BaseScreen<StartGame>
{
    [SerializeField]
    protected Text helpText;

    public void SetHelpText(string str)
    {
        helpText.text = str;
    }
}