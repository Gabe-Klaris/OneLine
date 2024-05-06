using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumnController : MonoBehaviour
{
    public AudioSource bgAudio;
    public Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        bgAudio = GetComponent<AudioSource>();
        slider = GameObject.FindGameObjectWithTag("PauseMenuSlider").transform.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        bgAudio.volume = slider.value;
    }
}
