using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // > 룸 카메라
    // 카메라 속도
    [SerializeField] private float speed;
    private float currentPosX;
    private Vector3 velocity = Vector3.zero;

    // 플레이어 카메라
    [SerializeField] private Transform _Player;
    [SerializeField] private float aheadDistance;
    [SerializeField] private float cameraSpeed;
    private float lookAhead;

    private void Update()
    {
        // SmoothDamp(Vector3 current, Vector3 target, ref Vector3 velocity, float smoothtime)
        // 부드러운 이동을 손쉽게 하기 위해서 활용되는 함수
        //transform.position = Vector3.SmoothDamp(transform.position, new Vector3(currentPosX,
        //    transform.position.y, transform.position.z),ref velocity, speed);

        // 플레이어
        transform.position = new Vector3(_Player.position.x + lookAhead, transform.position.y,
            transform.position.z);

        lookAhead = Mathf.Lerp(lookAhead, (aheadDistance * _Player.localScale.x),
            cameraSpeed * Time.deltaTime);

    }

    // 새로운 방으로 이동하기
    public void MoveToNewRoom(Transform _newRoom)
    {
        currentPosX = _newRoom.position.x;
    }
}
