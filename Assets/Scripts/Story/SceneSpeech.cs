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
    private List<Tuple<Speech,string>> desa =new List<Tuple<Speech, string>>() { };
    private int SpeechNumber=0;
    // Start is called before the first frame update
    private void AddSpeech(Speech speech,string text)
    {
        desa.Add(Tuple.Create(speech, text));
    }
    public void PrintSpeech()
    {
        if (SpeechNumber>0)
        {
            desa[SpeechNumber - 1].Item1.EndSpeech();
        }
        if(SpeechNumber != desa.Count )
        {

            desa[SpeechNumber].Item1.SpeechCaller(desa[SpeechNumber].Item2);
            SpeechNumber++;
            Debug.Log("??");
        }
        else
        {
            playerMovement.enabled=true;
        }
            
    }
    void SelectScene(int number)
    {
        if (number == 1)
        {
            AddSpeech(speech_player, "���.. ����\n�����..?");
            AddSpeech(speech_granfa, "�����ؼ� ���� �����̱��� ���...\n������?");
            AddSpeech(speech_granfa, "���� �༮������ ������ �������� �й��� ��,\n�������ִ� �� �߰��ߴܴ�.");
            AddSpeech(speech_granfa, "��ģ ���� �� ġ��������...\n�޸� Ĩ�� ����� ���� ����� ���ư� �� ������.");
            AddSpeech(speech_player, "�Ҿƹ����� ��������..?");
            AddSpeech(speech_granfa, "���� ���谡 ���� ���� ū ���迡 ������\n�������� �������� �ð��� ������..");
            AddSpeech(speech_granfa, "���� ����� ���� ������ ����ġ��\n�޸� Ĩ�� ȸ���� ���� ������ �˷��ָ�.");
            AddSpeech(speech_granfa, "���� ���迡 �ִ� �κ��鿡�� �ʹ� ���� �߰��� ����\n���߰������� �ִ밡 �Ǹ� �ٽ� ���� ����� ƨ���� ���´ܴ�.");
            AddSpeech(speech_granfa, "�ϴ� ��Ż�� ����\n���󼼰迡 �ٳ��������.");

        }else if(number == 2)
        {
            AddSpeech(speech_granfa, "���� ���ұ��� ���.\n���� ���� �޸� Ĩ�� �ʿ��� �̽����ָ�.");
            AddSpeech(speech_granfa, "�������� [��������]�� �����ϴܴ�.");
            AddSpeech(speech_granfa, "� ��Ż�� ���� �Ѿ\n���� ���� ���赵 ���شٿ�.");
        }else if (number == 3)
        {
            AddSpeech(speech_granfa, "�� �༮�� ���� [�츮]����\n�����ϴ� �༮�ΰ�?");
            AddSpeech(speech_granfa, "�� �̻��� ���ش�\n�뼭ġ �ʰڴ�.");
        }else if (number == 4)
        {
            AddSpeech(speech_granfa, "���� ���Ҵ�.\n�̹����� �޸� Ĩ�� �̽����ָ�.");
            AddSpeech(speech_granfa, "�������� ���� �߰� ������ �������� �ʱ� ������\n�߰����� �ö��� �ʴ´ܴ�.");
            AddSpeech(speech_granfa, "� ��Ż�� ���� �Ѿ\n������ ���� ���赵 ���شٿ�.");
            AddSpeech(speech_granfa, "�̹� �ӹ��� ������ �����ٸ�..\n�ʿ��� [����]�� �˷��ָ�.");
        }else if (number == 5)
        {
            AddSpeech(speech_granfa, "�� �༮�� ���� ������ CEO���� �ϻ��ϰ� ����� ����� ���� ���ٴ� �༮�̱���.");
            AddSpeech(speech_player, "�װ� ���� �Ҹ���?");
            AddSpeech(speech_granfa, "��ġ�� ���� ����.");
            AddSpeech(speech_granfa, "�׸��� �� �༮�� ���� ���⼭ ���ڴ�.");
        }else if (number == 6)
        {
            AddSpeech(speech_granfa, "��°�� �츮 ���� ȸ��鸸\n�븮�� ����?");
            AddSpeech(speech_player, "���� ȸ���̶��.");
            AddSpeech(speech_player, "�� ���� ���迡�� �׷� �� �ֳ�?");
            AddSpeech(speech_player, "�� ���� �� ���� ���� ���� ���踦\n�ı��ϴ� �Ǵ� �κ��� ���̴�.");
            AddSpeech(speech_granfa, "���󼼰衦\n�Ǵ硦\n�κ���?");
            AddSpeech(speech_granfa, "�װ� ���� �Ҹ���?\n���� [���� ����]��.");
            AddSpeech(speech_granfa, "�׸��� �� ���� �κ��� �ƴ�\n������� �ʳ�.");
            AddSpeech(speech_player, "������ ������ �κ������\n�׳��� �� ����̶�� �� �� ����\n ���׾Ƹ��� ����?");
            AddSpeech(speech_granfa, "ũŪ��\n�ƹ����� ������\n�� ���� �ν� �ý��ۿ�\n���� �� �� ����.");
            AddSpeech(speech_granfa, "�߰��� �׳��� ������\n���ѾƿԴ� �� �츮 ��ȣ�����̰�,");
            AddSpeech(speech_granfa, "�� ��ȣ������ �հ� ��������\n�׳��� ���� �� ���� ȸ����̴�.");
            AddSpeech(speech_granfa, "�׸��� ����ؼ� ����� ��е���\nȸ���ذ��� �ʾҳ�?");
            AddSpeech(speech_granfa, "�� �ƹ����� ���� ���� �� ������");

        }else if (number == 7)
        {
            AddSpeech(speech_granfa, "���� ������");
            AddSpeech(speech_player, "����..\n �� ������, ����� ��ü ����?");
            AddSpeech(speech_granfa, "�� ��ġä���ȳ���");
            AddSpeech(speech_granfa, "�׻� [���� ����]�� 3����\n�ٳ���� �̷��ٴϱ�.");
            AddSpeech(speech_granfa, "�̹����� ���� 7��°����.");
            AddSpeech(speech_player, "�װ� ���� �Ҹ��İ�!!!");
            AddSpeech(speech_granfa, "�� �༮�� [MEMORY RE:BOOT] ������Ʈ��\n ������ �ΰ�����.");
            AddSpeech(speech_granfa, "�� �ΰ��� �ƴ϶� ������Ʈ�� ���� �������\n AI�̱� ������ [���� ����]�� �� �� ������.");
            AddSpeech(speech_granfa, "�׷��� �ʸ� �̿��� �׵���\n������ ����� ȸ����� �ϻ��ϰ�\n��е��� ������ �Դܴ�.");
            AddSpeech(speech_granfa, "���� [���� ����]��, �ʴ� ��Ż�� ����\n[���� ����]�� �Ѿ�� ������.");
            AddSpeech(speech_granfa, "������ �׻� [���� ����]�� 3���� �ٳ����\n �� �� [����]�� ��ġä�� ������ �����Դܴ�.");
            AddSpeech(speech_granfa, "�׷����� ���ݱ��� 7���̳� ������ �й��߰�,\n�й��� ������ ���� ����� ������.");
            AddSpeech(speech_granfa, "�� 8��° ��ﵵ\n���� ���ʴ�.");
            AddSpeech(speech_player, "���� �뼭�� �� ���");
            AddSpeech(speech_player, "�� ������ ������\n���⿡�� ���ھ�.");
            AddSpeech(speech_granfa, "�� ��絵 �������� �������\n� ������ �ּ���.");
        }
    }
    void Start()
    {
        playerMovement.enabled = false;
        SelectScene(scene);
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            PrintSpeech();
        }
   
    }
}