using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField] string loadScene;
    public void ChangeScene()
    {
        SceneManager.LoadScene(loadScene);
    }
}
