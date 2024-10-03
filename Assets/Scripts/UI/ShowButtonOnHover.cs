using UnityEngine;
using UnityEngine.EventSystems;

public class ShowButtonsOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject[] buttons;  // ������Ʈ �ȿ� �ִ� ��ư���� �迭�� ����
    public GameObject[] appear;
    // ���콺�� ������Ʈ ���� �ö��� �� ��ư Ȱ��ȭ
    public void OnPointerEnter(PointerEventData eventData)
    {
        foreach (GameObject button in buttons)
        {
            button.SetActive(true);  // ��ư Ȱ��ȭ
        }
    }

    // ���콺�� ������Ʈ ������ ������ �� ��ư ��Ȱ��ȭ
    public void OnPointerExit(PointerEventData eventData)
    {
        foreach (GameObject button in buttons)
        {
            button.SetActive(false);  // ��ư ��Ȱ��ȭ
        }
    }
}
