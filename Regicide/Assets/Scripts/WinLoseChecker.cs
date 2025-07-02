using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinLoseChecker : MonoBehaviour
{
    private DangerResourceManager drm;
    void Start()
    {
        drm = FindObjectOfType<DangerResourceManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (drm.VassalNumber <= 0)
        {
            SceneManager.LoadScene("LoseScene");
        }

        if (drm.CannonCompletion == 100)
        {
            SceneManager.LoadScene("WinScene");
        }
    }
}
