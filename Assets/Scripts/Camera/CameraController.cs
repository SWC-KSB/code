using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // ī�޶� �ӵ�
    [SerializeField] private float speed;
    private float currentPosX;
    private Vector3 velocity = Vector3.zero;

    // �÷��̾� ī�޶�
    [SerializeField] private Transform _Player;
    [SerializeField] private float aheadDistance;
    [SerializeField] private float cameraSpeed;
    private float lookAhead;

    // ī�޶��� Y�� ����
    [SerializeField] private float yBoundary; // Y�� ���� (��: 10)
    private float originalY; // ī�޶��� ���� Y�� ��ġ

    private void Start()
    {
        // ī�޶��� ���� Y�� ��ġ ����
        originalY = transform.position.y;
    }

    private void Update()
    {
        // �÷��̾��� Y�� ��ġ üũ
        if (_Player.position.y > originalY + yBoundary)
        {
            // �÷��̾ Y�� ������ �ʰ��ϸ� ī�޶� ���� �̵�
            transform.position = new Vector3(transform.position.x, originalY + (_Player.position.y - yBoundary), transform.position.z);
        }

        // ī�޶� X�� ������
        transform.position = new Vector3(_Player.position.x + lookAhead, transform.position.y, transform.position.z);

        lookAhead = Mathf.Lerp(lookAhead, (aheadDistance * _Player.localScale.x), cameraSpeed * Time.deltaTime);
    }

    // ���ο� ������ �̵��ϱ�
    public void MoveToNewRoom(Transform _newRoom)
    {
        currentPosX = _newRoom.position.x;
    }
}
