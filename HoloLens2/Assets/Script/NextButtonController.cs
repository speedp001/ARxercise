using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Microsoft.MixedReality.Toolkit.UI;

public class NextButtonController : MonoBehaviour
{
    public Button nextButton;
    public PressableButton newNextButton;
    public AudioSource audioSource;
    public AudioClip exerciseDescriptionSound;
    public AudioClip clickSound;
    public GameObject canvas2;
    public GameObject canvas3;

    private float delayTime = 15f;

    private void OnEnable()
    {

        // 설명 음성 재생
        if (audioSource != null && exerciseDescriptionSound != null)
        {
            audioSource.clip = exerciseDescriptionSound;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioSource 또는 ExerciseDescriptionSound가 설정되지 않았습니다.");
        }

        // 15초 후 버튼 전환
        StartCoroutine(SwitchButtonsAfterDelay());

        // PressableButton의 ButtonPressed 이벤트 연결
        if (newNextButton != null)
        {
            newNextButton.ButtonPressed.AddListener(NextButtonPressed);
        }
        else
        {
            Debug.LogError("newNextButton이 설정되지 않았습니다.");
        }
    }

    IEnumerator SwitchButtonsAfterDelay()
    {
        yield return new WaitForSeconds(delayTime);

        if (nextButton != null)
        {
            nextButton.gameObject.SetActive(false);
        }
        if (newNextButton != null)
        {
            newNextButton.gameObject.SetActive(true);
        }
    }

    public void NextButtonPressed()
    {
        // 클릭 소리 재생
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
        else
        {
            Debug.LogWarning("AudioSource 또는 ClickSound가 설정되지 않았습니다.");
        }

        // Canvas 전환
        StartCoroutine(TransitionToCanvas3());
    }

    IEnumerator TransitionToCanvas3()
    {
        yield return new WaitForSeconds(1);

        if (canvas2 != null)
        {
            canvas2.SetActive(false);
        }

        if (canvas3 != null)
        {
            canvas3.SetActive(true);
        }
    }
}
