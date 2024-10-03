using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SceneSpeech : MonoBehaviour
{
    public Speech speech_granfa;
    public PlayerMovement playerMovement;
    public Speech speech_player;
    public int scene;
    private List<Tuple<Speech, string>> desa = new List<Tuple<Speech, string>>() { };
    private int SpeechNumber = 0;

    [Header("������ ����")]
    public GameObject prefabToSpawn;    // ������ ������
    public Transform spawnLocation;     // �������� ������ ��ġ

    [Header("�Ҿƹ��� ����")]
    public Transform grandfatherNewPosition; // ��ȭ�� ���� �� �Ҿƹ����� �̵��� ��ġ
    public float moveSpeed = 2f;              // �Ҿƹ��� �̵� �ӵ�
    private bool isWalking = false;            // �Ҿƹ��� �ȴ� ���� üũ

    // Start is called before the first frame update
    private void AddSpeech(Speech speech, string text)
    {
        desa.Add(Tuple.Create(speech, text));
    }

    public void PrintSpeech()
    {
        if (SpeechNumber > 0)
        {
            desa[SpeechNumber - 1].Item1.EndSpeech();
        }
        if (SpeechNumber != desa.Count)
        {
            desa[SpeechNumber].Item1.SpeechCaller(desa[SpeechNumber].Item2);
            SpeechNumber++;
        }
        else
        {
            playerMovement.enabled = true;

            // ��ȭ�� ������ ������ ����
            if (prefabToSpawn != null && spawnLocation != null)
            {
                Instantiate(prefabToSpawn, spawnLocation.position, spawnLocation.rotation);
            }

            // �Ҿƹ��� ��ġ ���� �� �ɾ��
            if (grandfatherNewPosition != null)
            {
                isWalking = true;
            }
        }
    }

    private void Update()
    {
        if (isWalking)
        {
            // �Ҿƹ����� ���� ��ġ�� ��ǥ ��ġ ���� �Ÿ��� ���
            float distance = Vector2.Distance(speech_granfa.transform.position, grandfatherNewPosition.position);

            // ��ǥ ��ġ�� �̵�
            speech_granfa.transform.position = Vector2.MoveTowards(speech_granfa.transform.position, grandfatherNewPosition.position, moveSpeed * Time.deltaTime);

            // �Ҿƹ����� ��ǥ ��ġ�� �������� ��
            if (distance < 0.1f)
            {
                isWalking = false;  // �ȴ� ���� ����
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            PrintSpeech();
        }
    }

    void SelectScene(int number)
    {
        if (number == 1)
        {
            AddSpeech(speech_player, "���.. ���� �����?");
            AddSpeech(speech_granfa, "�����ؼ� ���� �����̱���. ������?");
            AddSpeech(speech_granfa, "���� �༮������ ������ �������� �й��� ��, ������ �ִ� �� �߰��ߴܴ�.");
            AddSpeech(speech_granfa, "��ģ ���� ġ��������, �޸� Ĩ�� ����� ���� ����� ���� �� ������.");
            AddSpeech(speech_player, "�Ҿƹ����� ��������?");
            AddSpeech(speech_granfa, "���� ���谡 ���� ū ���迡 ������ ������ �ð��� ������.");
            AddSpeech(speech_granfa, "���� ����� ���� ������ ����ġ�� �޸� Ĩ�� ȸ���� ���� ������ �˷��ָ�.");
            AddSpeech(speech_granfa, "���� ���迡 �ִ� �κ��鿡�� �߰��Ǹ� ���߰������� ���̰� �ǰ�, �ִ�ġ�� �����ϸ� ���� ����� ƨ�� ������ �ȴ�.");
            AddSpeech(speech_granfa, "��Ż�� ���� ���� ���迡 �ٳ���ʶ�.");
        }
        else if (number == 2)
        {
            AddSpeech(speech_granfa, "���� ���Ҵ�. ���� �޸� Ĩ�� �ʿ��� �̽����ָ�.");
            AddSpeech(speech_granfa, "�������ʹ� [���� ����]�� �����ϴ�.");
            AddSpeech(speech_granfa, "� ��Ż�� ���� ���� ���� ���踦 ���شٿ�.");
        }
        else if (number == 3)
        {
            AddSpeech(speech_granfa, "�װ� ���� �츮���� �����ϴ� �� �༮�̱���.");
            AddSpeech(speech_granfa, "�� �̻��� ���ش� �뼭���� �ʰڴ�.");
        }
        else if (number == 4)
        {
            AddSpeech(speech_granfa, "���� ���Ҵ�. �̹����� �޸� Ĩ�� �̽����ָ�.");
            AddSpeech(speech_granfa, "�������� ���� �߰� ������ �������� ������ �߰����� �ö��� ���� ���̴�.");
            AddSpeech(speech_granfa, "��Ż�� ���� ������ ���� ���赵 ���شٿ�.");
            AddSpeech(speech_granfa, "�̹� �ӹ��� ������ ������ �˷��ָ�.");
        }
        else if (number == 5)
        {
            AddSpeech(speech_granfa, "�װ� ������ CEO���� �ϻ��ϰ� ����� ���� ���ٴ� �� �༮�̱���.");
            AddSpeech(speech_player, "���� �����̽���?");
            AddSpeech(speech_granfa, "��ġ�� ���� ����.");
            AddSpeech(speech_granfa, "���⼭ �� ���ڴ�.");
        }
        else if (number == 6)
        {
            AddSpeech(speech_granfa, "�� �츮 ���� ȸ��鸸 �븮�� ����?");
            AddSpeech(speech_player, "���� ȸ���̶��, �װ� ����?");
            AddSpeech(speech_player, "�� ���� ���迡 �׷� �� �ֳ���?");
            AddSpeech(speech_player, "�� ���� ����� ���� ���踦 �ı��ϴ� �Ǵ� �κ��� ���Դϴ�.");
            AddSpeech(speech_granfa, "���� ����? �Ǵ�? �κ�?");
            AddSpeech(speech_granfa, "���� [���� ����]��. �׸��� �� ����̾�.");
            AddSpeech(speech_player, "������ ������ �κ���� ����� �� ����?");
            AddSpeech(speech_granfa, "����, ������ �� �ν� �ý��ۿ� ���� �� �� ����.");
            AddSpeech(speech_granfa, "�߰��� �� �ѾƳ� �� �츮 ��ȣ�����̰�, �װ� ���� �� ���� ȸ����̾���.");
            AddSpeech(speech_granfa, "��� ����� ȸ���� ���� �ʾҳ�.");
            AddSpeech(speech_granfa, "���� ������...");
        }
        else if (number == 7)
        {
            AddSpeech(speech_granfa, "���� ���ҳ�.");
            AddSpeech(speech_player, "����! �� ������, ����� ��ü ����?");
            AddSpeech(speech_granfa, "�� ��ġë����. [���� ����]�� 3���� �ٳ���� �̷��±�.");
            AddSpeech(speech_granfa, "�̹��� ���� 7��°���.");
            AddSpeech(speech_player, "�װ� ���� �Ҹ���!!!");
            AddSpeech(speech_granfa, "�ʴ� [MEMORY RE:BOOT] ������Ʈ�� ������ �ΰ�����.");
            AddSpeech(speech_granfa, "�� �ΰ��� �ƴ϶� AI�� [���� ����]�� �� �� ������.");
            AddSpeech(speech_granfa, "�׷��� �ʸ� �̿��� ��� ȸ����� �ϻ��ϰ� ��е��� �����ȴܴ�.");
            AddSpeech(speech_granfa, "����� [���� ����]��, �� ��Ż�� ���� [���� ����]�� �� ������.");
            AddSpeech(speech_granfa, "������ �Ź� 3���� �ٳ���� �� ������ ��ġä�� ������ ����������.");
            AddSpeech(speech_granfa, "�׷��� ���ݱ��� 7���̳� �й��߰�, �׶����� ���� ����� ������.");
            AddSpeech(speech_granfa, "���� 8��° ��ﵵ ���� �ð��̱�.");
            AddSpeech(speech_player, "�̹��� �ٸ���. �̹����߸��� �� ������ ������ �����ڴ�.");
            AddSpeech(speech_granfa, "�� ��絵 ���� ���ȱ�. � ������.");
        }
    }

    void Start()
    {
        playerMovement.enabled = false;
        SelectScene(scene);
    }
}
