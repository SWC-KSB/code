using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SButton : MonoBehaviour
{
    public void StartButton()
    {
        SceneManager.LoadScene("start_Scene");
    }
}