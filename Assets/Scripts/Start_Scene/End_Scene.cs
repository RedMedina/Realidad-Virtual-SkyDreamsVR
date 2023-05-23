using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class End_Scene : MonoBehaviour
{
    public Button BackBtn;
    // Start is called before the first frame update
    void Start()
    {
        Button btnS = BackBtn.GetComponent<Button>();
        btnS.onClick.AddListener(BackScene);
    }

    public void BackScene() 
    {
        SceneManager.LoadScene("Start");
    } 
}
