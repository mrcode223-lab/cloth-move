using UnityEngine;

public class ObstacleObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.LogError("Lose");
        GameManager.Instance.ShowLosePopup();
    }
}
