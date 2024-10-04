using UnityEngine;

public class Yab : MonoBehaviour
{
    // ����� ������Ʈ
    public GameObject object1;
    public GameObject object2;

    // ���� ������ ������Ʈ
    public GameObject newObjectPrefab;

    // �� ������Ʈ�� ������ ��ġ
    public Vector3 spawnPosition;

    // �� ������Ʈ�� ��� ��������� üũ�ϴ� ����
    private bool isObject1Destroyed = false;
    private bool isObject2Destroyed = false;

    void Update()
    {
        // ù ��° ������Ʈ�� ��������� Ȯ��
        if (object1 == null && !isObject1Destroyed)
        {
            isObject1Destroyed = true;
        }

        // �� ��° ������Ʈ�� ��������� Ȯ��
        if (object2 == null && !isObject2Destroyed)
        {
            isObject2Destroyed = true;
        }

        // �� ������Ʈ�� ��� ������ٸ� �� ������Ʈ ����
        if (isObject1Destroyed && isObject2Destroyed)
        {
            Instantiate(newObjectPrefab, spawnPosition, Quaternion.identity);

            // ������Ʈ�� ������ �Ŀ��� �� ��ũ��Ʈ�� �� �̻� �������� �ʵ��� �Ѵ�
            this.enabled = false;
        }
    }
}
