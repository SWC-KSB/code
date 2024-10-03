using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System;
using Mono.Data.Sqlite;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public string dbname = "/data.db";
    public ConfirmButton button;
    public GameObject ContinueObject;
    public void StartButton()
    {
        string connectstring = @"Data Source=" + Application.streamingAssetsPath + dbname + ";";
        using (IDbConnection con = new SqliteConnection(connectstring))
        {
            con.Open();
            using (IDbTransaction transaction = con.BeginTransaction())
            {
                try
                {
                    IDbCommand command = con.CreateCommand();
                    command.CommandText = "SELECT stage FROM data";
                    IDataReader reader = command.ExecuteReader();
                    object stage = reader.GetValue(0);
                    string stageStr = stage.ToString();
                    Debug.Log(stage.ToString());
                    if (stageStr!=null)
                    {
                        ContinueObject.gameObject.SetActive(true);
                        button.src = stageStr;
                    }
                    else
                    {
                        SceneManager.LoadScene("Scene1");
                    }
                    //transaction.Commit();

                }
                catch (Exception ex)
                {
                    //transaction.Rollback();
                    Debug.Log(ex);
                }


            }
        }

    }
}
