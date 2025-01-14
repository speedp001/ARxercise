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

        // ���� ���� ���
        if (audioSource != null && exerciseDescriptionSound != null)
        {
            audioSource.clip = exerciseDescriptionSound;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioSource �Ǵ� ExerciseDescriptionSound�� �������� �ʾҽ��ϴ�.");
        }

        // 15�� �� ��ư ��ȯ
        StartCoroutine(SwitchButtonsAfterDelay());

        // PressableButton�� ButtonPressed �̺�Ʈ ����
        if (newNextButton != null)
        {
            newNextButton.ButtonPressed.AddListener(NextButtonPressed);
        }
        else
        {
            Debug.LogError("newNextButton�� �������� �ʾҽ��ϴ�.");
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
        // Ŭ�� �Ҹ� ���
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
        else
        {
            Debug.LogWarning("AudioSource �Ǵ� ClickSound�� �������� �ʾҽ��ϴ�.");
        }

        // Canvas ��ȯ
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
