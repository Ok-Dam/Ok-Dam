using UnityEngine;

public class BlockController : MonoBehaviour
{
    public float moveSpeed = 3f;
    private bool isMoving = true;
    private Vector3 direction = Vector3.right;

    private GameObject lastBlock;

    void Start()
    {
        lastBlock = GameObject.FindGameObjectsWithTag("Block").Length > 1 ?
                    GameObject.FindGameObjectsWithTag("Block")[^2] : null;
    }

    void Update()
    {
        if (isMoving)
        {
            transform.Translate(direction * moveSpeed * Time.deltaTime);

            if (transform.position.x > 3f) direction = Vector3.left;
            if (transform.position.x < -3f) direction = Vector3.right;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                isMoving = false;

                if (lastBlock != null)
                    CutBlock(lastBlock);
                else
                    GameManager.Instance.NextFloor(); // 첫 블럭은 무조건 성공
            }
        }
    }

    void CutBlock(GameObject below)
    {
        float prevX = below.transform.position.x;
        float prevWidth = below.transform.localScale.x;

        float currX = transform.position.x;
        float currWidth = transform.localScale.x;

        float prevLeft = prevX - prevWidth / 2f;
        float prevRight = prevX + prevWidth / 2f;

        float currLeft = currX - currWidth / 2f;
        float currRight = currX + currWidth / 2f;

        float maxLeft = Mathf.Max(prevLeft, currLeft);
        float minRight = Mathf.Min(prevRight, currRight);

        float overlap = minRight - maxLeft;

        if (overlap <= 0f)
        {
            GameManager.Instance.GameOver();
            Destroy(gameObject);
            return;
        }

        float newWidth = overlap;
        float newX = (maxLeft + minRight) / 2f;

        if (Mathf.Abs(overlap - prevWidth) < 0.01f)
        {
            GameManager.Instance.ShowPerfect();
        }

        transform.localScale = new Vector3(newWidth, transform.localScale.y, transform.localScale.z);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);

        GameManager.Instance.currentBlockWidth = newWidth;
        GameManager.Instance.NextFloor();
    }
}
