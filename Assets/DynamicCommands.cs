using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cradle;
using UnityEngine.UI;

//Prints Current Available Commands as Text
public class DynamicCommands : MonoBehaviour
{
    public IEnumerable<Cradle.StoryLink> links;
    public TextrisTwinePlayer twinePlayer;
    public Cradle.StoryVar items;
    public Text commandText;

    // Use this for initialization
    void Start()
    {
        twinePlayer = Object.FindObjectOfType<TextrisTwinePlayer>();
        links = twinePlayer.Story.GetCurrentLinks();
        items = twinePlayer.Story.Vars["inv"];
    }

    // Update is called once per frame
    void Update()
    {
        commandText.text = "CMD:/";
        PrintCommands();
        commandText.text += "\n INV:/";
        PrintInv();

    }

    void PrintCommands()
    {
        links = twinePlayer.Story.GetCurrentLinks();
        foreach (var link in links)
        {
            var text = link.Text;

            //Longer commands like "GET COAT" do not enter TETRIS space
            if (text.Contains(" "))
            {
                string[] words = text.Split(null);
                commandText.text += "\n" + words[0];
                commandText.text += "\n" + words[1];
            }
            else
            {
                commandText.text += "\n" + text;
            }
        }
    }

    void PrintInv()
    {
        items = twinePlayer.Story.Vars["inv"];

        if (items["NOTE"] == true)
        {
            commandText.text += "\n NOTE";
        }
        if (items["COAT"] == true)
        {
            commandText.text += "\n COAT";
        }
        if (items["GUN"] == true)
        {
            commandText.text += "\n GUN";
        }
        if (items["FOB"] == true)
        {
            commandText.text += "\n FOB";
        }
        if (items["CURE"] == true)
        {
            commandText.text += "\n CURE";
        }

    }
}
