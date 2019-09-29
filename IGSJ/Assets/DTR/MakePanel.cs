using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakePanel : MonoBehaviour
{
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
        panel = (PanelColor )Random.Range(0, 2);
        Top = (RowWire) Random.Range(0, 3);
        Mid = (RowWire) Random.Range(4, 7);
        Bot = (RowWire) Random.Range(8, 11);

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
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void MakeRedPanel()
    {
        if (Top == RowWire.TopR) Top = RowWire.TopN;
        if (Mid == RowWire.MidR) Mid = RowWire.MidN;
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
    }

    private void MakeGreenPanel()
    {
        if (Top == RowWire.TopG) Top = RowWire.TopN;
        if (Mid == RowWire.MidG) Mid = RowWire.MidN;
        if (Bot == RowWire.BotG) Bot = RowWire.BotN;

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
    }

    private void MakeBluePanel()
    {
        if (Top == RowWire.TopB) Top = RowWire.TopN;
        if (Mid == RowWire.MidB) Mid = RowWire.MidN;
        if (Bot == RowWire.BotB) Bot = RowWire.BotN;

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
    }
}
