using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour
{
    // �� �Լ��� ��ư�� OnClick �̺�Ʈ�� ����s

    // �Ǵ� �� �̸����� �̵��ϰ� ������ �Ʒ� �Լ� ���
    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
