using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartButtonController : MonoBehaviour
{
    public Button startButton;
    public Button newStartButton;
    public AudioSource audioSource;
    public AudioClip startSound;
    public AudioClip clickSound;
    public GameObject canvas1; 
    public GameObject canvas2; 

    private float delayTime = 5f;

    void Start()
    {
        // ���� ���� ���
        if (startSound != null)
        {
            audioSource.clip = startSound;
            audioSource.Play();
        }

        // 5�� �� ��ư ��ȯ
        StartCoroutine(SwitchButtonsAfterDelay());

        // new_start_button�� Ŭ�� �̺�Ʈ ���
        newStartButton.onClick.AddListener(OnNewStartButtonClick);
    }

    IEnumerator SwitchButtonsAfterDelay()
    {
        yield return new WaitForSeconds(delayTime);
        startButton.gameObject.SetActive(false);
        newStartButton.gameObject.SetActive(true);
    }

    public void OnNewStartButtonClick()
    {
        // Ŭ�� �Ҹ��� ���� ����ϰ� UI ��ȯ�� ���� ����
        if (audioSource && clickSound)
        {
            audioSource.PlayOneShot(clickSound);
        }

        // UI ��ȯ�� ���� �ڷ�ƾ ����
        StartCoroutine(TransitionToCanvas2());
    }

    IEnumerator TransitionToCanvas2()
    {
        yield return new WaitForSeconds(1); // ������� ����Ǵ� ���� ��� ���

        // ���� Canvas1�� ��Ȱ��ȭ�ϰ� Canvas2�� Ȱ��ȭ
        if (canvas1 != null)
        {
            canvas1.SetActive(false);
        }

        if (canvas2 != null)
        {
            canvas2.SetActive(true);
        }
    }
}
