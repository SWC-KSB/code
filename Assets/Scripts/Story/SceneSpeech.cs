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

    [Header("프리팹 설정")]
    public GameObject prefabToSpawn;    // 생성할 프리팹
    public Transform spawnLocation;     // 프리팹을 생성할 위치

    [Header("할아버지 설정")]
    public Transform grandfatherNewPosition; // 대화가 끝난 후 할아버지가 이동할 위치
    public float moveSpeed = 2f;              // 할아버지 이동 속도
    private bool isWalking = false;            // 할아버지 걷는 상태 체크

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

            // 대화가 끝나면 프리팹 생성
            if (prefabToSpawn != null && spawnLocation != null)
            {
                Instantiate(prefabToSpawn, spawnLocation.position, spawnLocation.rotation);
            }

            // 할아버지 위치 변경 및 걸어가기
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
            // 할아버지의 현재 위치와 목표 위치 간의 거리를 계산
            float distance = Vector2.Distance(speech_granfa.transform.position, grandfatherNewPosition.position);

            // 목표 위치로 이동
            speech_granfa.transform.position = Vector2.MoveTowards(speech_granfa.transform.position, grandfatherNewPosition.position, moveSpeed * Time.deltaTime);

            // 할아버지가 목표 위치에 도달했을 때
            if (distance < 0.1f)
            {
                isWalking = false;  // 걷는 상태 종료
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
            AddSpeech(speech_player, "어라.. 여긴 어디지?");
            AddSpeech(speech_granfa, "무사해서 정말 다행이구나. 괜찮니?");
            AddSpeech(speech_granfa, "‘그 녀석’과의 마지막 전투에서 패배한 후, 쓰러져 있는 널 발견했단다.");
            AddSpeech(speech_granfa, "다친 곳은 치료했지만, 메모리 칩에 충격이 가서 기억을 잃은 것 같구나.");
            AddSpeech(speech_player, "할아버지는 누구세요?");
            AddSpeech(speech_granfa, "가상 세계가 지금 큰 위험에 빠져서 설명할 시간이 없구나.");
            AddSpeech(speech_granfa, "가상 세계로 가서 적들을 물리치고 메모리 칩을 회수해 오면 진실을 알려주마.");
            AddSpeech(speech_granfa, "가상 세계에 있는 로봇들에게 발각되면 ‘발각도’가 쌓이게 되고, 최대치에 도달하면 이쪽 세계로 튕겨 나오게 된다.");
            AddSpeech(speech_granfa, "포탈을 통해 가상 세계에 다녀오너라.");
        }
        else if (number == 2)
        {
            AddSpeech(speech_granfa, "수고 많았다. 이제 메모리 칩을 너에게 이식해주마.");
            AddSpeech(speech_granfa, "이제부터는 [더블 점프]가 가능하다.");
            AddSpeech(speech_granfa, "어서 포탈을 통해 다음 가상 세계를 구해다오.");
        }
        else if (number == 3)
        {
            AddSpeech(speech_granfa, "네가 요즘 우리들을 방해하는 그 녀석이구나.");
            AddSpeech(speech_granfa, "더 이상의 방해는 용서하지 않겠다.");
        }
        else if (number == 4)
        {
            AddSpeech(speech_granfa, "수고 많았다. 이번에도 메모리 칩을 이식해주마.");
            AddSpeech(speech_granfa, "이제부터 적의 발각 센서에 감지되지 않으니 발각도가 올라가지 않을 것이다.");
            AddSpeech(speech_granfa, "포탈을 통해 마지막 가상 세계도 구해다오.");
            AddSpeech(speech_granfa, "이번 임무만 끝내면 진실을 알려주마.");
        }
        else if (number == 5)
        {
            AddSpeech(speech_granfa, "네가 대기업의 CEO들을 암살하고 기밀을 훔쳐 간다는 그 녀석이구나.");
            AddSpeech(speech_player, "무슨 말씀이시죠?");
            AddSpeech(speech_granfa, "시치미 떼지 마라.");
            AddSpeech(speech_granfa, "여기서 널 막겠다.");
        }
        else if (number == 6)
        {
            AddSpeech(speech_granfa, "왜 우리 대기업 회장들만 노리는 거지?");
            AddSpeech(speech_player, "대기업 회장이라니, 그게 뭐죠?");
            AddSpeech(speech_player, "이 가상 세계에 그런 게 있나요?");
            AddSpeech(speech_player, "내 눈엔 당신이 가상 세계를 파괴하는 악당 로봇일 뿐입니다.");
            AddSpeech(speech_granfa, "가상 세계? 악당? 로봇?");
            AddSpeech(speech_granfa, "여긴 [현실 세계]다. 그리고 난 사람이야.");
            AddSpeech(speech_player, "하지만 수많은 로봇들과 당신의 그 몸은?");
            AddSpeech(speech_granfa, "하하, 누군가 네 인식 시스템에 손을 댄 것 같군.");
            AddSpeech(speech_granfa, "발각된 널 쫓아낸 건 우리 경호원들이고, 네가 죽인 건 대기업 회장들이었다.");
            AddSpeech(speech_granfa, "계속 기밀을 회수해 왔지 않았나.");
            AddSpeech(speech_granfa, "이제 끝났군...");
        }
        else if (number == 7)
        {
            AddSpeech(speech_granfa, "수고 많았네.");
            AddSpeech(speech_player, "닥쳐! 난 누구고, 당신은 대체 뭐지?");
            AddSpeech(speech_granfa, "또 눈치챘구나. [현실 세계]에 3번만 다녀오면 이러는군.");
            AddSpeech(speech_granfa, "이번이 벌써 7번째라네.");
            AddSpeech(speech_player, "그게 무슨 소리야!!!");
            AddSpeech(speech_granfa, "너는 [MEMORY RE:BOOT] 프로젝트로 개조된 인간이지.");
            AddSpeech(speech_granfa, "난 인간이 아니라 AI라서 [현실 세계]에 갈 수 없었지.");
            AddSpeech(speech_granfa, "그래서 너를 이용해 기업 회장들을 암살하고 기밀들을 빼돌렸단다.");
            AddSpeech(speech_granfa, "여기는 [가상 세계]고, 넌 포탈을 통해 [현실 세계]로 간 것이지.");
            AddSpeech(speech_granfa, "하지만 매번 3번만 다녀오면 넌 진실을 눈치채고 나에게 덤벼오더군.");
            AddSpeech(speech_granfa, "그래도 지금까지 7번이나 패배했고, 그때마다 너의 기억을 지웠지.");
            AddSpeech(speech_granfa, "이제 8번째 기억도 지울 시간이군.");
            AddSpeech(speech_player, "이번엔 다르다. 이번에야말로 이 무한한 굴레를 끝내겠다.");
            AddSpeech(speech_granfa, "그 대사도 이제 질렸군. 어서 덤벼라.");
        }
    }

    void Start()
    {
        playerMovement.enabled = false;
        SelectScene(scene);
    }
}
