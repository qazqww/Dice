using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour
{
    Dictionary<int, string> place = new Dictionary<int, string>();

    void Start()
    {
        place.Add(10, "ten");
        place.Add(20, "2ten");
        place.Add(30, "3ten");
    }

    void Update()
    {

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
