using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NextButtonController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip exerciseDescriptionSound; // Exercise Description_1 음성
    public AudioClip clickSound; // 클릭 소리
    public Button explanationButton;
    public GameObject canvas2; // 현재 Canvas2
    public GameObject canvas3; // 전환될 Canvas3

    private void OnEnable()
    {
        // Canvas2가 활성화될 때 실행되는 코드
        PlayExerciseDescriptionSound();

        // 버튼 클릭 시 다음 Canvas로 전환하는 이벤트 등록
        explanationButton.onClick.AddListener(OnExplanationButtonClick);

        // 버튼을 5초 동안 비활성화 후 다시 활성화
        explanationButton.interactable = false; // 버튼 비활성화
        StartCoroutine(EnableButtonAfterDelay(15f)); // 15초 후 버튼 활성화
    }

    private void OnDisable()
    {
        // Canvas2가 비활성화될 때 이벤트 해제
        explanationButton.onClick.RemoveListener(OnExplanationButtonClick);
    }

    private void PlayExerciseDescriptionSound()
    {
        if (audioSource != null && exerciseDescriptionSound != null)
        {
            audioSource.clip = exerciseDescriptionSound;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioSource 또는 Exercise Description 음성이 설정되지 않았습니다.");
        }
    }

    private void OnExplanationButtonClick()
    {
        // 클릭 소리 재생
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }

        // Canvas2 비활성화하고 Canvas3 활성화
        StartCoroutine(TransitionToCanvas3());
    }

    IEnumerator TransitionToCanvas3()
    {
        // 전환 전에 잠시 대기 (필요 시)
        yield return new WaitForSeconds(0.5f);

        if (canvas2 != null)
        {
            canvas2.SetActive(false);
        }

        if (canvas3 != null)
        {
            canvas3.SetActive(true);
        }
    }

    IEnumerator EnableButtonAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        explanationButton.interactable = true; // 버튼 활성화
    }
}
