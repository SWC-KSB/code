using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QButton : MonoBehaviour
{
    public void QuitButton()
    {
        SceneManager.LoadScene("start_Scene");
    }
}