using UnityEngine;
using System.Collections;

public class BranchDescription : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip branchDescriptionSound; // Branch Description 음성
    public GameObject canvas3; // 현재 Canvas3
    public GameObject canvas4; // 전환될 Canvas4

    private void OnEnable()
    {
        // Canvas3가 활성화될 때 실행되는 코드
        PlayBranchDescriptionSound();
    }

    private void PlayBranchDescriptionSound()
    {
        if (audioSource != null && branchDescriptionSound != null)
        {
            audioSource.clip = branchDescriptionSound;
            audioSource.Play();

            // 음성 재생이 끝나면 다음 Canvas로 전환
            StartCoroutine(TransitionToCanvas4AfterSound());
        }
        else
        {
            Debug.LogWarning("AudioSource 또는 Branch Description 음성이 설정되지 않았습니다.");
        }
    }

    IEnumerator TransitionToCanvas4AfterSound()
    {
        // 음성이 끝날 때까지 대기
        yield return new WaitForSeconds(branchDescriptionSound.length);

        // 추가 대기 (1초)
        yield return new WaitForSeconds(2f);

        // Canvas3 비활성화하고 Canvas4 활성화
        if (canvas3 != null)
        {
            canvas3.SetActive(false);
        }

        if (canvas4 != null)
        {
            canvas4.SetActive(true);
        }
    }
}
