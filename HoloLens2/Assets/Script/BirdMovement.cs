using UnityEngine;

public class BirdMovement : MonoBehaviour
{
    public Transform startFlyingPosition;  // ���� ó�� ������ �����ϴ� ��ġ
    public GameObject greenWormObject; // ���� �ֹ��� ������Ʈ
    public Transform midAirPositionLeft;   // ���� �߰� ���� ��ġ
    public Transform midAirPositionRight;  // ������ �߰� ���� ��ġ
    public Transform wormPositionLeft;     // ���� �ֹ��� ��ġ
    public Transform wormPositionRight;    // ������ �ֹ��� ��ġ
    public Transform endPointLeft;         // ���� ������ ����
    public Transform endPointRight;        // ������ ������ ����
    public float flySpeed = 2f;            // ���� ���� �ӵ�
    public Animator birdAnimator;          // ���� �ִϸ����� ������Ʈ ����
    public AudioSource audioSource;        // ���� ����� �ҽ� ������Ʈ
    public AudioClip birdSound;            // ���� ���ƴٴ� ���� �Ҹ� Ŭ��
    public AudioClip countSound;           // �ֹ����� ����� ���� �Ҹ� Ŭ��

    private bool startFlying = false;      // ������ �������� ���θ� ��Ÿ���� �÷���
    private bool flyFromLeft = true;       // ���ʿ��� ����ϴ��� ���θ� ��Ÿ���� �÷���
    private bool hasReachedWorm = false;   // �ֹ����� �����ߴ��� ���θ� ��Ÿ���� �÷���
    private bool hasPickedUpWorm = false;  // �ֹ����� ��Ҵ��� ���θ� ��Ÿ���� �÷���
    private Transform currentWormPosition; // ���� ��ǥ �ֹ��� ��ġ
    private Transform currentEndPoint;     // ���� ��ǥ ������ ����
    private GameObject currentWormObject;  // ���� �ֹ��� ������Ʈ

    // �ʱ� ����
    void Start()
    {
        // ����� �ҽ��� �������� ���� ��� �� ����� �ҽ� ������Ʈ�� �߰�
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        ResetBirdPosition(); // ���� ��ġ �ʱ�ȭ
        Debug.Log("BirdMovement started. Initial position reset."); // �ʱ�ȭ �Ϸ� �α� ���
    }

    // �� ������ ȣ��Ǵ� �޼���
    void Update()
    {
        // ������ ���۵Ǿ��� ��� FlyToTarget �޼��� ȣ��
        if (startFlying)
        {
            FlyToTarget();
        }
    }

    // ���� ������ ������ �� ȣ��Ǵ� �޼���
    public void StartFlying(bool fromLeft)
    {
        startFlying = true; // ���� ���� ����
        flyFromLeft = fromLeft; // ��� ���� ����
        currentWormPosition = flyFromLeft ? wormPositionLeft : wormPositionRight; // �ֹ��� ��ǥ ��ġ ����
        currentEndPoint = flyFromLeft ? endPointLeft : endPointRight; // ���� ���� ���� ����
        currentWormObject = greenWormObject; // �ֹ��� ������Ʈ ����

        PlaySound(birdSound, true); // ���� �Ҹ� ���
        birdAnimator.SetTrigger("fly"); // �ִϸ����� Ʈ���� �����Ͽ� fly �ִϸ��̼� ����

        // ���� ���� �α� ���
        Debug.Log($"Flight started from {(flyFromLeft ? "left" : "right")}.");
    }

    // ��ǥ �������� ���� �̵���Ű�� �޼���
    private void FlyToTarget()
    {
        if (!hasReachedWorm) // �߰����� �������� ���� ���
        {
            // �߰� �������� �̵�
            Transform targetPosition = flyFromLeft ? midAirPositionLeft : midAirPositionRight;
            MoveTowardsTarget(targetPosition, "Glide", "Reached mid-air position, transitioning to worm.", () =>
            {
                hasReachedWorm = true; // �ֹ��� ��ǥ ��ġ�� ��ȯ
                birdAnimator.SetTrigger("Glide"); // Glide �ִϸ��̼� Ʈ���� ����
                Debug.Log($"Transition to Glide state. Current Position: {transform.position}, Target: {targetPosition.position}");
            });
        }
        else if (!hasPickedUpWorm) // �ֹ��� ��ġ �������� ���� ���
        {
            // �ֹ��� ��ġ�� �̵�
            MoveTowardsTarget(currentWormPosition, "PickUpWorm", "Reached worm position, attempting to pick up worm.", AttemptPickUpWorm);
        }
        else // ���� ���� �������� �̵�
        {
            MoveTowardsTarget(currentEndPoint, "flyBack", "Reached endpoint. Preparing to reset.", ReturnToStart);
        }
    }

    // ��ǥ �������� �̵��ϸ鼭 �ִϸ��̼��� �����ϴ� �޼���
    private void MoveTowardsTarget(Transform target, string animationTrigger, string reachedLog, System.Action onReached = null)
    {
        // ��ǥ ��ġ�� �������� ���� ��� ��� �̵�
        if (Vector3.Distance(transform.position, target.position) > 0.1f)
        {
            // ��ǥ �������� �̵�
            transform.position = Vector3.MoveTowards(transform.position, target.position, flySpeed * Time.deltaTime);
            birdAnimator.SetTrigger(animationTrigger); // �ִϸ��̼� Ʈ���� ����
            Debug.Log($"Moving towards {target.name}: Current Position: {transform.position}, Target: {target.position}");
        }
        else // ��ǥ ��ġ�� ������ ���
        {
            Debug.Log(reachedLog); // ������ �α� ���
            onReached?.Invoke(); // ���� �� ������ �ݹ� �Լ� ȣ��
        }
    }

    // EndPoint�� ���� ���ִ� ���ǹ�
    private void AttemptPickUpWorm()
    {
        if (currentWormObject.activeSelf)
        {
            hasPickedUpWorm = true;
        }
    }


    // ���� ��ġ�� ���ư��� �޼���
    private void ReturnToStart()
    {
        startFlying = false; // ���� ���� ����
        ResetBirdPosition(); // �� ��ġ �ʱ�ȭ
        Debug.Log("Returned to start position and reset worm.");
    }

    // �� ��ġ�� �ʱ�ȭ�ϴ� �޼���
    private void ResetBirdPosition()
    {
        hasReachedWorm = false; // �߰����� ���� ����
        hasPickedUpWorm = false; // �ֹ��� ��ġ ���� ����
        transform.position = startFlyingPosition.position; // ���� ��ġ�� �̵�
        //birdAnimator.SetTrigger("fly"); // �ʱ� ���� �ִϸ��̼� ����
        if (currentWormObject != null)
        {
            currentWormObject.SetActive(true);
        }
        StopSound(); // �Ҹ� ����
        Debug.Log("Bird position reset to start.");
    }

    // �Ҹ� ��� �޼���
    private void PlaySound(AudioClip clip, bool loop)
    {
        if (audioSource != null && clip != null) // ����� �ҽ��� Ŭ���� ������ ���
        {
            audioSource.clip = clip; // Ŭ�� ����
            audioSource.loop = loop; // �ݺ� ���� ����
            audioSource.Play(); // �Ҹ� ���
            Debug.Log("Playing sound: " + clip.name);
        }
    }

    // �Ҹ� ���� �޼���
    private void StopSound()
    {
        if (audioSource.isPlaying) // �Ҹ��� ��� ���� ���
        {
            audioSource.Stop(); // �Ҹ� ����
            Debug.Log("Stopped sound.");
        }
    }

    // ���� �ֹ��� �浹 �̺�Ʈ �߻� �� ȣ��Ǵ� �޼���
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter called with object: " + other.gameObject.name);  // �浹�� ������Ʈ �̸� Ȯ��
        if (other.CompareTag("Worm")) // Worm �±׿� �浹
        {
            hasPickedUpWorm = true; // �ֹ����� ���� ���·� ����
            PlaySound(countSound, false); // �Ҹ� ���
            greenWormObject.SetActive(false); // �ֹ��� ������Ʈ ��Ȱ��ȭ
            Debug.Log("Worm trigger entered, picked up worm and played count sound.");
        }
        else
        {
            Debug.Log("Trigger entered but no interaction with worm. Object: " + other.name);
        }
    }
}
