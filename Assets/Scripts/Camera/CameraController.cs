using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // > �� ī�޶�
    // ī�޶� �ӵ�
    [SerializeField] private float speed;
    private float currentPosX;
    private Vector3 velocity = Vector3.zero;

    // �÷��̾� ī�޶�
    [SerializeField] private Transform _Player;
    [SerializeField] private float aheadDistance;
    [SerializeField] private float cameraSpeed;
    private float lookAhead;

    private void Update()
    {
        // SmoothDamp(Vector3 current, Vector3 target, ref Vector3 velocity, float smoothtime)
        // �ε巯�� �̵��� �ս��� �ϱ� ���ؼ� Ȱ��Ǵ� �Լ�
        //transform.position = Vector3.SmoothDamp(transform.position, new Vector3(currentPosX,
        //    transform.position.y, transform.position.z),ref velocity, speed);

        // �÷��̾�
        transform.position = new Vector3(_Player.position.x + lookAhead, transform.position.y,
            transform.position.z);

        lookAhead = Mathf.Lerp(lookAhead, (aheadDistance * _Player.localScale.x),
            cameraSpeed * Time.deltaTime);

    }

    // ���ο� ������ �̵��ϱ�
    public void MoveToNewRoom(Transform _newRoom)
    {
        currentPosX = _newRoom.position.x;
    }
}
