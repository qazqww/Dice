using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : BaseScene
{
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
}
