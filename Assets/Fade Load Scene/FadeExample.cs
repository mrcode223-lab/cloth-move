using UnityEngine;

public class FadeExample : MonoBehaviour
{
    public void Fade()
    {
        FadeLoadScene.Fade("LoadingScene", Color.black, 2f, null);
    }
}