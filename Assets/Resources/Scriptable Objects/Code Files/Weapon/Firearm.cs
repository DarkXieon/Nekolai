using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Firearms")]
public class Firearm : ScriptableObject, IFirearm
{
    public Sprite FirearmSprite { get { return _firearmSprite; } }

    public AudioClip FireGunAudioClip { get { return _fireAudioClip; } }
    
    public string Name { get { return _name; } }

    public float BulletSpeed { get { return _bulletSpeed; } }

    public float LengthOfBarrel { get { return _lengthOfBarrel; } }

    public bool ShouldFlip { get { return _shouldFlip; } }

    [SerializeField]
    private Sprite _firearmSprite;

    [SerializeField]
    private AudioClip _fireAudioClip;

    [SerializeField]
    private string _name;
    
    [SerializeField]
    private float _bulletSpeed;

    [SerializeField]
    private float _lengthOfBarrel;

    [SerializeField]
    private bool _shouldFlip;
    
    //Eventually this will be triggered by an event
    public void FireGun()
    {
        throw new System.NotImplementedException();
    }

    public void ReadyGun()
    {
        throw new System.NotImplementedException();
    }
}