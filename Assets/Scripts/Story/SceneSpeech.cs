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
            AddSpeech(speech_player, "어라.. 여긴\n어디지..?");
            AddSpeech(speech_granfa, "무사해서 정말 다행이구나 얘야...\n괜찮니?");
            AddSpeech(speech_granfa, "“그 녀석”과의 마지막 전투에서 패배한 후,\n쓰러져있는 널 발견했단다.");
            AddSpeech(speech_granfa, "다친 곳은 다 치료했지만...\n메모리 칩에 충격이 가서 기억이 날아간 것 같구나.");
            AddSpeech(speech_player, "할아버지는 누구시죠..?");
            AddSpeech(speech_granfa, "가상 세계가 지금 아주 큰 위험에 빠져서\n차근차근 설명해줄 시간이 없구나..");
            AddSpeech(speech_granfa, "가상 세계로 가서 적들을 물리치고\n메모리 칩을 회수해 오면 진실을 알려주마.");
            AddSpeech(speech_granfa, "가상 세계에 있는 로봇들에게 너무 오래 발각을 당해\n“발각도”가 최대가 되면 다시 이쪽 세계로 튕겨져 나온단다.");
            AddSpeech(speech_granfa, "일단 포탈을 통해\n가상세계에 다녀오려무나.");

        }else if(number == 2)
        {
            AddSpeech(speech_granfa, "수고 많았구나 얘야.\n이제 얻어온 메모리 칩을 너에게 이식해주마.");
            AddSpeech(speech_granfa, "이제부터 [더블점프]가 가능하단다.");
            AddSpeech(speech_granfa, "어서 포탈을 통해 넘어가\n다음 가상 세계도 구해다오.");
        }else if (number == 3)
        {
            AddSpeech(speech_granfa, "네 녀석이 요즘 [우리]들을\n방해하는 녀석인가?");
            AddSpeech(speech_granfa, "더 이상의 방해는\n용서치 않겠다.");
        }else if (number == 4)
        {
            AddSpeech(speech_granfa, "수고 많았다.\n이번에도 메모리 칩을 이식해주마.");
            AddSpeech(speech_granfa, "이제부터 적의 발각 센서에 감지되지 않기 때문에\n발각도가 올라가지 않는단다.");
            AddSpeech(speech_granfa, "어서 포탈을 통해 넘어가\n마지막 가상 세계도 구해다오.");
            AddSpeech(speech_granfa, "이번 임무만 무사히 끝난다면..\n너에게 [진실]을 알려주마.");
        }else if (number == 5)
        {
            AddSpeech(speech_granfa, "네 녀석이 요즘 대기업의 CEO들을 암살하고 기업의 기밀을 훔쳐 간다던 녀석이구나.");
            AddSpeech(speech_player, "그게 무슨 소리지?");
            AddSpeech(speech_granfa, "시치미 떼지 마라.");
            AddSpeech(speech_granfa, "그리고 네 녀석은 내가 여기서 막겠다.");
        }else if (number == 6)
        {
            AddSpeech(speech_granfa, "어째서 우리 대기업 회장들만\n노리는 거지?");
            AddSpeech(speech_player, "대기업 회장이라니.");
            AddSpeech(speech_player, "이 가상 세계에도 그런 게 있나?");
            AddSpeech(speech_player, "내 눈에 네 놈은 그저 가상 세계를\n파괴하는 악당 로봇일 뿐이다.");
            AddSpeech(speech_granfa, "가상세계…\n악당…\n로봇…?");
            AddSpeech(speech_granfa, "그게 무슨 소리지?\n여긴 [현실 세계]다.");
            AddSpeech(speech_granfa, "그리고 나 또한 로봇이 아닌\n사람이지 않나.");
            AddSpeech(speech_player, "하지만 수많던 로봇들과…\n네놈의 그 사람이라고 볼 수 없는\n 몸뚱아리는 뭐지?");
            AddSpeech(speech_granfa, "크큭…\n아무래도 누군가\n네 놈의 인식 시스템에\n손을 댄 것 같군.");
            AddSpeech(speech_granfa, "발각된 네놈을 밖으로\n내쫓아왔던 건 우리 경호원들이고,");
            AddSpeech(speech_granfa, "그 경호원들을 뚫고 마지막에\n네놈이 죽인 건 대기업 회장들이다.");
            AddSpeech(speech_granfa, "그리고 계속해서 기업의 기밀들을\n회수해갔지 않았나?");
            AddSpeech(speech_granfa, "난 아무래도 여기 까진 것 같군…");

        }else if (number == 7)
        {
            AddSpeech(speech_granfa, "수고 많ㅇ…");
            AddSpeech(speech_player, "닥쳐..\n 난 누구고, 당신은 대체 뭐지?");
            AddSpeech(speech_granfa, "또 눈치채버렸나…");
            AddSpeech(speech_granfa, "항상 [현실 세계]로 3번만\n다녀오면 이런다니까.");
            AddSpeech(speech_granfa, "이번으로 벌써 7번째구나.");
            AddSpeech(speech_player, "그게 무슨 소리냐고!!!");
            AddSpeech(speech_granfa, "네 녀석은 [MEMORY RE:BOOT] 프로젝트로\n 개조된 인간이지.");
            AddSpeech(speech_granfa, "난 인간이 아니라 프로젝트를 위해 만들어진\n AI이기 때문에 [현실 세계]에 갈 수 없었다.");
            AddSpeech(speech_granfa, "그래서 너를 이용해 그동안\n수많은 기업의 회장들을 암살하고\n기밀들을 빼돌려 왔단다.");
            AddSpeech(speech_granfa, "여긴 [가상 세계]고, 너는 포탈을 통해\n[현실 세계]로 넘어갔던 것이지.");
            AddSpeech(speech_granfa, "하지만 항상 [현실 세계]로 3번만 다녀오면\n 넌 늘 [진실]을 눈치채고 나에게 덤벼왔단다.");
            AddSpeech(speech_granfa, "그럼에도 지금까지 7번이나 나에게 패배했고,\n패배할 때마다 너의 기억을 지웠지.");
            AddSpeech(speech_granfa, "자 8번째 기억도\n지울 차례다.");
            AddSpeech(speech_player, "절대 용서할 수 없어…");
            AddSpeech(speech_player, "이 무한의 굴레를\n여기에서 끊겠어.");
            AddSpeech(speech_granfa, "그 대사도 질리도록 들었으니\n어서 덤벼라 애송이.");
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