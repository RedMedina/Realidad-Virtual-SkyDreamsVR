using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Start_Level : MonoBehaviour
{

    public Button StartBtn;
    public Button CloseBtn;

    public GameObject LoadingScreen;
    public Image BarScreen;

    // Start is called before the first frame update
    void Start()
    {
        Button btnS = StartBtn.GetComponent<Button>();
        btnS.onClick.AddListener(LoadS);

        Button btnC = CloseBtn.GetComponent<Button>();
        btnC.onClick.AddListener(CerrarAplicacion);
    }

    public void LoadS() 
    {
        StartCoroutine(LoadScene());
    }

    public IEnumerator LoadScene()
    {
        LoadingScreen.SetActive(true);
        AsyncOperation Operation = SceneManager.LoadSceneAsync(1);
        while (!Operation.isDone)
        {
            float progressValue = Mathf.Clamp01(Operation.progress/0.9f);
            BarScreen.fillAmount = progressValue;
            yield return null;
        }
    }

    public void CerrarAplicacion()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
}
