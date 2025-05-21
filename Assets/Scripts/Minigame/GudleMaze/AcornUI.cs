using TMPro;
using UnityEngine;

public class AcornUI : MonoBehaviour
{
    public TextMeshProUGUI acornText;
    private PlayerInventory inventory;

    void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
    }

    void Update()
    {
        acornText.text = "µµ≈‰∏Æ: " + inventory.acornCount;
    }
}
