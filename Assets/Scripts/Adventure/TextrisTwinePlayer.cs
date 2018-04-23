using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cradle;

public class TextrisTwinePlayer : TwineTextPlayer {

    public float updateTextStepDuration = 0.09f;
    public float updateTextStepTimer = 0f;
    public UnityEngine.UI.Text storyText;
    string targetText = "";

    public override void TypeCommand(string command)
    {
        targetText += command + "\n\n";
    }

    private void Update()
    {
        updateTextStepTimer -= Time.deltaTime;
        if(updateTextStepTimer <= 0)
        {
            updateTextStepTimer += updateTextStepDuration;
            UpdateTextStep();
        }
    }

    public void UpdateTextStep()
    {
        // Add to the story text if we can type more letters
        if(storyText.text.Length < targetText.Length)
        {
            //Debug.Log("Adding character " + targetText[storyText.text.Length]);
            storyText.text += targetText[storyText.text.Length];
        }
    }

    public override void DisplayOutput(StoryOutput output)
    {
        Debug.Log("Doing output: " + output.Text);
        if (output is StoryText)
        {
            var text = (StoryText)output;
            if (!string.IsNullOrEmpty(text.Text))
            {
                targetText += text.Text.Replace('’', '\'');
            }
        }
        else if (output is StoryLink)
        {
            //var link = (StoryLink)output;
            //if (!ShowNamedLinks && link.IsNamed)
            //    return;

            //Button uiLink = (Button)Instantiate(LinkTemplate);
            //uiLink.gameObject.SetActive(true);
            //uiLink.name = "[[" + link.Text + "]]";

            //Text uiLinkText = uiLink.GetComponentInChildren<Text>();
            //uiLinkText.text = link.Text;
            //uiLink.onClick.AddListener(() =>
            //{
            //    this.Story.DoLink(link);
            //});
            //AddToUI((RectTransform)uiLink.transform, output, uiInsertIndex);

        }
        else if (output is LineBreak)
        {
            targetText += "\n\n";
        }
        //else if (output is OutputGroup)
        //{
        //    // Add an empty indicator to later positioning
        //    var groupMarker = new GameObject();
        //    groupMarker.name = output.ToString();
        //    AddToUI(groupMarker.AddComponent<RectTransform>(), output, uiInsertIndex);
        //}
    }
}
