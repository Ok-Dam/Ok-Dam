using UnityEngine;

public class TreeCutter : MonoBehaviour
{
    public GameObject treeWhole;
    public GameObject treeTop;
    public GameObject treeBottom;

    void Start()
    {
        treeTop.SetActive(false);
        treeBottom.SetActive(false);
    }

    public void CutTree()
    {
        treeWhole.SetActive(false);
        treeTop.SetActive(true);
        treeBottom.SetActive(true);

        Rigidbody rb = treeTop.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }
    }
}
