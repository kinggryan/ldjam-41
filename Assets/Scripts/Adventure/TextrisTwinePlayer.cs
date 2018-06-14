using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cradle;

public class TextrisTwinePlayer : TwineTextPlayer {

    public float updateTextStepDuration = 0.09f;
    private float updateTextStepTimer = 0f;
    public float updateTextDelay = 5.5f;
    public UnityEngine.UI.Text storyText;
    string targetStoryText = "";
    private SoundEngine soundEngine;
    public float textStepDurationMultiplier = 10;

    public override void TypeCommand(string command)
    {
        targetStoryText += "\n\n<color=white>" + command + "</color>\n\n";
    }

    private void Awake() {
        updateTextStepTimer = updateTextDelay;
        soundEngine = Object.FindObjectOfType<SoundEngine>();
    }

    private void Update()
    {   
        updateTextStepTimer -= Time.deltaTime;
        if(updateTextStepTimer <= 0)
        {
            if(Input.GetKey(KeyCode.Escape)){
                updateTextStepTimer += updateTextStepDuration / textStepDurationMultiplier;
            }else{
                updateTextStepTimer += updateTextStepDuration;
                
            }
            UpdateTextStep();
        }
    }

    public void UpdateTextStep()
    {
        // Add to the story text if we can type more letters
        if(storyText.text.Length < targetStoryText.Length)
        {
            // Print the whole tag at once for rich text
            var nextChar = targetStoryText[storyText.text.Length];
            if(nextChar == '<')
            {
                while(nextChar != '>')
                {
                    storyText.text += nextChar;
                    nextChar = targetStoryText[storyText.text.Length];
                }

                // do it one more time
                storyText.text += nextChar;
                nextChar = targetStoryText[storyText.text.Length];
            }
            //Debug.Log("Adding character " + targetStoryText[storyText.text.Length]);
            storyText.text += targetStoryText[storyText.text.Length];
            if (targetStoryText[storyText.text.Length - 1] == ' '){
                //Debug.Log("SPACE");
                
            //}else if(targetStoryText[storyText.text.Length] == "\n\n"){
            }else{
                soundEngine.PlaySoundWithName("Text");
            }
        }
    }

    public override void DisplayOutput(StoryOutput output)
    {
        

        if (output is StoryText)
        {
            var text = (StoryText)output;

            // HACK: Don't knwo why the fuck this is needed but the links are being interpretted as story text so....
            foreach(var commandStr in commandToNameMap.Values)
            {
                if(commandStr.Contains(text.Text))
                {
                    return;
                }
            }
    
            //Debug.Log("Printing text for story text:" + text.Text);
            if (!string.IsNullOrEmpty(text.Text))
            {
                targetStoryText += text.Text.Replace('’', '\'');
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
        else if (output is LineBreak) { 

            targetStoryText += "\n\n";
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
