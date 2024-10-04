using UnityEngine;

public class Yab : MonoBehaviour
{
    // 사라질 오브젝트
    public GameObject object1;
    public GameObject object2;

    // 새로 생성될 오브젝트
    public GameObject newObjectPrefab;

    // 새 오브젝트를 생성할 위치
    public Vector3 spawnPosition;

    // 두 오브젝트가 모두 사라졌는지 체크하는 변수
    private bool isObject1Destroyed = false;
    private bool isObject2Destroyed = false;

    void Update()
    {
        // 첫 번째 오브젝트가 사라졌는지 확인
        if (object1 == null && !isObject1Destroyed)
        {
            isObject1Destroyed = true;
        }

        // 두 번째 오브젝트가 사라졌는지 확인
        if (object2 == null && !isObject2Destroyed)
        {
            isObject2Destroyed = true;
        }

        // 두 오브젝트가 모두 사라졌다면 새 오브젝트 생성
        if (isObject1Destroyed && isObject2Destroyed)
        {
            Instantiate(newObjectPrefab, spawnPosition, Quaternion.identity);

            // 오브젝트가 생성된 후에는 이 스크립트를 더 이상 실행하지 않도록 한다
            this.enabled = false;
        }
    }
}
