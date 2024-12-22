using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BirdDescription : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip birdDescriptionSound; // Bird Description ����
    public GameObject canvas4; // ���� Canvas4

    private void OnEnable()
    {
        // Canvas4�� Ȱ��ȭ�� �� ����Ǵ� �ڵ�
        PlayBirdDescriptionSound();
    }

    private void PlayBirdDescriptionSound()
    {
        if (audioSource != null && birdDescriptionSound != null)
        {
            audioSource.clip = birdDescriptionSound;
            audioSource.Play();

            // ���� ����� ������ BirdGame ������� ��ȯ
            StartCoroutine(TransitionToBirdGameAfterSound());
        }
        else
        {
            Debug.LogWarning("AudioSource �Ǵ� Bird Description ������ �������� �ʾҽ��ϴ�.");
        }
    }

    IEnumerator TransitionToBirdGameAfterSound()
    {
        // ������ ���� ������ ���
        yield return new WaitForSeconds(birdDescriptionSound.length);

        // �߰� ��� (1��)
        yield return new WaitForSeconds(2f);

        // BirdGame ������� ��ȯ
        yield return SceneManager.LoadSceneAsync("BirdGame");

        // BirdGame ����� �ε�� �� ī�޶� ���� ����
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
            Debug.LogWarning("Main Camera�� ã�� �� �����ϴ�. CameraClearFlags ������ �����߽��ϴ�.");
        }
    }
}
