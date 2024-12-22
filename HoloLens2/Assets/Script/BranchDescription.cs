using UnityEngine;
using System.Collections;

public class BranchDescription : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip branchDescriptionSound; // Branch Description ����
    public GameObject canvas3; // ���� Canvas3
    public GameObject canvas4; // ��ȯ�� Canvas4

    private void OnEnable()
    {
        // Canvas3�� Ȱ��ȭ�� �� ����Ǵ� �ڵ�
        PlayBranchDescriptionSound();
    }

    private void PlayBranchDescriptionSound()
    {
        if (audioSource != null && branchDescriptionSound != null)
        {
            audioSource.clip = branchDescriptionSound;
            audioSource.Play();

            // ���� ����� ������ ���� Canvas�� ��ȯ
            StartCoroutine(TransitionToCanvas4AfterSound());
        }
        else
        {
            Debug.LogWarning("AudioSource �Ǵ� Branch Description ������ �������� �ʾҽ��ϴ�.");
        }
    }

    IEnumerator TransitionToCanvas4AfterSound()
    {
        // ������ ���� ������ ���
        yield return new WaitForSeconds(branchDescriptionSound.length);

        // �߰� ��� (1��)
        yield return new WaitForSeconds(2f);

        // Canvas3 ��Ȱ��ȭ�ϰ� Canvas4 Ȱ��ȭ
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
