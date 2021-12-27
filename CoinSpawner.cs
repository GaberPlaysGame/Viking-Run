using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public GameObject money;
    private int count = 4;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < count; i++)
        {
            Transform c = Instantiate<Transform>(money.transform);
            Transform p = transform.GetChild(Random.Range(0, transform.childCount));

            if (p.childCount > 0)
            {
                i--;
                continue;
            }
            else
            {
                c.position = p.position + new Vector3(0, 0.5f, 0);
                c.parent = p;
            }
        }
    }
}
