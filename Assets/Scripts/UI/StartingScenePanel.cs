using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class  StartingScenePanel : MonoBehaviour
{
    public void ToCharSelectionScreen()
    {
        SceneManager.LoadScene(1);
    }
    
}
