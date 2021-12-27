using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Money_Destroy : MonoBehaviour
{
    GameObject ui;
    public static int money = 0;
    Text go_money;

    // Start is called before the first frame update
    void Start()
    {
        ui = GameObject.Find("Canvas");

        foreach (Transform child in ui.transform)
        {
            if (child.name == "Text_money")
                go_money = child.GetComponent<Text>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter Collision");
        if (other.gameObject.CompareTag("Player"))
        {
            money++;
            go_money.text = "Money: " + money;
            Destroy(gameObject);
        }
    }
}
