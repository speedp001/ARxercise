using UnityEngine;

public class BirdMovement : MonoBehaviour
{
    public Transform startFlyingPosition;  // 새의 처음 비행을 시작하는 위치
    public GameObject greenWormObject; // 실제 애벌레 오브젝트
    public Transform midAirPositionLeft;   // 왼쪽 중간 지점 위치
    public Transform midAirPositionRight;  // 오른쪽 중간 지점 위치
    public Transform wormPositionLeft;     // 왼쪽 애벌레 위치
    public Transform wormPositionRight;    // 오른쪽 애벌레 위치
    public Transform endPointLeft;         // 왼쪽 마지막 지점
    public Transform endPointRight;        // 오른쪽 마지막 지점
    public float flySpeed = 2f;            // 새의 비행 속도
    public Animator birdAnimator;          // 새의 애니메이터 컴포넌트 참조
    public AudioSource audioSource;        // 새의 오디오 소스 컴포넌트
    public AudioClip birdSound;            // 새가 날아다닐 때의 소리 클립
    public AudioClip countSound;           // 애벌레를 잡았을 때의 소리 클립

    private bool startFlying = false;      // 비행을 시작할지 여부를 나타내는 플래그
    private bool flyFromLeft = true;       // 왼쪽에서 출발하는지 여부를 나타내는 플래그
    private bool hasReachedWorm = false;   // 애벌레에 도착했는지 여부를 나타내는 플래그
    private bool hasPickedUpWorm = false;  // 애벌레를 잡았는지 여부를 나타내는 플래그
    private Transform currentWormPosition; // 현재 목표 애벌레 위치
    private Transform currentEndPoint;     // 현재 목표 마지막 지점
    private GameObject currentWormObject;  // 현재 애벌레 오브젝트

    // 초기 설정
    void Start()
    {
        // 오디오 소스가 설정되지 않은 경우 새 오디오 소스 컴포넌트를 추가
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        ResetBirdPosition(); // 새의 위치 초기화
        Debug.Log("BirdMovement started. Initial position reset."); // 초기화 완료 로그 출력
    }

    // 매 프레임 호출되는 메서드
    void Update()
    {
        // 비행이 시작되었을 경우 FlyToTarget 메서드 호출
        if (startFlying)
        {
            FlyToTarget();
        }
    }

    // 새의 비행을 시작할 때 호출되는 메서드
    public void StartFlying(bool fromLeft)
    {
        startFlying = true; // 비행 시작 설정
        flyFromLeft = fromLeft; // 출발 방향 설정
        currentWormPosition = flyFromLeft ? wormPositionLeft : wormPositionRight; // 애벌레 목표 위치 설정
        currentEndPoint = flyFromLeft ? endPointLeft : endPointRight; // 최종 도착 지점 설정
        currentWormObject = greenWormObject; // 애벌레 오브젝트 설정

        PlaySound(birdSound, true); // 비행 소리 재생
        birdAnimator.SetTrigger("fly"); // 애니메이터 트리거 설정하여 fly 애니메이션 시작

        // 비행 시작 로그 출력
        Debug.Log($"Flight started from {(flyFromLeft ? "left" : "right")}.");
    }

    // 목표 지점으로 새를 이동시키는 메서드
    private void FlyToTarget()
    {
        if (!hasReachedWorm) // 중간지점 도착하지 않은 경우
        {
            // 중간 지점으로 이동
            Transform targetPosition = flyFromLeft ? midAirPositionLeft : midAirPositionRight;
            MoveTowardsTarget(targetPosition, "Glide", "Reached mid-air position, transitioning to worm.", () =>
            {
                hasReachedWorm = true; // 애벌레 목표 위치로 전환
                birdAnimator.SetTrigger("Glide"); // Glide 애니메이션 트리거 설정
                Debug.Log($"Transition to Glide state. Current Position: {transform.position}, Target: {targetPosition.position}");
            });
        }
        else if (!hasPickedUpWorm) // 애벌레 위치 도착하지 않은 경우
        {
            // 애벌레 위치로 이동
            MoveTowardsTarget(currentWormPosition, "PickUpWorm", "Reached worm position, attempting to pick up worm.", AttemptPickUpWorm);
        }
        else // 최종 도착 지점으로 이동
        {
            MoveTowardsTarget(currentEndPoint, "flyBack", "Reached endpoint. Preparing to reset.", ReturnToStart);
        }
    }

    // 목표 지점으로 이동하면서 애니메이션을 설정하는 메서드
    private void MoveTowardsTarget(Transform target, string animationTrigger, string reachedLog, System.Action onReached = null)
    {
        // 목표 위치에 도달하지 않은 경우 계속 이동
        if (Vector3.Distance(transform.position, target.position) > 0.1f)
        {
            // 목표 지점으로 이동
            transform.position = Vector3.MoveTowards(transform.position, target.position, flySpeed * Time.deltaTime);
            birdAnimator.SetTrigger(animationTrigger); // 애니메이션 트리거 설정
            Debug.Log($"Moving towards {target.name}: Current Position: {transform.position}, Target: {target.position}");
        }
        else // 목표 위치에 도달한 경우
        {
            Debug.Log(reachedLog); // 도달한 로그 출력
            onReached?.Invoke(); // 도착 시 실행할 콜백 함수 호출
        }
    }

    // EndPoint로 가게 해주는 조건문
    private void AttemptPickUpWorm()
    {
        if (currentWormObject.activeSelf)
        {
            hasPickedUpWorm = true;
        }
    }


    // 시작 위치로 돌아가는 메서드
    private void ReturnToStart()
    {
        startFlying = false; // 비행 중지 설정
        ResetBirdPosition(); // 새 위치 초기화
        Debug.Log("Returned to start position and reset worm.");
    }

    // 새 위치를 초기화하는 메서드
    private void ResetBirdPosition()
    {
        hasReachedWorm = false; // 중간지점 도착 여부
        hasPickedUpWorm = false; // 애벌레 위치 도착 여부
        transform.position = startFlyingPosition.position; // 시작 위치로 이동
        //birdAnimator.SetTrigger("fly"); // 초기 상태 애니메이션 설정
        if (currentWormObject != null)
        {
            currentWormObject.SetActive(true);
        }
        StopSound(); // 소리 정지
        Debug.Log("Bird position reset to start.");
    }

    // 소리 재생 메서드
    private void PlaySound(AudioClip clip, bool loop)
    {
        if (audioSource != null && clip != null) // 오디오 소스와 클립이 설정된 경우
        {
            audioSource.clip = clip; // 클립 설정
            audioSource.loop = loop; // 반복 여부 설정
            audioSource.Play(); // 소리 재생
            Debug.Log("Playing sound: " + clip.name);
        }
    }

    // 소리 정지 메서드
    private void StopSound()
    {
        if (audioSource.isPlaying) // 소리가 재생 중인 경우
        {
            audioSource.Stop(); // 소리 정지
            Debug.Log("Stopped sound.");
        }
    }

    // 새와 애벌레 충돌 이벤트 발생 시 호출되는 메서드
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter called with object: " + other.gameObject.name);  // 충돌한 오브젝트 이름 확인
        if (other.CompareTag("Worm")) // Worm 태그와 충돌
        {
            hasPickedUpWorm = true; // 애벌레를 잡은 상태로 설정
            PlaySound(countSound, false); // 소리 재생
            greenWormObject.SetActive(false); // 애벌레 오브젝트 비활성화
            Debug.Log("Worm trigger entered, picked up worm and played count sound.");
        }
        else
        {
            Debug.Log("Trigger entered but no interaction with worm. Object: " + other.name);
        }
    }
}
