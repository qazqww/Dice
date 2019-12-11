using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logo : MonoBehaviour
{
    float elapsedTime = 0;

    void Start()
    {
        SceneMng.Instance.AddScene<Title>(Scene.Title);
        SceneMng.Instance.AddScene<Help>(Scene.Help);
        SceneMng.Instance.AddScene<Game>(Scene.Game);

        AudioManager.Instance.LoadClip<BackgroundType>("BGM/");
        AudioManager.Instance.LoadClip<SoundType>("Sounds/");

        AudioManager.Instance.PlayBackground(BackgroundType.logo, false);
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= 2.0f)
        {
            SceneMng.Instance.Enable(Scene.Title);
        }
    }
}
