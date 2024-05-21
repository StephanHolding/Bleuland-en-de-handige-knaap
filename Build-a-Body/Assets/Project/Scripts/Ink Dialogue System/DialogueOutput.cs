using FMOD.Studio;
using FMOD_AudioManagement;
using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Dialogue
{
    public class DialogueOutput : MonoBehaviour
    {

        public TextMeshProUGUI uiText;
        public TextMeshProUGUI characterNameText;
        public Image spriteOutput;
        public Button[] choiceButtons;
        public TMP_InputField inputfield;
        public Image graphicSpace;

        //other classes are allowed to make changes to this struct. it is reset after every dialogue line.
        public DialogueCharacter.DialogueCharacterSettings dialogueOverrides;
        public bool IsWriting { get; private set; }

        private string currentlyWriting;
        private float delayForCurrentLine = 0;
        private bool canSkip = true;
        private DialogueCharacter currentCharacter;
        private Button inputfieldSubmitButton;
        private EventInstance talkingSoundReference;
        private bool isActive;

        private void Awake()
        {
            if (inputfield != null)
                inputfieldSubmitButton = inputfield.transform.GetChild(0).GetComponent<Button>();

            AddButtonSoundToButtons();
        }

        public void Toggle(bool toggle)
        {
            if (isActive != toggle)
            {
                transform.GetChild(0).gameObject.SetActive(toggle);

                uiText.transform.parent.gameObject.SetActive(toggle);
                characterNameText.transform.parent.gameObject.SetActive(toggle);

                if (inputfield != null)
                    inputfield.gameObject.SetActive(false);

                DisableBackgroundGraphic();
                HideChoiceUI();

                isActive = toggle;
            }
        }

        public void SkipAnimation()
        {
            if (canSkip)
            {
                StopAllCoroutines();
                uiText.text = currentlyWriting;
                IsWriting = false;
                ResetOverrides();
            }
        }

        public void SetCharacter(DialogueCharacter character)
        {
            print(character.name);
            currentCharacter = character;
            dialogueOverrides = currentCharacter.characterSettings; //characterSettings is a struct, so this works.
        }

        public void ActivateInputfield(UnityAction<string> onEndEdit)
        {
            inputfield.text = string.Empty;
            DialogueManager.instance.LockStory();

            inputfieldSubmitButton.onClick.RemoveAllListeners();
            inputfieldSubmitButton.onClick.AddListener(delegate
            {
                if (!string.IsNullOrWhiteSpace(inputfield.text))
                {
                    onEndEdit.Invoke(inputfield.text);
                    inputfield.gameObject.SetActive(false);

                    DialogueManager.instance.UnlockStory();
                    DialogueManager.instance.ContinueStory();
                }
            });

            inputfieldSubmitButton.onClick.AddListener(delegate
            {
                FMODAudioManager.instance.PlayOneShot("OK");
            });

            inputfield.gameObject.SetActive(true);

        }

        private void ResetOverrides()
        {
            dialogueOverrides = currentCharacter.characterSettings;
            delayForCurrentLine = 0;
        }

        public void StartWriting(string fullLine)
        {
            Toggle(true);

            IsWriting = true;
            currentlyWriting = fullLine;
            uiText.text = string.Empty;
            characterNameText.text = dialogueOverrides.displayName;

            if (spriteOutput != null)
                spriteOutput.sprite = dialogueOverrides.characterSprite;

            uiText.font = dialogueOverrides.fontAsset;

            StartCoroutine(WritingAnimation());
        }

        public void ShowChoiceUI(List<Choice> choices, DialogueManager managerInstance)
        {
            for (int i = 0; i < choices.Count; i++)
            {
                int index = i;
                Button b = choiceButtons[i];
                b.gameObject.SetActive(true);
                b.transform.GetComponentInChildren<TextMeshProUGUI>().text = choices[i].text;
                b.onClick.AddListener(delegate
                {
                    managerInstance.MakeChoice(index);
                    HideChoiceUI();
                });

                b.onClick.AddListener(delegate
                {
                    FMODAudioManager.instance.PlayOneShot("OK");
                });
            }
        }

        public void HideChoiceUI()
        {
            foreach (Button b in choiceButtons)
            {
                b.onClick.RemoveAllListeners();
                b.gameObject.SetActive(false);
            }
        }

        public void SetDelay(float delay)
        {
            delayForCurrentLine = delay;
        }

        public void ContinueStory()
        {
            DialogueManager.instance.ContinueStory();
        }

        private IEnumerator WritingAnimation()
        {
            float waitTime = 1 / dialogueOverrides.talkingSpeed;

            if (delayForCurrentLine != 0)
            {
                canSkip = false;
                DialogueManager.instance.LockStory();
                print("waiting for " + delayForCurrentLine + " seconds");
                yield return new WaitForSeconds(delayForCurrentLine);
                DialogueManager.instance.UnlockStory();
                canSkip = true;
            }


            for (int i = 0; i < currentlyWriting.Length; i++)
            {
                char toAdd = currentlyWriting[i];

                uiText.text += toAdd;

                if (dialogueOverrides.playSoundEachCharacter)
                {
                    if (toAdd != ' ')
                    {
                        if (!string.IsNullOrEmpty(dialogueOverrides.talkingSound))
                            FMODAudioManager.instance.PlayOneShot(dialogueOverrides.talkingSound);
                    }
                }
                else
                {
                    if (!FMODAudioManager.IsPlaying(talkingSoundReference))
                    {
                        if (!string.IsNullOrEmpty(dialogueOverrides.talkingSound))
                            talkingSoundReference = FMODAudioManager.instance.Play(dialogueOverrides.talkingSound);
                    }
                }



                yield return new WaitForSeconds(waitTime);
            }

            IsWriting = false;
            ResetOverrides();
        }

        public void SetBackgroundGraphic(string spriteName)
        {
            Sprite sprite = Resources.Load<Sprite>("Dynamic Images/" + spriteName);
            SetBackgroundGraphic(sprite);
        }

        public void SetBackgroundGraphic(Sprite sprite)
        {
            graphicSpace.sprite = sprite;
            graphicSpace.preserveAspect = true;
            graphicSpace.gameObject.SetActive(true);
        }

        public void DisableBackgroundGraphic()
        {
            if (graphicSpace != null)
                graphicSpace.gameObject.SetActive(false);
        }

        public void InputfieldOnInputChangedSoundEffect()
        {
            FMODAudioManager.instance.PlayOneShot("writing sound");
        }

        private void AddButtonSoundToButtons()
        {
            foreach (Button button in choiceButtons)
            {
                button.onClick.AddListener(delegate
                {
                    FMODAudioManager.instance.PlayOneShot("OK");
                });
            }

            inputfieldSubmitButton?.onClick.AddListener(delegate
            {
                FMODAudioManager.instance.PlayOneShot("OK");
            });
        }

    }

}

