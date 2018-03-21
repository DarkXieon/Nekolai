using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Melee Weapon")]
public class MeleeWeapon : ScriptableObject, IMeleeWeapon
{
    public Sprite WeaponSprite { get { return _weaponSprite; } }

    public AudioClip SwingWeaponAudioClip { get { return _swingWeaponAudioClip; } }

    public string Name { get { return _name; } }

    public int Power { get { return _power; } }

    public float SwingRotationStartAngle { get { return _swingRotationStartAngle; } }

    public float SwingRotationEndAngle { get { return _swingRotationEndAngle; } }

    public float SwingRotationSpeed { get { return _swingRotationSpeed; } }

    public float SpriteScale { get { return _spriteScale; } }

    public bool ShouldFlip { get { return _shouldFlip; } }

    public Vector2 ColliderDimensions { get { return _colliderDimensions; } }

    public Vector2 ColliderOffset { get { return _colliderOffset; } }

    [SerializeField]
    private Sprite _weaponSprite;

    [SerializeField]
    private AudioClip _swingWeaponAudioClip;

    [SerializeField]
    private string _name;

    [SerializeField]
    private int _power;

    [SerializeField]
    private float _swingRotationStartAngle;

    [SerializeField]
    private float _swingRotationEndAngle;

    [SerializeField]
    private float _swingRotationSpeed;

    [SerializeField]
    private float _spriteScale;

    [SerializeField]
    private Vector2 _colliderDimensions;

    [SerializeField]
    private Vector2 _colliderOffset;

    [SerializeField]
    private bool _shouldFlip;
    
    /*
    public void Attack()
    {
        throw new System.NotImplementedException();
    }
    */
}
