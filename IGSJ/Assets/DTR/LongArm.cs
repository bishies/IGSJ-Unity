using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongArm : MonoBehaviour
{
    [SerializeField]
    GameObject chainPrefab, prevObj;

    [SerializeField]
    [Range(1, 100)]
    int length = 1;

    [SerializeField]
    float chainDist = 2f;

    bool snapFirst, snapLast;

    public GameObject parent1, parent2;

    GameObject[] points;


    void Start()
    {
        Spawn();
    }

    void Update()
    {
        for (int i = 0; i < points.Length-1; i++)
        {
            points[i].GetComponent<LineRenderer>().SetPosition(0, points[i].transform.position);
            points[i].GetComponent<LineRenderer>().SetPosition(1, points[i + 1].transform.position);
        }

        points[0].transform.position = parent1.transform.position;
        points[0].transform.SetParent(parent1.transform);
        points[points.Length - 1].transform.position = parent2.transform.position;
        points[points.Length - 1].transform.SetParent(parent2.transform);
    }

    public void Spawn()
    {
        int count = (int)(length / chainDist);
        points = new GameObject[count];

        for (int i = 0; i < count; i++)
        {
            GameObject temp;

            temp = Instantiate(chainPrefab, new Vector3(transform.position.x, transform.position.y + chainDist * (i + 1), transform.position.z), Quaternion.identity, prevObj.transform);
            temp.transform.eulerAngles = new Vector3(180, 0, 0);

            temp.GetComponent<Rigidbody>().maxAngularVelocity = .00001f;

            temp.name = prevObj.transform.childCount.ToString();
            Debug.Log(i);

            if (i == 0)
            {
                Destroy(temp.GetComponent<CharacterJoint>());
            }
            else
            {
                temp.GetComponent<CharacterJoint>().connectedBody = prevObj.transform.Find((prevObj.transform.childCount-1).ToString()).GetComponent<Rigidbody>();
                if(i < count - 1)
                {
                    temp.GetComponent<LineRenderer>().SetPosition(0, temp.transform.position);
                    temp.GetComponent<LineRenderer>().SetPosition(1, prevObj.transform.Find((prevObj.transform.childCount - 1).ToString()).transform.position);
                }
                else
                {
                    Destroy(temp.GetComponent<LineRenderer>());
                }
            }

            points[i] = temp;
        }

        //prevObj.transform.Find((prevObj.transform.childCount).ToString()).transform.position = parent2.transform.position;
        //prevObj.transform.Find((prevObj.transform.childCount).ToString()).SetParent(parent2.transform);
    }
}


