using UnityEngine;

public class WormTrigger : MonoBehaviour
{
    public BirdMovement birdMovementScript;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter called with object: " + other.gameObject.name);  // �浹�� ������Ʈ �̸� Ȯ��
        if (other.CompareTag("Worm"))
        {
            Debug.Log("Collider entered with Worm tag");  // Worm �±װ� �����Ǿ����� Ȯ��

            bool flyFromLeft = (gameObject.name == "LeftSphere");
            birdMovementScript.StartFlying(flyFromLeft);
            Debug.Log($"Triggered StartFlying from {(flyFromLeft ? "left" : "right")}.");
        }
        else
        {
            Debug.Log("Collider entered but not with Worm tag");  // �ٸ� �±׷� �浹�� �߻����� �� Ȯ��
        }
    }
}