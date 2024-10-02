using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Speech : MonoBehaviour
{
    public Transform targetObject;   // 따라갈 오브젝트 (타겟)
    public Vector3 offset;           // 상대적인 위치 (오프셋)
    public Vector3 initoffset;
    public TextMeshProUGUI tmpText;  // TMP 컴포넌트
    public Image SpeechBubble;
    void Start()
    {
        initoffset = offset;
        SpeechBubble.gameObject.SetActive(false);
     }

    void Update()
    {
        if (targetObject != null)
        {
            // 타겟 오브젝트의 위치에 상대적인 오프셋 추가
            Vector3 targetPosition = targetObject.position + offset;

            // TMP 오브젝트를 따라갈 오브젝트의 위치로 이동
            SpeechBubble.transform.position = targetPosition;
        }
    }
    public void SpeechCaller(string text)
    {
        tmpText.text = text;
        int line = text.Split("\n").Length;
        Debug.Log(text);
        tmpText.ForceMeshUpdate();
        Vector2 size = tmpText.GetPreferredValues();
        tmpText.rectTransform.sizeDelta = size;
        SpeechBubble.rectTransform.sizeDelta = new Vector2(size.x*1.5f,size.y+line*30);
        Vector3 bubbleoffset = new Vector3(SpeechBubble.rectTransform.sizeDelta.x / 100 * tmpText.rectTransform.localScale.x * -1, SpeechBubble.rectTransform.sizeDelta.y / 50);
        offset = bubbleoffset;
        SpeechBubble.gameObject.SetActive(true);
    }
    public void EndSpeech()
    {
        SpeechBubble.gameObject.SetActive(false);
    }
}
