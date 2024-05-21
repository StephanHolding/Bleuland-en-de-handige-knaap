using FMOD.Studio;
using FMODUnity;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using STOP_MODE = FMOD.Studio.STOP_MODE;

/// <summary>
/// 
/// Used to play and stop audios from FMOD.
/// Also stops any playing audio on scene change.
/// 
/// </summary>

namespace FMOD_AudioManagement
{
    public class FMODAudioManager : SingletonTemplateMono<FMODAudioManager>
    {

        public EventReference[] fmodEvents;

        private Dictionary<string, EventReference> fmodEventsDictionary = new Dictionary<string, EventReference>();
        private List<EventInstance> audioInstances = new List<EventInstance>();

        protected override void Awake()
        {
            base.Awake();

            SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
            fmodEventsDictionary = FillEventsDictionary();
        }

        private void SceneManager_sceneUnloaded(Scene arg0)
        {
            StopAll();
        }

        private void OnDestroy()
        {
            StopAll();
            SceneManager.sceneUnloaded -= SceneManager_sceneUnloaded;
        }

        public void PlayOneShot(EventReference eventReference, Vector3 worldPosition = default(Vector3))
        {
            RuntimeManager.PlayOneShot(eventReference, worldPosition);
        }

        public void PlayOneShot(string audioName)
        {
            PlayOneShot(audioName, Vector3.zero);
        }

        public void PlayOneShot(string eventName, Vector3 worldPosition = default(Vector3))
        {
            PlayOneShot(GetEventReferenceFromDictionary(eventName), worldPosition);
        }

        public EventInstance Play(EventReference eventReference)
        {
            EventInstance toReturn = CreateEventInstance(eventReference);
            toReturn.start();
            audioInstances.Add(toReturn);
            return toReturn;
        }

        public EventInstance Play(string audioName)
        {
            return Play(GetEventReferenceFromDictionary(audioName));
        }

        public void Stop(EventInstance toStop, STOP_MODE stopMode = STOP_MODE.ALLOWFADEOUT)
        {
            toStop.setUserData(IntPtr.Zero);
            toStop.stop(stopMode);
            toStop.release();

            if (audioInstances.Contains(toStop))
            {
                audioInstances.Remove(toStop);
            }

        }

        public void StopAll()
        {
            for (int i = 0; i < audioInstances.Count; i++)
            {
                EventInstance toStop = audioInstances[i];

                toStop.setUserData(IntPtr.Zero);
                toStop.stop(STOP_MODE.ALLOWFADEOUT);
                toStop.release();
            }

            audioInstances.Clear();
        }

        public EventInstance CreateEventInstance(EventReference eventReference)
        {
            return RuntimeManager.CreateInstance(eventReference);
        }
        public EventInstance CreateEventInstance(string eventName)
        {
            return RuntimeManager.CreateInstance(GetEventReferenceFromDictionary(eventName));
        }

        private EventReference GetEventReferenceFromDictionary(string eventName)
        {
            return fmodEventsDictionary[eventName];
        }

        private Dictionary<string, EventReference> FillEventsDictionary()
        {
            Dictionary<string, EventReference> toReturn = new Dictionary<string, EventReference>();

            foreach (EventReference eRef in fmodEvents)
            {
                string[] seperatedPath = eRef.Path.Split('/', StringSplitOptions.RemoveEmptyEntries);
                toReturn.Add(seperatedPath[^1], eRef);
            }

            return toReturn;
        }

        public void SetParameterByLabel(EventInstance instance, string parameterName, string value)
        {
            instance.setParameterByNameWithLabel(parameterName, value);
        }

        public static bool IsPlaying(EventInstance instance)
        {
            instance.getPlaybackState(out PLAYBACK_STATE state);
            return state == PLAYBACK_STATE.PLAYING;
        }
    }

}

