using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {

    public string gameScene;
    public Slider slider;
    public Text progressText;
    public Button startButton;
    public bool canLoad = false;
    AsyncOperation operation;
    public Button playButton;
    public Button optionButton;

    public void ButtonStartGame()
    {
        StartCoroutine(LoadGame(gameScene));
    }

    private void Update()
    {
        //Gets theme no. which sets which scene to load
        if (PlayerPrefs.GetInt("Theme") == 0)
        {
            gameScene = "Main Scene";
        }
        else if (PlayerPrefs.GetInt("Theme") == 1)
        {
            gameScene = "Pirate Scene";
        }
    }


    public IEnumerator LoadGame(string sceneName)
    {
        //Loads game scene in background to prevent lag
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        //Slider shows progress as calculated below
        slider.gameObject.SetActive(true);
        playButton.gameObject.SetActive(false);
        optionButton.gameObject.SetActive(false);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            slider.value = progress;
            if (operation.progress >= 0.9f)
            {
                //startButton.gameObject.SetActive(true);
                if (canLoad)
                {
                    operation.allowSceneActivation = true;
                }
            }
            yield return null;
        }
    }

    public void StartButton()
    {
        canLoad = true;
    }

    public void ButtonQuit()
    {
        Application.Quit();
    }
}
