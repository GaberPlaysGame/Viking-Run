using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_Scene : MonoBehaviour
{
    private static int scene_count = 2;
    private static int chunk;
    private GameObject player;
    private GameObject scene;
    private static Transform t;
    Vector3 shift;

    private float[,] parameters = { 
        { 13.7f, -1.6f, 2.39f },
        { 14.7f, -1.6f, -7.5f },
        { 14.7f, -1.6f, -7.5f },
        { 17.7f, -1.6f, 2.39f },
        { 13.7f, -1.6f, 2.39f },
        { 13.7f, -1.6f, 2.39f },
        { 13.7f, -1.6f, 2.39f }
    };

    void CreateSceneObjectOnStart()
    {
        while (true) { 
            Debug.Log("Entered loop, queue count: " + Platform_Queue.Count());

            if (Platform_Queue.Count() == 1)
            {
                t = transform;
                CreateSceneObject(t);
            }
            else if (Platform_Queue.Count() <= 3)
            {
                CreateSceneObject(t);
            }
            else
                break;
        }
    }
    void CreateSceneObject(Transform trans)
    {
        Debug.Log("Trigger entered");
        chunk = Random.Range(0, 10);
        string chunk_name = "Platform_b_type1";
        switch (chunk)
        {
            case 1:
                chunk_name = "Platform_b_type2";
                break;
            case 2:
                chunk_name = "Platform_b_type3";
                break;
            case 3:
            case 4:
                chunk = 3;
                chunk_name = "Platform_b_type4";
                break;
            case 5:
            case 6:
                chunk = 4;
                chunk_name = "Platform_b_type5";
                break;
            case 7:
            case 8:
                chunk = 5;
                chunk_name = "Platform_b_type6";
                break;
            case 9:
                chunk = 6;
                chunk_name = "Platform_b_type7";
                break;
            default:
                chunk = 0;
                chunk_name = "Platform_b_type1";
                break;
        }
        int sign_x = 1, sign_z = 1, para_x = 0, para_z = 2;
        switch (t.eulerAngles.y)
        {
            case 0:
                Debug.Log("switch entered 0");
                sign_x = 1; 
                sign_z = 1;
                para_x = 0;
                para_z = 2;
                break;
            case 90:
                Debug.Log("switch entered 90");
                sign_x = 1;
                sign_z = -1;
                para_x = 2;
                para_z = 0;
                break;
            case 180:
                Debug.Log("switch entered 180");
                sign_x = -1;
                sign_z = -1;
                para_x = 0;
                para_z = 2;
                break;
            case 270:
                Debug.Log("switch entered 270");
                sign_x = -1;
                sign_z = 1;
                para_x = 2;
                para_z = 0;
                break;
            default:
                Debug.Log("switch entered nothing: " + (t.eulerAngles.y));
                break;
        }

        shift = new Vector3(sign_x*parameters[chunk, para_x], parameters[chunk, 1], sign_z*parameters[chunk, para_z]);
        //shift = new Vector3(sign_x * parameters[3, para_x], parameters[3, 1], sign_z * parameters[3, para_z]);

        //GameObject prefab = (GameObject)Resources.Load("Customize/Models/Environment/Platform_b_type4", typeof(GameObject));
        GameObject prefab = (GameObject)Resources.Load("Customize/Models/Environment/"+ chunk_name, typeof(GameObject));
        var newObject = Instantiate(prefab, trans.position + shift, trans.rotation);
        
        newObject.transform.parent = scene.transform;

        t = newObject.transform.GetChild(0);

        Platform_Queue.Push(newObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (Platform_Queue.Count() == 0)
        {
            GameObject start = GameObject.Find("Platform_start");
            Platform_Queue.Push(start);
        }
        scene = GameObject.Find("Scene");
        CreateSceneObjectOnStart();
        player = GameObject.FindWithTag("Player");
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CreateSceneObject(t);
            if (Platform_Queue.Count() >= 7)
            {
                GameObject pre = Platform_Queue.Pop();
                Destroy(pre);
            }
        }
    }
}
