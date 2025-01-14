using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;
using System.Collections.Generic;

public class BranchController : MonoBehaviour
{
    [Header("MRTK Components")]
    public ObjectManipulator manipulator;

    [Header("Transparency Settings")]
    [Range(0f, 1f)] public float ungrabbedAlpha = 0.3f; // ��� �� ����
    [Range(0f, 1f)] public float grabbedAlpha = 1.0f; // ����� �� ����

    // Branch, Worm �� �ڽĿ� �ִ� ��� Renderer�� �� ���� ó���ϱ� ����
    private Renderer[] childRenderers;

    void Awake()
    {
        // �ڽ��� ������ �ڽı��� �޷� �ִ� ��� Renderer ��������
        childRenderers = GetComponentsInChildren<Renderer>();
    }

    void Start()
    {
        // ������ �� ���� ���� (�⺻������ ��� �� ����)
        SetAlpha(ungrabbedAlpha);

        // ObjectManipulator �̺�Ʈ�� �Լ� ���
        if (manipulator != null)
        {
            manipulator.OnManipulationStarted.AddListener(OnGrabStarted);
            manipulator.OnManipulationEnded.AddListener(OnGrabEnded);
        }
        else
        {
            Debug.LogError("ObjectManipulator�� ������� �ʾҽ��ϴ�.");
        }
    }

    private void OnGrabStarted(ManipulationEventData eventData)
    {
        // ����ڰ� ������ ��� �������� �� �� �������ϰ�
        SetAlpha(grabbedAlpha);
    }

    private void OnGrabEnded(ManipulationEventData eventData)
    {
        // ����ڰ� ������ ������ �� �� �ٽ� �����ϰ�
        SetAlpha(ungrabbedAlpha);
    }

    // Branch �� �ڽ� ������Ʈ(Renderer)�� ���İ� ����
    private void SetAlpha(float alphaValue)
    {
        foreach (var rend in childRenderers)
        {
            // ������Ʈ�� ���� Material�� ���� ��
            var mats = rend.materials;
            for (int i = 0; i < mats.Length; i++)
            {
                Color c = mats[i].color;
                c.a = alphaValue;
                mats[i].color = c;
            }
        }
    }
}
