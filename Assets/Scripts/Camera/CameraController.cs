using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // 카메라 속도
    [SerializeField] private float speed;
    private float currentPosX;
    private Vector3 velocity = Vector3.zero;

    // 플레이어 카메라
    [SerializeField] private Transform _Player;
    [SerializeField] private float aheadDistance;
    [SerializeField] private float cameraSpeed;
    private float lookAhead;

    // 카메라의 Y축 범위
    [SerializeField] private float yBoundary; // Y축 범위 (예: 10)
    private float originalY; // 카메라의 원래 Y축 위치

    private void Start()
    {
        // 카메라의 원래 Y축 위치 저장
        originalY = transform.position.y;
    }

    private void Update()
    {
        // 플레이어의 Y축 위치 체크
        if (_Player.position.y > originalY + yBoundary)
        {
            // 플레이어가 Y축 범위를 초과하면 카메라를 위로 이동
            transform.position = new Vector3(transform.position.x, originalY + (_Player.position.y - yBoundary), transform.position.z);
        }

        // 카메라 X축 움직임
        transform.position = new Vector3(_Player.position.x + lookAhead, transform.position.y, transform.position.z);

        lookAhead = Mathf.Lerp(lookAhead, (aheadDistance * _Player.localScale.x), cameraSpeed * Time.deltaTime);
    }

    // 새로운 방으로 이동하기
    public void MoveToNewRoom(Transform _newRoom)
    {
        currentPosX = _newRoom.position.x;
    }
}
