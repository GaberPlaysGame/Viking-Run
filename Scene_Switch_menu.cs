using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Scene_Switch_menu : MonoBehaviour, IPointerClickHandler
{
    public int SceneDestination = 1;

    public void OnPointerClick(PointerEventData e)
    {
        Scene scene = SceneManager.GetActiveScene();

        SceneManager.LoadScene(SceneDestination);
    }
}
