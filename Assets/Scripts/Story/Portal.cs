using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System;
using System.IO;
using UnityEngine.SceneManagement;
using Mono.Data.Sqlite;
public class Portal : MonoBehaviour
{
    public string sceneToLoad; // 이동할 씬의 이름
    private string dbname = "/data.db";

    // 플레이어가 포탈에 들어오면 씬을 로드
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // 포탈에 들어온 것이 플레이어일 때만
        {
            LoadScene();
        }
    }

    // 씬 로드 함수
    public void LoadScene()
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
                    command.CommandText = "UPDATE data SET stage = @stage";
                    IDbDataParameter stageParma = command.CreateParameter();
                    stageParma.ParameterName = "@stage";
                    stageParma.Value = sceneToLoad;
                    command.Parameters.Add(stageParma);
                    command.ExecuteNonQuery();
                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Debug.Log(ex);
                }


            }
        }
            SceneManager.LoadScene(sceneToLoad);
    }
}