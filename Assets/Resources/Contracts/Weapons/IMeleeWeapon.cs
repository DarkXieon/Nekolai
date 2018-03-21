using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public interface IMeleeWeapon
{
    Sprite WeaponSprite { get; }
    AudioClip SwingWeaponAudioClip { get; }
    string Name { get; }
    // The amount of damage the weapon will give to enemies
    int Power { get; } //gets converted to stat power later
    float SwingRotationStartAngle { get; }
    float SwingRotationEndAngle { get; }
    float SwingRotationSpeed { get; }
    float SpriteScale { get; }
    Vector2 ColliderDimensions { get; }
    Vector2 ColliderOffset { get; }
    bool ShouldFlip { get; }

    //void Attack();
}
