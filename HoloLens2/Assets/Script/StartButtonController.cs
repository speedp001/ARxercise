using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Microsoft.MixedReality.Toolkit.UI;

public class StartButtonController : MonoBehaviour
{
    public Button startButton;
    public PressableButton newStartButton;
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

        // Button Pressed �̺�Ʈ ����
        newStartButton.ButtonPressed.AddListener(StartButtonPressed);
    }

    IEnumerator SwitchButtonsAfterDelay()
    {
        yield return new WaitForSeconds(delayTime);
        startButton.gameObject.SetActive(false);
        newStartButton.gameObject.SetActive(true);
    }

    public void StartButtonPressed()
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
