using UnityEngine;

public class WormTrigger : MonoBehaviour
{
    public BirdMovement birdMovementScript;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter called with object: " + other.gameObject.name);  // 충돌한 오브젝트 이름 확인
        if (other.CompareTag("Worm"))
        {
            Debug.Log("Collider entered with Worm tag");  // Worm 태그가 감지되었는지 확인

            bool flyFromLeft = (gameObject.name == "LeftSphere");
            birdMovementScript.StartFlying(flyFromLeft);
            Debug.Log($"Triggered StartFlying from {(flyFromLeft ? "left" : "right")}.");
        }
        else
        {
            Debug.Log("Collider entered but not with Worm tag");  // 다른 태그로 충돌이 발생했을 때 확인
        }
    }
}