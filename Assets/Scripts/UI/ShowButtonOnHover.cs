using UnityEngine;
using UnityEngine.EventSystems;

public class ShowButtonsOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject[] buttons;  // 오브젝트 안에 있는 버튼들을 배열로 설정
    public GameObject[] appear;
    // 마우스가 오브젝트 위로 올라갔을 때 버튼 활성화
    public void OnPointerEnter(PointerEventData eventData)
    {
        foreach (GameObject button in buttons)
        {
            button.SetActive(true);  // 버튼 활성화
        }
    }

    // 마우스가 오브젝트 밖으로 나갔을 때 버튼 비활성화
    public void OnPointerExit(PointerEventData eventData)
    {
        foreach (GameObject button in buttons)
        {
            button.SetActive(false);  // 버튼 비활성화
        }
    }
}
