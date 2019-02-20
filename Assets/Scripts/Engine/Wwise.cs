using UnityEngine;
public class Wwise : ScriptableObject
{
    private GameObject instance;
    void OnEnable()
    {
        instance = GameObject.Find("WwiseGlobal");
    }
    public void Play(string eventName)
    {
        AkSoundEngine.PostEvent(eventName, instance);
    }

    public void RTPC(string name, float value)
    {
        AkSoundEngine.SetRTPCValue(name, value, instance);
    }
}