using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BirdDescription : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip birdDescriptionSound; // Bird Description 음성
    public GameObject canvas4; // 현재 Canvas4

    private void OnEnable()
    {
        // Canvas4가 활성화될 때 실행되는 코드
        PlayBirdDescriptionSound();
    }

    private void PlayBirdDescriptionSound()
    {
        if (audioSource != null && birdDescriptionSound != null)
        {
            audioSource.clip = birdDescriptionSound;
            audioSource.Play();

            // 음성 재생이 끝나면 BirdGame 장면으로 전환
            StartCoroutine(TransitionToBirdGameAfterSound());
        }
        else
        {
            Debug.LogWarning("AudioSource 또는 Bird Description 음성이 설정되지 않았습니다.");
        }
    }

    IEnumerator TransitionToBirdGameAfterSound()
    {
        // 음성이 끝날 때까지 대기
        yield return new WaitForSeconds(birdDescriptionSound.length);

        // 추가 대기 (1초)
        yield return new WaitForSeconds(2f);

        // BirdGame 장면으로 전환
        yield return SceneManager.LoadSceneAsync("BirdGame");

        // BirdGame 장면이 로드된 후 카메라 설정 변경
        SetCameraClearFlagsToSkybox();
    }

    private void SetCameraClearFlagsToSkybox()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            mainCamera.clearFlags = CameraClearFlags.Skybox;
        }
        else
        {
            Debug.LogWarning("Main Camera를 찾을 수 없습니다. CameraClearFlags 설정이 실패했습니다.");
        }
    }
}
