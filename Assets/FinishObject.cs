using UnityEngine;

public class FinishObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.LogError("Win");
        GameManager.Instance.ShowWinPopup();
    }
}
