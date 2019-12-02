using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Channel
{
    None,
    C1,
    C2
}

public enum Scene
{
    None,
    Logo,
    Title,
    Help,
    Game
}

public class SceneMng : MonoBehaviour
{
    static SceneMng instance;
    public static SceneMng Instance {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("Scene Manager", typeof(SceneMng));
                instance = obj.GetComponent<SceneMng>();
                DontDestroyOnLoad(obj);
            }
            return instance;
        }
    }

    Dictionary<Scene, BaseScene> sceneDic = new Dictionary<Scene, BaseScene>();
    Scene curScene;
    object message;

    void Update()
    {
        if(sceneDic.ContainsKey(curScene))
        {
            object msg = sceneDic[curScene].Message;

            if (msg == null)
                return;

            System.Type t = msg.GetType();
            if(t == typeof(Channel))
            {
                EventScene((Channel)msg);
                sceneDic[curScene].Message = null;
            }
        }
    }

    public T AddScene<T>(Scene scene, bool virtualLoad = false) where T : BaseScene
    {
        if (sceneDic.ContainsKey(scene))
            return sceneDic[scene].GetComponent<T>();

        GameObject obj = new GameObject(scene.ToString(), typeof(T));
        obj.transform.SetParent(transform);

        T t = obj.GetComponent<T>();
        t.Init();
        t.VirtualLoad = virtualLoad;
        sceneDic.Add(scene, t);
        return t;
    }

    public void Enable(Scene scene)
    {
        foreach(KeyValuePair<Scene, BaseScene> pair in sceneDic)
        {
            if (pair.Key == scene)
                pair.Value.gameObject.SetActive(true);
            else
                pair.Value.gameObject.SetActive(false);
        }

        curScene = scene;
        sceneDic[curScene].LoadAsyncScene(scene.ToString());
    }

    public void EventScene(Channel ch)
    {
        Scene changeScene = sceneDic[curScene].GetScene(ch);
        if (changeScene != Scene.None)
            Enable(changeScene);
    }
}
