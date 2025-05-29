using UnityEngine;

public class YutnoriManager : MonoBehaviour
{
    public GameObject yutnoriRootPrefab; // ¿∑≥Ó¿Ã ∑Á∆Æ «¡∏Æ∆’
    private GameObject yutnoriRootInstance; // «ˆ¿Á ¿ŒΩ∫≈œΩ∫

    public void StartYutnori()
    {
        if (yutnoriRootInstance != null)
            Destroy(yutnoriRootInstance);

        yutnoriRootInstance = Instantiate(yutnoriRootPrefab);
    }

    public void EndYutnori()
    {
        if (yutnoriRootInstance != null)
        {
            Destroy(yutnoriRootInstance);
            yutnoriRootInstance = null;
        }
    }
}
