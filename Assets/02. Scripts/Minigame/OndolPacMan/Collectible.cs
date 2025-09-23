using UnityEngine;

public class Collectible : MonoBehaviour, IPlayerInteractable
{
    public void OnPlayerInteract(GameObject player)
    {
        // Notify GameManager or CollectibleManager about this collection
        pacGameManager.Instance.CollectibleCollected();

        // Disable or destroy collectible
        gameObject.SetActive(false);
    }
}
