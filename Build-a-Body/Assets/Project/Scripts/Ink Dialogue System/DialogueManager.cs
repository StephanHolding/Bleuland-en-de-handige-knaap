using Dialogue.Blackboard;
using FMOD_AudioManagement;
using Ink.Runtime;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Dialogue
{
    public class DialogueManager : SingletonTemplateMono<DialogueManager>
    {
        public abstract class DialogueCommandBase
        {
            public string id;

            protected DialogueCommandBase(string id)
            {
                this.id = id;
            }

            public abstract void Invoke(string arg = "");
        }
        public class DialogueCommand<T> : DialogueCommandBase
        {
            public Action<T> command;

            public DialogueCommand(string id, Action<T> command) : base(id)
            {
                this.command = command;
            }

            public override void Invoke(string arg = "")
            {
                command.Invoke((T)ParseStringArgument(arg, typeof(T)));
            }

            private object ParseStringArgument(string arg, Type type)
            {
                if (type == typeof(int))
                {
                    return int.Parse(arg);
                }
                else if (type == typeof(float))
                {
                    arg = arg.Replace('.', ',');
                    return float.Parse(arg);
                }
                else if (type == typeof(bool))
                {
                    return bool.Parse(arg);
                }

                return arg;
            }
        }
        public class DialogueQueue
        {
            public abstract class DialogueWrapperBase
            {
                public event DialogueEvent OnDialogueSequenceEnded;

                protected DialogueWrapperBase(DialogueEvent onDialogueSequenceEnded)
                {
                    if (onDialogueSequenceEnded != null)
                        this.OnDialogueSequenceEnded += onDialogueSequenceEnded;
                }

                public abstract bool CanContinue();
                public abstract bool HasEnded();
                public abstract string Continue();

                public void CallEndEvent()
                {
                    OnDialogueSequenceEnded?.Invoke();
                }
            }
            public class InkDialogueWrapper : DialogueWrapperBase
            {
                public Story InkStory { get; private set; }

                public InkDialogueWrapper(TextAsset story, DialogueEvent onDialogueSequenceEnded = null) : base(onDialogueSequenceEnded)
                {
                    InkStory = new Story(story.text);
                }

                public void MakeChoice(int choiceIndex)
                {
                    InkStory.ChooseChoiceIndex(choiceIndex);
                }

                public override bool CanContinue()
                {
                    return InkStory.canContinue;
                }

                public override bool HasEnded()
                {
                    return !InkStory.canContinue && InkStory.currentChoices.Count == 0;
                }

                public override string Continue()
                {
                    return InkStory.Continue();
                }
            }
            public class CustomDialogueWrapper : DialogueWrapperBase
            {

                public Queue<string> toSay = new Queue<string>();

                public CustomDialogueWrapper(string[] dialogue, DialogueEvent onDialogueSequenceEnded = null) : base(onDialogueSequenceEnded)
                {
                    foreach (string str in dialogue)
                    {
                        toSay.Enqueue(str);
                    }
                }

                public override string Continue()
                {
                    if (toSay.Count > 0)
                    {
                        return toSay.Dequeue();
                    }
                    else
                    {
                        return null;
                    }
                }

                public override bool CanContinue()
                {
                    return toSay.Count > 0;
                }

                public override bool HasEnded()
                {
                    return toSay.Count == 0;
                }
            }



            public DialogueWrapperBase currentDialogue;
            private Queue<DialogueWrapperBase> queue = new Queue<DialogueWrapperBase>();

            public void AddToQueue(DialogueWrapperBase toEnqueue)
            {
                queue.Enqueue(toEnqueue);
            }

            public bool Dequeue()
            {
                currentDialogue?.CallEndEvent();

                if (queue.Count > 0)
                {
                    currentDialogue = queue.Dequeue();
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public void MakeChoice(int choiceIndex)
            {
                if (currentDialogue is InkDialogueWrapper inkStory)
                {
                    inkStory.MakeChoice(choiceIndex);
                }
            }

            public void End()
            {
                currentDialogue = null;
                queue.Clear();
            }

        }

        public TextAsset[] stories;
        public bool playFirstStoryOnStart = true;
        public DialogueOutput output;

        public delegate void DialogueEvent();
        public event DialogueEvent StoryQueueEnded;

        public bool MayContinueStory { get; private set; }

        private bool dialogueReadingActive;
        private DialogueQueue dialogueQueue = new DialogueQueue();
        private Dictionary<string, DialogueCharacter> allCharacters;
        private Dictionary<string, DialogueCommandBase> allCommands;

        protected override void Awake()
        {
            base.Awake();
            if (output == null)
                FindOutput();


            AddAllCommands();
            LoadCharacters();
        }

        private void Start()
        {
            if (playFirstStoryOnStart)
            {
                StartNewStory(0);
            }
        }

        private void Update()
        {
            //maybe update to new input system?
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ContinueStory();
            }
        }

        public void LockStory()
        {
            MayContinueStory = false;
        }

        public void UnlockStory()
        {
            MayContinueStory = true;
        }

        public void Say(string[] lines, DialogueEvent onDialogueSequenceEnded = null)
        {
            dialogueQueue.AddToQueue(new DialogueQueue.CustomDialogueWrapper(lines, onDialogueSequenceEnded));
            StartDialogue();
        }

        public void Say(string line, DialogueEvent onDialogueSequenceEnded = null)
        {
            Say(new[] { line }, onDialogueSequenceEnded);
        }

        public void Say(TextAsset story, DialogueEvent onDialogueSequenceEnded = null)
        {
            dialogueQueue.AddToQueue(new DialogueQueue.InkDialogueWrapper(story, onDialogueSequenceEnded));
            StartDialogue();
        }

        public static TextAsset LoadStoryFromResources(string assetName)
        {
            string path = "Ink Files";

            switch (LocalizationSettings.SelectedLocale.Identifier.Code)
            {
                case "en":
                    path += "/English/";
                    break;
                case "nl":
                    path += "/Dutch/";
                    break;
                default:
                    Debug.Log("Locale not found " + LocalizationSettings.SelectedLocale.Identifier.Code);
                    break;

            }

            path += assetName;

            return Resources.Load<TextAsset>(path);
        }

        public void MakeChoice(int choiceIndex)
        {
            dialogueQueue.MakeChoice(choiceIndex);
            ContinueStory();
        }

        private void FindOutput()
        {
            output = FindObjectOfType<DialogueOutput>();
        }

        private void LoadCharacters()
        {
            allCharacters = new Dictionary<string, DialogueCharacter>();
            DialogueCharacter[] characters = Resources.LoadAll<DialogueCharacter>("Dialogue Characters");
            foreach (DialogueCharacter character in characters)
            {
                allCharacters.Add(character.name, character);
            }

        }

        private void AddAllCommands()
        {
            allCommands = new Dictionary<string, DialogueCommandBase>();
            AddCommand(new DialogueCommand<string>("character", delegate (string characterName)
            {
                output.SetCharacter(GetCharacter(characterName));
            }));
            AddCommand(new DialogueCommand<int>("talkingspeed", delegate (int textSpeed)
            {
                output.dialogueOverrides.talkingSpeed = textSpeed;
            }));
            AddCommand(new DialogueCommand<string>("inputfield", delegate (string variableName)
            {
                //Story is locked from DialogueInput class

                output.ActivateInputfield(delegate (string arg0)
                {
                    DialogueBlackboard.SetVariable(variableName, arg0);
                });
            }));
            AddCommand(new DialogueCommand<string>("loadscene", delegate (string sceneName)
            {
                SceneHandler.instance.LoadScene(sceneName);
            }));
            AddCommand(new DialogueCommand<string>("graphic", delegate (string spriteName)
            {
                if (spriteName == "false")
                {
                    output.DisableBackgroundGraphic();
                }
                else
                {
                    output.SetBackgroundGraphic(spriteName);
                }
            }));
            AddCommand(new DialogueCommand<float>("delay", delegate (float delay)
            {
                output.SetDelay(delay);
            }));
            AddCommand(new DialogueCommand<string>("playoneshot", delegate (string soundName)
            {
                if (FMODAudioManager.instance != null)
                {
                    FMODAudioManager.instance.PlayOneShot(soundName);
                }
            }));
            AddCommand(new DialogueCommand<string>("playmusic", delegate (string musicName)
            {
                if (FMODAudioManager.instance != null)
                {
                    FMODAudioManager.instance.Play(musicName);
                }
            }));
            AddCommand(new DialogueCommand<string>("stopAllMusic", delegate
            {
                if (FMODAudioManager.instance != null)
                {
                    FMODAudioManager.instance.StopAll();
                }
            }));
            AddCommand(new DialogueCommand<string>("camera", delegate (string targetTag)
            {
                Camera.main.transform.GetComponent<CameraMovementController>()?.GoTo(targetTag);
            }));
            AddCommand(new DialogueCommand<string>("sprite", delegate (string spriteName)
            {
                output.ShowSprite(spriteName);
            }));
        }

        private void AddCommand(DialogueCommandBase toAdd)
        {
            allCommands.Add(toAdd.id, toAdd);
        }

        public void ContinueStory()
        {
            if (!output.IsWriting)
            {
                if (MayContinueStory)
                {
                    if (dialogueQueue.currentDialogue != null)
                    {
                        if (dialogueQueue.currentDialogue.CanContinue())
                        {
                            string nextLine = dialogueQueue.currentDialogue.Continue();

                            if (dialogueQueue.currentDialogue is DialogueQueue.InkDialogueWrapper inkWrapper)
                            {
                                Story inkStory = inkWrapper.InkStory;

                                //take care of any tags that come with this line of dialogue
                                if (inkStory.currentTags.Count > 0)
                                {
                                    HandleInkTags(inkStory.currentTags);
                                }

                                //if there are choices, show them on screen
                                if (inkStory.currentChoices.Count > 0)
                                {
                                    output.ShowChoiceUI(inkStory.currentChoices, this);
                                }

                                if (nextLine.Contains('[') && nextLine.Contains(']'))
                                {
                                    nextLine = HandleBlackboardTags(nextLine);
                                }
                            }

                            output.StartWriting(nextLine);

                        }
                        else if (dialogueQueue.currentDialogue.HasEnded()) //when we cannot continue and there are also no choices to make, we must have reached the end of dialogue.
                        {
                            if (dialogueQueue.Dequeue())
                            {
                                ContinueStory();
                            }
                            else
                            {
                                StoryQueueEnded?.Invoke();
                                dialogueQueue.End();
                                dialogueReadingActive = false;
                                output.Toggle(false);
                            }
                        }
                    }
                }
            }
            else
            {
                output.SkipAnimation();
            }
        }

        public void StartDialogue()
        {
            if (!dialogueReadingActive)
            {
                dialogueReadingActive = true;

                if (dialogueQueue.Dequeue())
                {
                    output.HideChoiceUI();
                    output.Toggle(true);
                    UnlockStory();
                    ContinueStory();
                }
            }
        }

        public void StartNewStory(int storyIndex)
        {
            Say(stories[storyIndex]);
        }

        public void StartNewStory(string storyName)
        {
            for (int i = 0; i < stories.Length; i++)
            {
                if (stories[i].name == storyName)
                {
                    StartNewStory(i);
                    return;
                }
            }
        }

        private void HandleInkTags(List<string> tags)
        {
            foreach (string inkTag in tags)
            {
                string usingTag = inkTag;

                if (usingTag.Contains('[') && usingTag.Contains(']'))
                {
                    usingTag = HandleBlackboardTags(usingTag);
                }

                string[] args = usingTag.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (allCommands.ContainsKey(args[0]))
                {
                    DialogueCommandBase command = allCommands[args[0]];

                    if (args.Length > 1)
                        command.Invoke(args[1]);
                    else
                        command.Invoke();
                }
            }
        }

        private string HandleBlackboardTags(string line)
        {
            int tagStart = line.IndexOf('[') + 1;
            int tagEnd = line.IndexOf(']') - 1;
            string tag = line.Substring(tagStart, (tagEnd - tagStart) + 1);
            string info = DialogueBlackboard.GetVariable<string>(tag);
            string toReturn = line.Replace("[" + tag + "]", info);
            return toReturn;
        }

        private DialogueCharacter GetCharacter(string characterName)
        {
            return allCharacters[characterName];
        }
    }
}