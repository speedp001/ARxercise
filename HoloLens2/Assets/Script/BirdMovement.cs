using UnityEngine;

public class BirdMovement : MonoBehaviour
{
    public GameObject birdObject;                // 실제 새 오브젝트
    public GameObject greenWormObject;     // 실제 애벌레 오브젝트
    public Transform startFlyingPosition;  // 새의 처음 비행을 시작하는 위치
    public Transform midAirPositionLeft;   // 왼쪽 중간 지점 위치
    public Transform midAirPositionRight;  // 오른쪽 중간 지점 위치
    public Transform wormPositionLeft;     // 왼쪽 애벌레 위치
    public Transform wormPositionRight;    // 오른쪽 애벌레 위치
    public Transform endPointLeft;         // 왼쪽 마지막 지점
    public Transform endPointRight;        // 오른쪽 마지막 지점
    public float flySpeed;                 // 새의 비행 속도
    public Animator birdAnimator;          // 새의 애니메이터 컴포넌트 참조
    public AudioSource audioSource;        // 새의 오디오 소스 컴포넌트
    public AudioClip birdSound;            // 새가 날아다닐 때의 소리 클립
    public AudioClip countSound;           // 애벌레를 잡았을 때의 소리 클립

    private bool startFlying = false;      // 비행을 시작할지 여부를 나타내는 플래그
    private bool flyFromLeft;              // 왼쪽에서 출발하는지 여부를 나타내는 플래그
    private bool hasReachedMidPoint = false; // 중간 지점에 도달했는지 여부
    private bool hasReachedWorm = false;   // 애벌레 지점에 도달했는지 여부
    private bool hasPickedUpWorm = false;  // 최종 지점에 도달했는지 여부
    private Transform currentWormPosition; // 현재 목표 애벌레 위치
    private Transform currentEndPoint;     // 현재 목표 마지막 지점

    void Start()
    {
        // 오디오를 재생 공간
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        ResetBirdPosition(); // 새의 위치 초기화
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
        if (!hasReachedMidPoint) // 중간 지점으로 이동
        {
            // Glide로 상태 변경
            birdAnimator.SetTrigger("Glide");

            MoveTowardsTarget(flyFromLeft ? midAirPositionLeft : midAirPositionRight, "Glide", () =>
            {
                Debug.Log("Reached mid-air position. Transitioning to PickUpWorm.");
                hasReachedMidPoint = true;
            });
        }
        else if (!hasReachedWorm) // 애벌레 위치로 이동
        {
            // Landing으로 상태 변경
            birdAnimator.SetTrigger("PickUpWorm");

            MoveTowardsTarget(currentWormPosition, "PickUpWorm", () =>
            {
                Debug.Log("Reached worm position. Attempting to pick up worm.");
                hasReachedWorm = true;

                // 새 오브젝트 숨김
                HideBird();
            });
        }
        else if (!hasPickedUpWorm) // 최종 도착 지점으로 이동
        {
            // flyBack으로 상태 변경
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

        float temporaryFlySpeed = flySpeed; // 기본 속도를 저장
        Vector3 currentPosition = transform.position;

        // 중간 지점에서 Worm 위치로 이동 시 속도 조정
        if (!hasReachedWorm && hasReachedMidPoint)
        {
            temporaryFlySpeed = flySpeed * 1.5f; // 속도 증가

            // Character Controller 중심 좌표 변경
            AdjustCharacterController();
        }

        if (Vector3.Distance(transform.position, target.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, temporaryFlySpeed * Time.deltaTime);
            Debug.Log($"Moving towards {target.name}: Current Position: {transform.position}, Target: {target.position}");
        }
        else
        {
            onReached?.Invoke(); // 목표 위치 도달 시 콜백 호출
        }
    }

    private void AdjustCharacterController()
    {
        CharacterController characterController = GetComponent<CharacterController>();
        if (characterController != null)
        {
            characterController.center = new Vector3(0f, 0.25f, 1.6f); // landing 애니메이션에 맞게 조절
            Debug.Log($"Character Controller center adjusted: {characterController.center}");
        }
    }

    private void ResetCharacterController()
    {
        CharacterController characterController = GetComponent<CharacterController>();
        if (characterController != null)
        {
            characterController.center = new Vector3(0f, 0.9f, 0f); // 기본값으로 복원
            Debug.Log($"Character Controller center reset to: {characterController.center}");
        }
    }

    private void HideBird()
    {
        Renderer[] renderers = birdObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = false; // 모든 Renderer 비활성화
        }
        Debug.Log("Bird is now hidden.");
    }

    private void ShowBird()
    {
        Renderer[] renderers = birdObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = true; // 모든 Renderer 활성화
        }
        Debug.Log("Bird is now visible.");
    }

    private void ReturnToStart()
    {
        startFlying = false;
        hasReachedMidPoint = false;
        hasReachedWorm = false;
        hasPickedUpWorm = false;

        // 새 오브젝트 보이기
        ShowBird();

        //Character Controller 초기화
        ResetCharacterController();

        // Animator 초기화
        birdAnimator.ResetTrigger("Glide");
        birdAnimator.ResetTrigger("PickUpWorm");
        birdAnimator.ResetTrigger("flyBack");
        birdAnimator.SetTrigger("Exit");

        // Bird 위치 초기화 및 GreenWorm 재생성
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