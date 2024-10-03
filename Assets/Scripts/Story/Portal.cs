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
    public string sceneToLoad; // �̵��� ���� �̸�
    private string dbname = "/data.db";

    // �÷��̾ ��Ż�� ������ ���� �ε�
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // ��Ż�� ���� ���� �÷��̾��� ����
        {
            LoadScene();
        }
    }

    // �� �ε� �Լ�
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