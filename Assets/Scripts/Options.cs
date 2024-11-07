using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class Options : MonoBehaviour
{
    public Slider volume;
    public AudioMixer audioMixer;
    void Start()
    {
        float defaultVolume = PlayerPrefs.GetFloat("volume", 0.75f);
        volume.SetValueWithoutNotify(defaultVolume);
        audioMixer.SetFloat("volume", defaultVolume);
    }
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("volume", volume);
        Debug.Log(volume);
    }
}
