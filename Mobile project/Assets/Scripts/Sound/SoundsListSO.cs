using UnityEngine;

[CreateAssetMenu(fileName = "SoundList", menuName = "Scriptables/SoundList", order = 0)]
public class SoundsListSO : ScriptableObject
{
    public SoundInfo[] sounds;
    public SoundInfoList[] randomSounds;

    public SoundInfo FindSound(string soundName)
    {
        foreach (SoundInfo s in sounds)
        {
            if (s.clipName == soundName) return s;
        }
        
        Debug.Log("No sound named " + soundName + ".");
        return null;
    }

    public SoundInfo FindRandomSound(string categoryName)
    {
        foreach (SoundInfoList soundList in randomSounds)
        {
            if (soundList.categoryName == categoryName) return soundList.sounds[Random.Range(0, soundList.sounds.Length)];
        }
        
        Debug.Log("No category named " + categoryName + ".");
        return null;
    }
}

[System.Serializable]
public class SoundInfo
{
    public string clipName;
    public AudioClip clip;
    [Range(0,1)]
    public float clipVolume = 1;
}

[System.Serializable]
public class SoundInfoList
{
    public string categoryName;
    public SoundInfo[] sounds;
}
