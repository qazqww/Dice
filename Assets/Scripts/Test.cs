using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

enum TestEnum
{
    one,
    two
}

public class Test : MonoBehaviour
{
    Dictionary<int, string> place = new Dictionary<int, string>();
    int p = 0;

    void Start()
    {
        AudioManager.Instance.LoadClip<BackgroundType>("BGM/");
        AudioManager.Instance.LoadClip<SoundType>("Sounds/");
        //AudioManager.Instance.PlayBackground(BackgroundType.bgm_start);
    }

    void Update()
    {
        //Debug.Log(Board.charCode);

        if(Input.GetKeyDown(KeyCode.R))
        {
            int num = Random.Range(1, 4);
            string str = "rock_impact_small_hit_0" + num;
            Debug.Log(str);
            AudioManager.Instance.PlayUISound(str);
        }
    }

    void OnMouseOver()
    {
        //If your mouse hovers over the GameObject with the script attached, output this message
        Debug.Log("Mouse is over GameObject.");
    }

    void OnMouseExit()
    {
        //The mouse is no longer hovering over the GameObject so output this message each frame
        Debug.Log("Mouse is no longer on GameObject.");
    }

    private void OnGUI()
    {
        if(GUI.Button(new Rect(0,0,200,200), "Test"))
        {
            StartCoroutine(LoadScene("Combat"));
        }
    }

    public IEnumerator LoadScene(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while(!operation.isDone)
        {
            yield return null;
            Debug.Log(operation.progress);
        }
    }
}
