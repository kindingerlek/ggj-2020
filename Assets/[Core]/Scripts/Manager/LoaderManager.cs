using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : SingletonMonoBehaviour<LoadSceneManager>
{

    [SerializeField]
    private LoadingViewController view;
    private AsyncOperation asyncOp;

    public static SceneField bufferLoad;
    public static float loadProgress;

    static LoadSceneManager()
    {
        Lazy = false;
        FindInactive = true;
        DestroyOthers = true;
        Persist = true;
    }

    public static void LoadScene(SceneField scene)
    {
        if (bufferLoad == null)
        {
            bufferLoad = scene;
            Instance.StartCoroutine(Instance.Init());
        }
    }

    private IEnumerator Init()
    {
        Scene loadingScene = SceneManager.GetSceneByName("Loading");

        if (loadingScene.buildIndex == -1)
        {
            SceneManager.LoadScene("Loading", LoadSceneMode.Single);
            yield return null;
        }
        view = GameObject.FindObjectOfType<LoadingViewController>();

        if(view == null)
        {
            SceneManager.LoadScene("Loading", LoadSceneMode.Single);
            yield return null;
        }

        if(bufferLoad != null)
            StartCoroutine(LoadAsynchronously(LoadSceneManager.bufferLoad, 1));
    }

    IEnumerator LoadAsynchronously(SceneField scene, float delay)
    {
        Debug.Log("Loading the scene: " + scene);
        asyncOp = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
        asyncOp.allowSceneActivation = false;

        LoadSceneManager.bufferLoad = null;

        yield return new WaitForSeconds(delay);

        while(!asyncOp.isDone) 
        {
            if (asyncOp.progress < 0.9f)
            {
                if (view != null)
                {
                    view.UpdateUI(Mathf.Clamp01(asyncOp.progress / 0.9f));
                    yield return null;
                }
            }
            else
            {
                if (view != null)
                    view.UpdateUI(1);
                yield return new WaitForSeconds(delay);
                asyncOp.allowSceneActivation = true;
            }
            yield return null;
        }        
    }
}