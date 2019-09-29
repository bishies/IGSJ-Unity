using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakePanel : MonoBehaviour
{
    public GameObject RedPanel;
    public GameObject GreenPanel;
    public GameObject BluePanel;
    private GameObject PanelInUse;

    private BatterySnap TOP_ROW;
    private BatterySnap MIDDLE_ROW;
    private BatterySnap BOTTOM_ROW;
    
    public GameObject Red_wire;
    public GameObject Green_wire;
    public GameObject Blue_wire;

    private bool isDone = false;

    enum PanelColor
    {
        Red,
        Green,
        Blue
    }
    enum RowWire
    {
        TopN, TopR, TopG, TopB,
        MidN, MidR, MidG, MidB,
        BotN, BotR, BotG, BotB
    }

    private PanelColor panel;
    private RowWire Top, Mid, Bot;
    private RowWire Top_ans, Mid_ans, Bot_ans;

    // Start is called before the first frame update
    void Start()
    {
        panel = (PanelColor) ((int) Random.Range(0, 98) / 33);
        Top = (RowWire) ((int)Random.Range(0, 99) / 25);
        Mid = (RowWire) (((int)Random.Range(0, 99) / 25) + 4);
        Bot = (RowWire) (((int)Random.Range(0, 99) / 25) + 8);

        switch(panel)
        {
            case PanelColor.Red:
                MakeRedPanel();
                break;
            case PanelColor.Green:
                MakeGreenPanel();
                break;
            case PanelColor.Blue:
                MakeBluePanel();
                break;
            default:
                print("Something bad happened when creating panel!");
                break;
        }

        switch(Top)
        {
            case RowWire.TopR:
                TOP_ROW.battery = Instantiate(Red_wire, new Vector3(0, 0, 0), Quaternion.identity);
                break;
            case RowWire.TopG:
                TOP_ROW.battery = Instantiate(Green_wire, new Vector3(0, 0, 0), Quaternion.identity);
                break;
            case RowWire.TopB:
                TOP_ROW.battery = Instantiate(Blue_wire, new Vector3(0, 0, 0), Quaternion.identity);
                break;
            case RowWire.TopN:
            default:
                TOP_ROW.battery = null;
                break;
        }

        switch (Mid)
        {
            case RowWire.MidR:
                MIDDLE_ROW.battery = Instantiate(Red_wire, new Vector3(0, 0, 0), Quaternion.identity);
                break;
            case RowWire.MidG:
                MIDDLE_ROW.battery = Instantiate(Green_wire, new Vector3(0, 0, 0), Quaternion.identity);
                break;
            case RowWire.MidB:
                MIDDLE_ROW.battery = Instantiate(Blue_wire, new Vector3(0, 0, 0), Quaternion.identity);
                break;
            case RowWire.MidN:
            default:
                MIDDLE_ROW.battery = null;
                break;
        }

        switch (Bot)
        {
            case RowWire.BotR:
                BOTTOM_ROW.battery = Instantiate(Red_wire, new Vector3(0, 0, 0), Quaternion.identity);
                break;
            case RowWire.BotG:
                BOTTOM_ROW.battery = Instantiate(Green_wire, new Vector3(0, 0, 0), Quaternion.identity);
                break;
            case RowWire.BotB:
                BOTTOM_ROW.battery = Instantiate(Blue_wire, new Vector3(0, 0, 0), Quaternion.identity);
                break;
            case RowWire.BotN:
            default:
                BOTTOM_ROW.battery = null;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isDone) return;

        if(Top == Top_ans && Mid == Mid_ans && Bot == Bot_ans)
        {
            isDone = true;
            //correct!
            // do things
            StartCoroutine(PanelComplete());
        }

        if (TOP_ROW.battery)
        {
            if(TOP_ROW.battery.tag == "RedBattery") Top = RowWire.TopR;
            else if (TOP_ROW.battery.tag == "GreenBattery") Top = RowWire.TopG;
            else if (TOP_ROW.battery.tag == "BlueBattery") Top = RowWire.TopB;
        }
        else Top = RowWire.TopN;

        if (MIDDLE_ROW.battery)
        {
            if (MIDDLE_ROW.battery.tag == "RedBattery") Mid = RowWire.MidR;
            else if (MIDDLE_ROW.battery.tag == "GreenBattery") Mid = RowWire.MidG;
            else if (MIDDLE_ROW.battery.tag == "BlueBattery") Mid = RowWire.MidB;
        }
        else Mid = RowWire.MidN;

        if (BOTTOM_ROW.battery)
        {
            if (BOTTOM_ROW.battery.tag == "RedBattery") Bot = RowWire.BotR;
            else if (BOTTOM_ROW.battery.tag == "GreenBattery") Bot = RowWire.BotG;
            else if (BOTTOM_ROW.battery.tag == "BlueBattery") Bot = RowWire.BotB;
        }
        else Bot = RowWire.BotN;
    }

    private void MakeRedPanel()
    {
        if (Top == RowWire.TopR) Top = RowWire.TopB;
        if (Mid == RowWire.MidR) Mid = RowWire.MidG;
        if (Bot == RowWire.BotR) Bot = RowWire.BotN;

        // If there are no wires; place red wire in the middle row
        if (Top == RowWire.TopN &&
            Mid == RowWire.MidN &&
            Bot == RowWire.BotN)
        {
            Top_ans = RowWire.TopN;
            Mid_ans = RowWire.MidR;
            Bot_ans = RowWire.BotN;
        }
        // If blue wire in top row; remove any bottom row wire and place red wire in bottom row
        else if (Top == RowWire.TopB)
        {
            Top_ans = RowWire.TopB;
            Mid_ans = Mid;
            Bot_ans = RowWire.BotR;
        }
        // Otherwise; remove all wires, place red wire in top row
        else
        {
            Top_ans = RowWire.TopR;
            Mid_ans = RowWire.MidN;
            Bot_ans = RowWire.BotN;
        }

        // set which panel to show
        RedPanel.SetActive(true);
        GreenPanel.SetActive(false);
        BluePanel.SetActive(false);
        PanelInUse = RedPanel;

        TOP_ROW = RedPanel.transform.GetChild(0).gameObject.GetComponent<BatterySnap>();
        MIDDLE_ROW = RedPanel.transform.GetChild(1).gameObject.GetComponent<BatterySnap>();
        BOTTOM_ROW = RedPanel.transform.GetChild(2).gameObject.GetComponent<BatterySnap>();        
    }

    private void MakeGreenPanel()
    {
        if (Top == RowWire.TopG) Top = RowWire.TopR;
        if (Mid == RowWire.MidG) Mid = RowWire.MidB;
        if (Bot == RowWire.BotG) Bot = RowWire.BotB;

        // If red wire in middle row; remove all wires and place green wire in top row
        if (Mid == RowWire.MidR)
        {
            Top_ans = RowWire.TopG;
            Mid_ans = RowWire.MidN;
            Bot_ans = RowWire.BotN;
        }
        // Otherwise, if blue wire in middle row; remove all blue wires and place green wire in middle row
        else if (Mid == RowWire.MidB)
        {
            if (Top == RowWire.TopB)
            {
                Top_ans = RowWire.TopN;
            }
            else Top_ans = Top;

            Mid_ans = RowWire.MidG;

            if (Bot == RowWire.BotB)
            {
                Bot_ans = RowWire.BotN;
            }
            else Bot_ans = Bot;
        }
        // Otherwise; remove any bottom row wire and place green wire in bottom row
        else
        {
            Top_ans = Top;
            Mid_ans = Mid;
            Bot_ans = RowWire.BotG;
        }

        // set which panel to show
        RedPanel.SetActive(false);
        GreenPanel.SetActive(true);
        BluePanel.SetActive(false);
        PanelInUse = GreenPanel;

        TOP_ROW = GreenPanel.transform.GetChild(0).gameObject.GetComponent<BatterySnap>();
        MIDDLE_ROW = GreenPanel.transform.GetChild(1).gameObject.GetComponent<BatterySnap>();
        BOTTOM_ROW = GreenPanel.transform.GetChild(2).gameObject.GetComponent<BatterySnap>();
    }

    private void MakeBluePanel()
    {
        if (Top == RowWire.TopB) Top = RowWire.TopN;
        if (Mid == RowWire.MidB) Mid = RowWire.MidG;
        if (Bot == RowWire.BotB) Bot = RowWire.BotR;

        // If there is exactly one red wire; remove it and replace it with a blue wire
        if (Top == RowWire.TopR && Mid != RowWire.MidR && Bot != RowWire.BotR)
        {
            Top_ans = RowWire.TopB;
            Mid_ans = Mid;
            Bot_ans = Bot;
        }
        else if (Top != RowWire.TopR && Mid == RowWire.MidR && Bot != RowWire.BotR)
        {
            Top_ans = Top;
            Mid_ans = RowWire.MidB;
            Bot_ans = Bot;
        }
        else if (Top != RowWire.TopR && Mid != RowWire.MidR && Bot == RowWire.BotR)
        {
            Top_ans = Top;
            Mid_ans = Mid;
            Bot_ans = RowWire.BotB;
        }
        // If there is exactly one green wire; remove any other wires and then place blue wires in all empty rows
        else if (Top == RowWire.TopG && Mid != RowWire.MidG && Bot != RowWire.BotG)
        {
            Top_ans = RowWire.TopG;
            Mid_ans = RowWire.MidB;
            Bot_ans = RowWire.BotB;
        }
         else if (Top != RowWire.TopG && Mid == RowWire.MidG && Bot != RowWire.BotG)
        {
            Top_ans = RowWire.TopB;
            Mid_ans = RowWire.MidG;
            Bot_ans = RowWire.BotB;
        }
        else if (Top != RowWire.TopG && Mid != RowWire.MidG && Bot == RowWire.BotG)
        {
            Top_ans = RowWire.TopB;
            Mid_ans = RowWire.MidB;
            Bot_ans = RowWire.BotG;
        }
        // Otherwise; remove any bottom wire and place blue wire in bottom row
        else
        {
            Top_ans = Top;
            Mid_ans = Mid;
            Bot_ans = RowWire.BotB;
        }

        // set which panel to show
        RedPanel.SetActive(false);
        GreenPanel.SetActive(false);
        BluePanel.SetActive(true);
        PanelInUse = BluePanel;

        TOP_ROW = BluePanel.transform.GetChild(0).gameObject.GetComponent<BatterySnap>();
        MIDDLE_ROW = BluePanel.transform.GetChild(1).gameObject.GetComponent<BatterySnap>();
        BOTTOM_ROW = BluePanel.transform.GetChild(2).gameObject.GetComponent<BatterySnap>();        
    }

    IEnumerator PanelComplete()
    {
        PanelInUse.transform.GetChild(3).gameObject.GetComponent<TextMesh>().text = "DONE!";
        PanelInUse.transform.GetChild(3).gameObject.GetComponent<TextMesh>().color = Color.black;
        PanelInUse.transform.GetChild(4).gameObject.SetActive(false);
        yield return new WaitForSeconds(5);
        PanelInUse.SetActive(false);
    }
}
