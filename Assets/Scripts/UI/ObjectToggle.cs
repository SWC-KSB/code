using UnityEngine;

public class ObjectToggle : MonoBehaviour
{
    public GameObject targetObject; // 비활성화할 이미지 오브젝트
    public ShowMenuOnEsc showMenuOnEsc; 
    public void OnButtonClick()
    {
        // 대상 오브젝트가 null이 아니면 비활성화
        if (targetObject != null)
        {
            targetObject.SetActive(false); // 오브젝트를 비활성화
            Time.timeScale = 1;  // 게임 시간 재개 // 마우스 커서 다시 잠금
            Cursor.visible = false;  // 마우스 커서 숨김
            showMenuOnEsc.setMenuActive(false);
        }
        else
        {
            Debug.LogWarning("targetObject가 할당되지 않았습니다."); // null 체크
        }
    }
}
