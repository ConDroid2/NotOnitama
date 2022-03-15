using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] private AudioSource _audioSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip _pieceMoveClick;
    [SerializeField] private AudioClip _menuButtonClicked;

    public void PlayPieceMoveClick()
    {
        _audioSource.clip = _pieceMoveClick;
        _audioSource.Play();
    }

    public void PlayMenuButonClicked()
    {
        _audioSource.clip = _menuButtonClicked;
        _audioSource.Play();
    }
}
