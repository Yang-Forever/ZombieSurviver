using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleMgr : MonoBehaviour
{
    [Header("UI Setting")]
    public Button gameStart_Btn;
    public Button config_Btn;
    public Button quitGame_Btn;

    [Header("Config Setting")]
    public GameObject configPanel;
    public GameObject tutorialPanel;
    public Button configClose_Btn;
    public Button tutorial_Btn;
    public Button tutoExit_Btn;

    // Start is called before the first frame update
    void Start()
    {
        if (gameStart_Btn != null)
            gameStart_Btn.onClick.AddListener(() =>
            {
                FadeMgr.Inst.LoadScene("GameScene");
            });

        if (config_Btn != null)
            config_Btn.onClick.AddListener(() =>
            {
                configPanel.SetActive(true);
            });

        if (quitGame_Btn != null)
            quitGame_Btn.onClick.AddListener(() =>
            {
                Application.Quit();
            });

        if (configClose_Btn != null)
            configClose_Btn.onClick.AddListener(() =>
            {
                configPanel.SetActive(false);
            });

        if (tutorial_Btn != null)
            tutorial_Btn.onClick.AddListener(() =>
            {
                configPanel.SetActive(false);
                tutorialPanel.SetActive(true);
            });

        if (tutoExit_Btn != null)
            tutoExit_Btn.onClick.AddListener(() =>
            {
                configPanel.SetActive(true);
                tutorialPanel.SetActive(false);
            });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
