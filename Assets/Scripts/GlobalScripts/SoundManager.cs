using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource _sourceVoiceOver;
    public AudioSource _sourceOther;


    private void Awake()
    {

        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
        _sourceVoiceOver = GetComponent<AudioSource>();
    }


    public void PlayVoiceOver(int level, int step, TMP_Text subtitleTxt)
    {
        subtitleTxt.gameObject.SetActive(true);
        subtitleTxt.text = GameManager.Instance.levelDatas[level].voiceOverDatas[step].subText;
        if (PlayerPrefs.GetInt("isEnglish", 0) == 0)
        {
            _sourceVoiceOver.PlayOneShot(GameManager.Instance.levelDatas[level].voiceOverDatas[step]._english);
        }
        else
        {
            _sourceVoiceOver.PlayOneShot(GameManager.Instance.levelDatas[level].voiceOverDatas[step]._hindi);

        }

    }

    public IEnumerator PlayDelayedSound(int level, int step, TMP_Text subtitle , float delay)
    {
        yield return new WaitForSeconds(delay);
        PlayVoiceOver(level, step, subtitle);

    }

    [Serializable]
    public class VoiceOverData
    {
        public AudioClip _english;
        public AudioClip _hindi;
        public string subText;
    }
}
