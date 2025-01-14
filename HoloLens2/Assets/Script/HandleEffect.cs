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

    // Transform ��� �ӵ� ��� ����
    private Vector3 lastPosition;

    void Start()
    {
        // ���� �ȳ� ���� ���
        if (introAudio != null)
        {
            introAudio.Play();
        }

        if (targetRenderer == null)
        {
            targetRenderer = GetComponent<Renderer>();
        }
        targetMaterial = targetRenderer.material;

        // ��¦�Ÿ� ����
        targetMaterial.EnableKeyword("_EMISSION");
        targetMaterial.SetColor("_EmissionColor", emissionColor);

        // ȿ�� ���� �ð� �� ������ ���� �� ��Ȱ��ȭ
        Invoke(nameof(StopBlinking), effectDuration);

        // �ʱ� ��ġ ����
        lastPosition = transform.position;
    }

    void Update()
    {
        // 1) ��¦�Ÿ� ȿ�� ������Ʈ
        if (isBlinking)
        {
            float emissionIntensity = Mathf.PingPong(Time.time * blinkSpeed, 1.0f);
            targetMaterial.SetColor("_EmissionColor", emissionColor * emissionIntensity);
        }

        // 2) �̵� �ӵ� ���� -> ���� �ӵ� �̻��̸� ����� ���
        float distance = Vector3.Distance(transform.position, lastPosition);
        float speed = distance / Time.deltaTime; // �ʴ� �̵��Ÿ�

        // speedThreshold �̻��̰�, ������� ��� ���� �ƴϸ� ��� ����
        if (speed > speedThreshold && whooshAudio != null && !whooshAudio.isPlaying)
        {
            whooshAudio.Play();
        }
        // �ӵ��� threshold �����ε� ������� ��� ���̸� ����
        else if (speed < speedThreshold && whooshAudio != null && whooshAudio.isPlaying)
        {
            whooshAudio.Stop();
        }

        // �̹� �������� ��ġ�� lastPosition���� ����
        lastPosition = transform.position;
    }

    private void StopBlinking()
    {
        isBlinking = false;

        // ��¦�Ÿ� ����
        targetMaterial.SetColor("_EmissionColor", Color.black);

        // ������ ����
        targetRenderer.enabled = false;
    }
}

