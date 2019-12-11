using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseScene : MonoBehaviour
{
    Dictionary<Channel, Scene> channel = new Dictionary<Channel, Scene>();

    object message;
    public object Message
    {
        get { return message; }
        set { message = value; }
    }

    float elapsedTime = 0;
    bool isLoad = false;
    bool virtualLoad = false;
    public bool VirtualLoad
    {
        set { virtualLoad = value; }
    }

    AsyncOperation asyncOperation;

    public void Init()
    {
        Register();
    }

    protected virtual void Register()
    {

    }

    protected virtual void Run()
    {

    }
    
    public virtual void SceneUpdate(float progress)
    {

    }

    protected virtual IEnumerator IELoadScene(AsyncOperation operation)
    {
        while (!isLoad)
        {
            SceneUpdate(operation.progress);
            isLoad = operation.isDone;
            yield return null;
        }
    }

    // 페이크 로딩용
    protected virtual IEnumerator IELoadScene(AsyncOperation operation, bool none)
    {
        bool state = false;
        while (!state)
        {
            elapsedTime += Time.deltaTime;
            elapsedTime = Mathf.Clamp01(elapsedTime);

            if(elapsedTime >= 1.0f)
            {
                elapsedTime = 0;
                state = true;
            }

            yield return null;
        }
    }

    public void LoadAsyncScene(string sceneName)
    {
        asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        if(virtualLoad)
            StartCoroutine(IELoadScene(asyncOperation, true));
        else
            StartCoroutine(IELoadScene(asyncOperation));
    }

    public void LoadAdditiveScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }

    protected void AddChannel (Channel ch, Scene sc)
    {
        if (!channel.ContainsKey(ch))
            channel.Add(ch, sc);
    }

    public bool ContainsKey(Channel ch)
    {
        if (channel.ContainsKey(ch))
            return true;
        else
            return false;
    }

    public Scene GetScene(Channel ch)
    {
        if (channel.ContainsKey(ch))
            return channel[ch];
        else
            return Scene.None;
    }
}
