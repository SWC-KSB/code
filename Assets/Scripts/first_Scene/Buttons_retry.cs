using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons_retry : MonoBehaviour
{
    public void StartButton()
    {
        SceneManager.LoadScene("start_Scene");
    }
}
