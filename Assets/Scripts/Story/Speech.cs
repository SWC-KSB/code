using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Speech : MonoBehaviour
{
    public Transform targetObject;   // ���� ������Ʈ (Ÿ��)
    public Vector3 offset;           // ������� ��ġ (������)
    public Vector3 initoffset;
    public TextMeshProUGUI tmpText;  // TMP ������Ʈ
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
            // Ÿ�� ������Ʈ�� ��ġ�� ������� ������ �߰�
            Vector3 targetPosition = targetObject.position + offset;

            // TMP ������Ʈ�� ���� ������Ʈ�� ��ġ�� �̵�
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
