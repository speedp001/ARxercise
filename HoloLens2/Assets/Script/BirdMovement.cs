using UnityEngine;

public class BirdMovement : MonoBehaviour
{
    public GameObject birdObject;                // ���� �� ������Ʈ
    public GameObject greenWormObject;     // ���� �ֹ��� ������Ʈ
    public Transform startFlyingPosition;  // ���� ó�� ������ �����ϴ� ��ġ
    public Transform midAirPositionLeft;   // ���� �߰� ���� ��ġ
    public Transform midAirPositionRight;  // ������ �߰� ���� ��ġ
    public Transform wormPositionLeft;     // ���� �ֹ��� ��ġ
    public Transform wormPositionRight;    // ������ �ֹ��� ��ġ
    public Transform endPointLeft;         // ���� ������ ����
    public Transform endPointRight;        // ������ ������ ����
    public float flySpeed;                 // ���� ���� �ӵ�
    public Animator birdAnimator;          // ���� �ִϸ����� ������Ʈ ����
    public AudioSource audioSource;        // ���� ����� �ҽ� ������Ʈ
    public AudioClip birdSound;            // ���� ���ƴٴ� ���� �Ҹ� Ŭ��
    public AudioClip countSound;           // �ֹ����� ����� ���� �Ҹ� Ŭ��

    private bool startFlying = false;      // ������ �������� ���θ� ��Ÿ���� �÷���
    private bool flyFromLeft;              // ���ʿ��� ����ϴ��� ���θ� ��Ÿ���� �÷���
    private bool hasReachedMidPoint = false; // �߰� ������ �����ߴ��� ����
    private bool hasReachedWorm = false;   // �ֹ��� ������ �����ߴ��� ����
    private bool hasPickedUpWorm = false;  // ���� ������ �����ߴ��� ����
    private Transform currentWormPosition; // ���� ��ǥ �ֹ��� ��ġ
    private Transform currentEndPoint;     // ���� ��ǥ ������ ����

    void Start()
    {
        // ������� ��� ����
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        ResetBirdPosition(); // ���� ��ġ �ʱ�ȭ
        Debug.Log("BirdMovement started. Initial position reset.");
    }

    void Update()
    {
        if (startFlying)
        {
            FlyToTarget();
        }
    }

    public void StartFlying(bool fromLeft)
    {
        flyFromLeft = fromLeft;
        currentWormPosition = flyFromLeft ? wormPositionLeft : wormPositionRight;
        currentEndPoint = flyFromLeft ? endPointLeft : endPointRight;

        PlaySound(birdSound, true);
        Debug.Log($"Flight started from {(flyFromLeft ? "left" : "right")}.");

        startFlying = true;
    }

    private void FlyToTarget()
    {
        if (!hasReachedMidPoint) // �߰� �������� �̵�
        {
            // Glide�� ���� ����
            birdAnimator.SetTrigger("Glide");

            MoveTowardsTarget(flyFromLeft ? midAirPositionLeft : midAirPositionRight, "Glide", () =>
            {
                Debug.Log("Reached mid-air position. Transitioning to PickUpWorm.");
                hasReachedMidPoint = true;
            });
        }
        else if (!hasReachedWorm) // �ֹ��� ��ġ�� �̵�
        {
            // Landing���� ���� ����
            birdAnimator.SetTrigger("PickUpWorm");

            MoveTowardsTarget(currentWormPosition, "PickUpWorm", () =>
            {
                Debug.Log("Reached worm position. Attempting to pick up worm.");
                hasReachedWorm = true;

                // �� ������Ʈ ����
                HideBird();
            });
        }
        else if (!hasPickedUpWorm) // ���� ���� �������� �̵�
        {
            // flyBack���� ���� ����
            birdAnimator.SetTrigger("flyBack");

            MoveTowardsTarget(currentEndPoint, "flyBack", () =>
            {
                Debug.Log("Reached endpoint. Preparing to reset.");
                ReturnToStart();
            });
        }
    }

    private void MoveTowardsTarget(Transform target, string animationTrigger, System.Action onReached = null)
    {

        float temporaryFlySpeed = flySpeed; // �⺻ �ӵ��� ����
        Vector3 currentPosition = transform.position;

        // �߰� �������� Worm ��ġ�� �̵� �� �ӵ� ����
        if (!hasReachedWorm && hasReachedMidPoint)
        {
            temporaryFlySpeed = flySpeed * 1.5f; // �ӵ� ����

            // Character Controller �߽� ��ǥ ����
            AdjustCharacterController();
        }

        if (Vector3.Distance(transform.position, target.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, temporaryFlySpeed * Time.deltaTime);
            Debug.Log($"Moving towards {target.name}: Current Position: {transform.position}, Target: {target.position}");
        }
        else
        {
            onReached?.Invoke(); // ��ǥ ��ġ ���� �� �ݹ� ȣ��
        }
    }

    private void AdjustCharacterController()
    {
        CharacterController characterController = GetComponent<CharacterController>();
        if (characterController != null)
        {
            characterController.center = new Vector3(0f, 0.25f, 1.6f); // landing �ִϸ��̼ǿ� �°� ����
            Debug.Log($"Character Controller center adjusted: {characterController.center}");
        }
    }

    private void ResetCharacterController()
    {
        CharacterController characterController = GetComponent<CharacterController>();
        if (characterController != null)
        {
            characterController.center = new Vector3(0f, 0.9f, 0f); // �⺻������ ����
            Debug.Log($"Character Controller center reset to: {characterController.center}");
        }
    }

    private void HideBird()
    {
        Renderer[] renderers = birdObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = false; // ��� Renderer ��Ȱ��ȭ
        }
        Debug.Log("Bird is now hidden.");
    }

    private void ShowBird()
    {
        Renderer[] renderers = birdObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = true; // ��� Renderer Ȱ��ȭ
        }
        Debug.Log("Bird is now visible.");
    }

    private void ReturnToStart()
    {
        startFlying = false;
        hasReachedMidPoint = false;
        hasReachedWorm = false;
        hasPickedUpWorm = false;

        // �� ������Ʈ ���̱�
        ShowBird();

        //Character Controller �ʱ�ȭ
        ResetCharacterController();

        // Animator �ʱ�ȭ
        birdAnimator.ResetTrigger("Glide");
        birdAnimator.ResetTrigger("PickUpWorm");
        birdAnimator.ResetTrigger("flyBack");
        birdAnimator.SetTrigger("Exit");

        // Bird ��ġ �ʱ�ȭ �� GreenWorm �����
        ResetBirdPosition();
        Debug.Log("Returned to start position and reset worm.");
    }

    private void ResetBirdPosition()
    {
        transform.position = startFlyingPosition.position;
        greenWormObject.SetActive(true);
        StopSound();
        Debug.Log("Bird position reset to start.");
    }

    private void PlaySound(AudioClip clip, bool loop)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.clip = clip;
            audioSource.loop = loop;
            audioSource.Play();
            Debug.Log("Playing sound: " + clip.name);
        }
    }

    private void StopSound()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
            Debug.Log("Stopped sound.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter called with object: " + other.gameObject.name);
        if (other.CompareTag("Worm"))
        {
            //hasPickedUpWorm = true;
            PlaySound(countSound, false);
            greenWormObject.SetActive(false);
            Debug.Log("Worm trigger entered, picked up worm and played count sound.");
        }
    }
}