using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Title : BaseScene
{
    public Button audioOn;
    public Button audioOff;

    protected override void Register()
    {
        AddChannel(Channel.C1, Scene.Help);
        AddChannel(Channel.C2, Scene.Game);
    }

    void Awake()
    {
        AudioManager.Instance.PlayBackground(BackgroundType.bgm_start);
    }

    public void ToGameScene()
    {
        SceneMng.Instance.Enable(Scene.Game);
    }

    public void ToSettingScene()
    {

    }

    public void ToHelpScene()
    {
        SceneMng.Instance.Enable(Scene.Help, true);
        //SceneMng.Instance.Enable(Scene.Help);
    }

    public void AudioOff()
    {
        AudioManager.Instance.SetVolume(0);
        audioOn.gameObject.SetActive(false);
        audioOff.gameObject.SetActive(true);        
    }

    public void AudioOn()
    {
        AudioManager.Instance.SetVolume(1);
        audioOn.gameObject.SetActive(true);
        audioOff.gameObject.SetActive(false);
    }
}
