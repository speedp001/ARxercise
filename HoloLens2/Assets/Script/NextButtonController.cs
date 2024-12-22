using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NextButtonController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip exerciseDescriptionSound; // Exercise Description_1 ����
    public AudioClip clickSound; // Ŭ�� �Ҹ�
    public Button explanationButton;
    public GameObject canvas2; // ���� Canvas2
    public GameObject canvas3; // ��ȯ�� Canvas3

    private void OnEnable()
    {
        // Canvas2�� Ȱ��ȭ�� �� ����Ǵ� �ڵ�
        PlayExerciseDescriptionSound();

        // ��ư Ŭ�� �� ���� Canvas�� ��ȯ�ϴ� �̺�Ʈ ���
        explanationButton.onClick.AddListener(OnExplanationButtonClick);

        // ��ư�� 5�� ���� ��Ȱ��ȭ �� �ٽ� Ȱ��ȭ
        explanationButton.interactable = false; // ��ư ��Ȱ��ȭ
        StartCoroutine(EnableButtonAfterDelay(15f)); // 15�� �� ��ư Ȱ��ȭ
    }

    private void OnDisable()
    {
        // Canvas2�� ��Ȱ��ȭ�� �� �̺�Ʈ ����
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
            Debug.LogWarning("AudioSource �Ǵ� Exercise Description ������ �������� �ʾҽ��ϴ�.");
        }
    }

    private void OnExplanationButtonClick()
    {
        // Ŭ�� �Ҹ� ���
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }

        // Canvas2 ��Ȱ��ȭ�ϰ� Canvas3 Ȱ��ȭ
        StartCoroutine(TransitionToCanvas3());
    }

    IEnumerator TransitionToCanvas3()
    {
        // ��ȯ ���� ��� ��� (�ʿ� ��)
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
        explanationButton.interactable = true; // ��ư Ȱ��ȭ
    }
}
