using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class FPControlPanel : MonoBehaviour
{
    public GameObject player;

    public Color green;
    public Color red;
    public Color yellow;

    public Button[] mapButtons;
    public Slider[] switches;

    public float minSpawnTime;
    public float maxSpawnTime;

    private GameObject currPanel;
    private float spawnTime;
    private float time;

    public bool manualReady = false;
    public bool minigame = false;
    public bool gameover = false;
    public bool safe = false;
    public bool won = false;

    public int currBox = -1;
    public int fixCount = 0;
    public int requiredToWin = 6;

    public bool[] damaged;

    public GameObject[] panels;

    private Vector3 swipeInitPos;
    private Vector3 swipeEndPos;

    //0 - UP, 1 - NEUTRAL, 2 - DOWN
    public int[] switchStates;
    //0 - GREEN, 1 - RED, 2 - ACTIVATED
    public int[] buttonStatus;
    //0 - NEUTRAL, 1 - UP, 2 - DOWN
    public int[] switchSolution;

    // Start is called before the first frame update
    void Start()
    {
        showCursor();
        damaged = new bool[mapButtons.Length];
        buttonStatus = new int[mapButtons.Length];
        switchSolution = new int[switches.Length];
        switchStates = new int[switches.Length];
        zeroOut();
        for(int i = 0; i < mapButtons.Length; i++)
        {
            greenButtons(i);
        }
        redButtons(Random.Range(0, mapButtons.Length));
        randomTime();
        time = 0;
    }

    void randomTime()
    {
        spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
    }

    void greenButtons(int index)
    {
        buttonStatus[index] = 0;
        mapButtons[index].interactable = false;
        mapButtons[index].image.color = green;
        mapButtons[index].transform.GetChild(0).GetComponent<Text>().text = "GREEN";
        Debug.Log("green");
    }

    void redButtons(int index)
    {
        damaged[index] = true;
        buttonStatus[index] = 1;
        mapButtons[index].interactable = true;
        mapButtons[index].image.color = red;
        mapButtons[index].transform.GetChild(0).GetComponent<Text>().text = "RED";
        isGameOver();
        Debug.Log("red");
    }

    public void actButtons(int index)
    {
        if (!manualReady && !minigame)
        {
            buttonStatus[index] = 2;
            mapButtons[index].interactable = false;
            mapButtons[index].image.color = yellow;
            mapButtons[index].transform.GetChild(0).GetComponent<Text>().text = "ACTIVATED";
            currBox = index;
            activation();
            Debug.Log("yellow");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameover)
        {
            time += Time.deltaTime;
            //Debug.Log(time);
            if (time >= spawnTime && !safe)
            {
                List<int> canDamage = new List<int>();
                for(int i = 0; i < damaged.Length; i++)
                {
                    if (!damaged[i])
                    {
                        canDamage.Add(i);
                    }
                }
                if(canDamage.Count != 0)
                {
                    int pick = Random.Range(0, canDamage.Count);
                    Debug.Log(pick);
                    redButtons(canDamage[pick]);
                    randomTime();
                    time = 0;
                }
                else
                {
                    safe = true;
                }
            }
        }
        

        //if (panels[2].activeSelf == true)
        //{
        //    if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) { }
        //    {
        //        swipeInitPos = Input.GetTouch(0).position;
        //    }

        //    if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) { }
        //    {
        //        swipeEndPos = Input.GetTouch(0).position;
        //        Debug.Log(swipeInitPos.y + ", " + swipeEndPos.y);
        //        if(swipeEndPos.y < swipeInitPos.y)
        //        {
        //            //get button index and direction flipSwitch(index, 1)
        //        }

        //        if (swipeEndPos.y > swipeInitPos.y)
        //        {
        //            //get button index and direction flipSwitch(index, 2)
        //        }
        //    }
        //}
    }

    public void mapSelect(int buttonKey)
    {
        mapButtons[buttonKey].interactable = false;
        mapButtons[buttonKey].transform.GetChild(0).GetComponent<Text>().text = "ACTIVATED";
    }

    public void activation()
    {
        minigame = true;
        zeroOut();
        generateSolution();
        closePanel();
    }

    void zeroOut()
    {
        for(int i = 0; i < switches.Length; i++)
        {
            switches[i].value = 1;
            switchStates[i] = 1;
        }
    }
    void generateSolution()
    {
        for(int i = 0; i < switchSolution.Length; i++)
        {
            int solution = Random.Range(0,99);
            if(solution%2 == 1)
            {
                switchSolution[i] = 2;
            }
            else
            {
                switchSolution[i] = 0;
            }
            
        }
    }

    public void openMap()
    {
        currPanel = panels[0];
        currPanel.SetActive(true);
        showCursor();
    }

    public void openMinigame()
    {
        currPanel = panels[1];
        currPanel.SetActive(true);
        showCursor();
        if (!manualReady)
        {
            for (int i = 0; i < buttonStatus.Length; i++)
            {
                if (buttonStatus[i] == 2)
                {
                    panels[3].SetActive(true);
                    return;
                }
            }
        }
    }

    void showCursor()
    {
        player.GetComponent<RigidbodyFirstPersonController>().mouseLook.lockCursor = false;
    }


    public void openManual()
    {
        currPanel = panels[2];
        currPanel.SetActive(true);
        showCursor();
    }

    public void closePanel()
    {
        currPanel.SetActive(false);
    }

    public void flipSwitch(int index)
    {

        switchStates[index] = (int)switches[index].value;
        if(switchSolution[index] == switchStates[index])
        {
            switches[index].transform.GetChild(0).GetComponent<Image>().color = green;
        }
        else
        {
            switches[index].transform.GetChild(0).GetComponent<Image>().color = red;
        }

        if (checkSolution())
        {
            manualReady = true;
            panels[3].SetActive(false);
            closePanel();
        }
    }

    public bool checkSolution()
    {
        for(int i = 0; i < switches.Length; i++)
        {
            if(switchStates[i] != switchSolution[i])
            {
                return false;
            }

        }
        return true;
    }

    public void fixBox()
    {
        if(manualReady && minigame)
        {
            greenButtons(currBox);
            manualReady = false;
            minigame = false;
            fixCount++;
            if (fixCount >= requiredToWin)
            {

            }
        }
    }

    public void isGameOver()
    {
        for(int i=0; i<buttonStatus.Length; i++)
        {
            if(buttonStatus[i] == 0)
            {
                return;
            }
        }
        gameover = true;
        Debug.Log("GAME OVER");
    }
}
