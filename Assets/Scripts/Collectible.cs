using UnityEngine;

public class Collectible : MonoBehaviour
{
    public int amount = 1;                 // How much resource it gives
    public string resourceType;            // Must match a field name in PlayerResources

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null)
        {

            var resources = ResourceManager.Instance.players[player.playerId];
            resources[resourceType] += amount;

            Debug.Log($"Player {player.playerId}:  {resourceType }{resources[resourceType]}");

            Destroy(gameObject);
        }
    }
}
