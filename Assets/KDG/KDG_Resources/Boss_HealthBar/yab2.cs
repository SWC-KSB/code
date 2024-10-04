using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yab2 : MonoBehaviour
{
    // ����� ������Ʈ
    public GameObject object1;
    

    // ���� ������ ������Ʈ
    public GameObject newObjectPrefab;

    // �� ������Ʈ�� ������ ��ġ
    public Vector3 spawnPosition;

    // �� ������Ʈ�� ��� ��������� üũ�ϴ� ����
    private bool isObject1Destroyed = false;
    

    void Update()
    {
        // ù ��° ������Ʈ�� ��������� Ȯ��
        if (object1 == null && !isObject1Destroyed)
        {
            isObject1Destroyed = true;
        }

       

        // �� ������Ʈ�� ��� ������ٸ� �� ������Ʈ ����
        if (isObject1Destroyed )
        {
            Instantiate(newObjectPrefab, spawnPosition, Quaternion.identity);

            // ������Ʈ�� ������ �Ŀ��� �� ��ũ��Ʈ�� �� �̻� �������� �ʵ��� �Ѵ�
            this.enabled = false;
        }
    }
}
