using UnityEngine;

public class HandleEffect : MonoBehaviour
{
    [Header("Visual Blink Settings")]
    public Renderer targetRenderer;
    public Color emissionColor;
    public float blinkSpeed;
    public float effectDuration;

    [Header("Audio Settings")]
    public AudioSource whooshAudio;
    public AudioSource introAudio;
    public float speedThreshold = 2f;

    private Material targetMaterial;
    private bool isBlinking = true;

    // Transform 기반 속도 계산 변수
    private Vector3 lastPosition;

    void Start()
    {
        // 시작 안내 음성 재생
        if (introAudio != null)
        {
            introAudio.Play();
        }

        if (targetRenderer == null)
        {
            targetRenderer = GetComponent<Renderer>();
        }
        targetMaterial = targetRenderer.material;

        // 반짝거림 시작
        targetMaterial.EnableKeyword("_EMISSION");
        targetMaterial.SetColor("_EmissionColor", emissionColor);

        // 효과 지속 시간 후 깜박임 중지 및 비활성화
        Invoke(nameof(StopBlinking), effectDuration);

        // 초기 위치 설정
        lastPosition = transform.position;
    }

    void Update()
    {
        // 1) 반짝거림 효과 업데이트
        if (isBlinking)
        {
            float emissionIntensity = Mathf.PingPong(Time.time * blinkSpeed, 1.0f);
            targetMaterial.SetColor("_EmissionColor", emissionColor * emissionIntensity);
        }

        // 2) 이동 속도 측정 -> 일정 속도 이상이면 오디오 재생
        float distance = Vector3.Distance(transform.position, lastPosition);
        float speed = distance / Time.deltaTime; // 초당 이동거리

        // speedThreshold 이상이고, 오디오가 재생 중이 아니면 재생 시작
        if (speed > speedThreshold && whooshAudio != null && !whooshAudio.isPlaying)
        {
            whooshAudio.Play();
        }
        // 속도가 threshold 이하인데 오디오가 재생 중이면 정지
        else if (speed < speedThreshold && whooshAudio != null && whooshAudio.isPlaying)
        {
            whooshAudio.Stop();
        }

        // 이번 프레임의 위치를 lastPosition으로 갱신
        lastPosition = transform.position;
    }

    private void StopBlinking()
    {
        isBlinking = false;

        // 반짝거림 끄기
        targetMaterial.SetColor("_EmissionColor", Color.black);

        // 렌더링 중지
        targetRenderer.enabled = false;
    }
}

