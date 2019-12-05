using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logo : MonoBehaviour
{
    float elapsedTime = 0;

    void Start()
    {
        SceneMng.Instance.AddScene<Title>(Scene.Title);
        SceneMng.Instance.AddScene<Game>(Scene.Board, true);
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= 2.0f)
        {
            SceneMng.Instance.Enable(Scene.Board);
        }
    }
}
