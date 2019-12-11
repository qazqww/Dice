using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Help : BaseScene
{
    protected override void Register()
    {
        AddChannel(Channel.C1, Scene.Title);
    }

    public void ToTitleScene()
    {
        SceneManager.UnloadSceneAsync("Help");
        //SceneMng.Instance.Enable(Scene.Title);
    }
}
