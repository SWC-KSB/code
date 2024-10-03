using UnityEngine;

public class ShowMenuOnEsc : MonoBehaviour
{
    public GameObject image;  // ������ �̹����� ��ư�� ��� �ִ� �г�
    private bool isMenuActive = false;  // �޴� ���¸� �����ϴ� ����

    void Start()
    {
        // ���� ���� �� �޴� �г��� ��Ȱ��ȭ
        image.SetActive(false);
        Cursor.visible = false;  // ���콺 Ŀ�� ����
    }

    void Update()
    {
        // ESC Ű�� ������ �� �޴� Ȱ��ȭ/��Ȱ��ȭ
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isMenuActive = !isMenuActive;
            image.SetActive(isMenuActive);

            if (isMenuActive)
            {
                // �޴��� Ȱ��ȭ�Ǹ� ���� �ð� ���� �� ���콺 Ȱ��ȭ
                Time.timeScale = 0;  // ���� �ð� ����  // ���콺 Ŀ�� ��� ����
                Cursor.visible = true;  // ���콺 Ŀ�� ǥ��
            }
            else
            {
                // �޴��� ��Ȱ��ȭ�Ǹ� ���� �ð� �簳 �� ���콺 ���
                Time.timeScale = 1;  // ���� �ð� �簳 // ���콺 Ŀ�� �ٽ� ���
                Cursor.visible = false;  // ���콺 Ŀ�� ����
            }
        }

    }
    public void setMenuActive (bool Active)
    {
        isMenuActive = Active;
    }
}

