using UnityEngine;

public class DontDestroyOnLoadObject : MonoBehaviour
{
    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}