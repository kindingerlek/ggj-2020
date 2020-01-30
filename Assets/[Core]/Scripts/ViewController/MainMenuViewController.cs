using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuViewController : MonoBehaviour
{
    [SerializeField]
    private Button bnt_StartGame;
    
    [SerializeField]
    private Button bnt_Options;
    
    [SerializeField]
    private Button bnt_Credits;
    
    [SerializeField]
    private Button bnt_Quit;
    
    [SerializeField]
    private SceneField GameScene;


    // Start is called before the first frame update
    void Start()
    {
        bnt_StartGame.onClick.AddListener(OnStartGameClicked);
        bnt_Options.onClick.AddListener(OnOptionsClicked);
        bnt_Credits.onClick.AddListener(OnCreditsClicked);
        bnt_Quit.onClick.AddListener(OnQuitClicked);
    }

    public void OnStartGameClicked()
    {
        LoadSceneManager.LoadScene(GameScene);
    }

    public void OnOptionsClicked()
    {
        Debug.Log("Button Options was clicked");
    }

    public void OnCreditsClicked()
    {
        Debug.Log("Button Credits was clicked");
    }

    public void OnQuitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}
