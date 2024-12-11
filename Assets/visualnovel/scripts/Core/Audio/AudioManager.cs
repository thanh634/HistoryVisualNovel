using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private static string SFX_PARENT_NAME = "SFX";
    private static string SFX_NAME_FORMAT = "SFX - [{0}]";
    public const float TRACK_TRANSITION_SPEED = 1.0f;

    public static AudioManager instance { get; private set; }

    public Dictionary<int, AudioChannel> channels = new Dictionary<int, AudioChannel>();

    public AudioMixerGroup musicMixer;
    public AudioMixerGroup sfxMixer;
    public AudioMixerGroup voicesMixer;

    private Transform sfxRoot;

    private void Awake()
    {
        if(instance == null)
        {
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
            Debug.Log("AudioManager is created");
            instance = this;
        }
        else
        {
            DestroyImmediate(gameObject);
            return;
        }

        sfxRoot = new GameObject(SFX_PARENT_NAME).transform;
        sfxRoot.SetParent(transform);
    }

    public AudioSource PlaySoundEffect(string filePath, AudioMixerGroup mixer = null, float volume = 1, float pitch = 1, bool loop = false)
    {
        AudioClip clip = Resources.Load<AudioClip>(filePath);

        if(clip == null)
        {
            Debug.LogError($"Could not load audio file '{filePath}'. Please make sure this exist");
            return null;
        }

        return PlaySoundEffect(clip, mixer, volume, pitch, loop);
    }
    public AudioSource PlaySoundEffect(AudioClip clip, AudioMixerGroup mixer = null, float volume = 1, float pitch = 1, bool loop = false)
    {
        AudioSource efffectSource = new GameObject(string.Format(SFX_NAME_FORMAT, clip.name)).AddComponent<AudioSource>();
        efffectSource.transform.SetParent(sfxRoot);
        efffectSource.transform.position = sfxRoot.position;

        efffectSource.clip = clip;

        if (mixer == null)
            mixer = sfxMixer;

        efffectSource.outputAudioMixerGroup = mixer;
        efffectSource.volume = volume;
        efffectSource.spatialBlend = 0;
        efffectSource.pitch = pitch;
        efffectSource.loop = loop;

        efffectSource.Play();

        if (!loop)
            Destroy(efffectSource.gameObject, (clip.length / pitch) + 1f);

        return efffectSource;
    }

    public AudioSource PlayVoice(string filePath, float volume = 1, float pitch = 1, bool loop = false)
    {
        return PlaySoundEffect(filePath, voicesMixer, volume, pitch, loop);
    }
    public AudioSource PlayVoice(AudioClip clip, float volume = 1, float pitch = 1, bool loop = false)
    {
        return PlaySoundEffect(clip, voicesMixer, volume, pitch, loop);
    }

    public AudioChannel TryGetChannel(int channelNumber, bool createIfDoesNotExist = false)
    {
        AudioChannel channel = null;

        if (channels.TryGetValue(channelNumber, out channel))
            return channel;
        else if (createIfDoesNotExist)
        {
            channel = new AudioChannel(channelNumber);
            channels.Add(channelNumber, channel);
            return channel;
        }
        return null;
    }
    public AudioTrack PlayTrack(string filePath, int channel = 0, bool loop = true, float startingVolume = 0f, float volumeCap = 1f, float pitch = 1f)
    {
        AudioClip clip = Resources.Load<AudioClip>(filePath);

        if (clip == null)
        {
            Debug.LogError($"Could not load audio file '{filePath}'. Please make sure this exist");
            return null;
        }

        return PlayTrack(clip, channel, loop, startingVolume, volumeCap, pitch, filePath);
    }
    public AudioTrack PlayTrack(AudioClip clip, int channel = 0, bool loop = true, float startingVolume = 0f, float volumeCap = 1f, float pitch = 1f, string filePath = "")
    {
        AudioChannel audioChannel = TryGetChannel(channel, true);
        AudioTrack track = audioChannel.PlayTrack(clip, loop, startingVolume, volumeCap, pitch, filePath);
        return track;
    }

    public void StopSoundEffect(AudioClip clip) => StopSoundEffect(clip.name);
    public void StopSoundEffect(string soundName)
    {

        soundName = soundName.ToLower();
        AudioSource[] sources = sfxRoot.GetComponentsInChildren<AudioSource>();
        foreach (var source in sources)
        {
            if (source.clip.name.ToLower() == soundName)
            {
                Destroy(source.gameObject);
                return;
            }
        }
    }

    public void StopTrack(int channel)
    {
        AudioChannel c = TryGetChannel(channel, createIfDoesNotExist: false);
        if (c == null)
            return;

        c.StopTrack();
    }

    public void StopTrack(string trackName)
    {
        trackName = trackName.ToLower();

        foreach(var channel in channels.Values)
        {
            if (channel.activeTrack != null && channel.activeTrack.name.ToLower() == trackName) 
            {
                channel.StopTrack();
                return;
            }
        }
    }
}
