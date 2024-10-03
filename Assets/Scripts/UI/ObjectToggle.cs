using UnityEngine;

public class ObjectToggle : MonoBehaviour
{
    public GameObject targetObject; // ��Ȱ��ȭ�� �̹��� ������Ʈ
    public ShowMenuOnEsc showMenuOnEsc; 
    public void OnButtonClick()
    {
        // ��� ������Ʈ�� null�� �ƴϸ� ��Ȱ��ȭ
        if (targetObject != null)
        {
            targetObject.SetActive(false); // ������Ʈ�� ��Ȱ��ȭ
            Time.timeScale = 1;  // ���� �ð� �簳 // ���콺 Ŀ�� �ٽ� ���
            Cursor.visible = false;  // ���콺 Ŀ�� ����
            showMenuOnEsc.setMenuActive(false);
        }
        else
        {
            Debug.LogWarning("targetObject�� �Ҵ���� �ʾҽ��ϴ�."); // null üũ
        }
    }
}
