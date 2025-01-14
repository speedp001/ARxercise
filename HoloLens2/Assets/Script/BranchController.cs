using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;
using System.Collections.Generic;

public class BranchController : MonoBehaviour
{
    [Header("MRTK Components")]
    public ObjectManipulator manipulator;

    [Header("Transparency Settings")]
    [Range(0f, 1f)] public float ungrabbedAlpha = 0.3f; // 잡기 전 투명도
    [Range(0f, 1f)] public float grabbedAlpha = 1.0f; // 잡았을 때 투명도

    // Branch, Worm 등 자식에 있는 모든 Renderer를 한 번에 처리하기 위함
    private Renderer[] childRenderers;

    void Awake()
    {
        // 자신을 포함해 자식까지 달려 있는 모든 Renderer 가져오기
        childRenderers = GetComponentsInChildren<Renderer>();
    }

    void Start()
    {
        // 시작할 때 투명도 설정 (기본적으로 잡기 전 상태)
        SetAlpha(ungrabbedAlpha);

        // ObjectManipulator 이벤트에 함수 등록
        if (manipulator != null)
        {
            manipulator.OnManipulationStarted.AddListener(OnGrabStarted);
            manipulator.OnManipulationEnded.AddListener(OnGrabEnded);
        }
        else
        {
            Debug.LogError("ObjectManipulator가 연결되지 않았습니다.");
        }
    }

    private void OnGrabStarted(ManipulationEventData eventData)
    {
        // 사용자가 가지를 잡기 시작했을 때 → 불투명하게
        SetAlpha(grabbedAlpha);
    }

    private void OnGrabEnded(ManipulationEventData eventData)
    {
        // 사용자가 가지를 놓았을 때 → 다시 투명하게
        SetAlpha(ungrabbedAlpha);
    }

    // Branch 및 자식 오브젝트(Renderer)의 알파값 변경
    private void SetAlpha(float alphaValue)
    {
        foreach (var rend in childRenderers)
        {
            // 오브젝트에 여러 Material이 있을 시
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
