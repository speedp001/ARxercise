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
        // 시작 사운드 재생
        if (startSound != null)
        {
            audioSource.clip = startSound;
            audioSource.Play();
        }

        // 5초 후 버튼 전환
        StartCoroutine(SwitchButtonsAfterDelay());

        // Button Pressed 이벤트 연결
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
        // 클릭 소리를 먼저 재생하고 UI 전환을 조금 지연
        if (audioSource && clickSound)
        {
            audioSource.PlayOneShot(clickSound);
        }

        // UI 전환을 위한 코루틴 시작
        StartCoroutine(TransitionToCanvas2());
    }

    IEnumerator TransitionToCanvas2()
    {
        yield return new WaitForSeconds(1); // 오디오가 재생되는 동안 잠시 대기

        // 기존 Canvas1을 비활성화하고 Canvas2를 활성화
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
