using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Scene_Switcher_Game : MonoBehaviour, IPointerClickHandler
{
    public int SceneDestination = 0;

    public void OnPointerClick(PointerEventData e)
    {
        Scene scene = SceneManager.GetActiveScene();

        SceneManager.LoadScene(SceneDestination);
    }
}
