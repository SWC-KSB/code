using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ConfirmButton : MonoBehaviour
{
    // Start is called before the first frame update
    public string src;
    // Update is called once per frame
    public void OnClick()    
    { 
        SceneManager.LoadScene(src);
    }
}
