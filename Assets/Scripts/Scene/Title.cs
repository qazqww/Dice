using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : BaseScene
{
    protected override void Register()
    {
        AddChannel(Channel.C1, Scene.Help);
        AddChannel(Channel.C2, Scene.Board);
    }

    void Start()
    {
        AudioManager.Instance.PlayBackground(BackgroundType.bgm_start);
    }

    //SceneMng.Instance.EventScene(Channel.C1);
}
