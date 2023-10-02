using UnityEngine;
using UnityEngine.UI;

public class MusicianPointer : MonoBehaviour
{
    public Musician parentMusician;
    public SpriteRenderer affinityImage;
    public Sprite[] affinityGood;
    public Sprite[] affinityBad;

    /// <summary>
    /// Affinity sprite is set to neutral by default. If its proficient, will set to good note, otherwise set to bad note.
    /// </summary>
    /// <param name="isProficient">Has Proficiency with the instrument</param>
    public void UpdateAffinitySprite(bool isProficient)
    {
        affinityImage.sprite = isProficient ? affinityGood[Random.Range(0, affinityGood.Length)] : affinityBad[Random.Range(0, affinityBad.Length)];
    }
}
