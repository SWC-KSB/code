using UnityEngine;

public class ShowMenuOnEsc : MonoBehaviour
{
    public GameObject image;  // 반투명 이미지와 버튼을 담고 있는 패널
    private bool isMenuActive = false;  // 메뉴 상태를 저장하는 변수

    void Start()
    {
        // 게임 시작 시 메뉴 패널을 비활성화
        image.SetActive(false);
        Cursor.visible = false;  // 마우스 커서 숨김
    }

    void Update()
    {
        // ESC 키를 눌렀을 때 메뉴 활성화/비활성화
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isMenuActive = !isMenuActive;
            image.SetActive(isMenuActive);

            if (isMenuActive)
            {
                // 메뉴가 활성화되면 게임 시간 정지 및 마우스 활성화
                Time.timeScale = 0;  // 게임 시간 멈춤  // 마우스 커서 잠금 해제
                Cursor.visible = true;  // 마우스 커서 표시
            }
            else
            {
                // 메뉴가 비활성화되면 게임 시간 재개 및 마우스 잠금
                Time.timeScale = 1;  // 게임 시간 재개 // 마우스 커서 다시 잠금
                Cursor.visible = false;  // 마우스 커서 숨김
            }
        }

    }
    public void setMenuActive (bool Active)
    {
        isMenuActive = Active;
    }
}

