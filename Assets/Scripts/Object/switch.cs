using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour
{
    public GameObject swch;
    public GameObject obj;
    private bool isTrigger = false;
    // Start is called before the first frame update
    void Start()
    {
        swch.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isTrigger && Input.GetKeyDown(KeyCode.F))
        {
            obj.SetActive(!obj.activeSelf);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        swch.SetActive(true);
        isTrigger = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        isTrigger = false;
        swch.SetActive(false);

    }

}
