using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public interface IFirearm
{
    Sprite FirearmSprite { get; }
    AudioClip FireGunAudioClip { get; }
    string Name { get; }
    float BulletSpeed { get; }
    float LengthOfBarrel { get; }
    bool ShouldFlip { get; }

    void FireGun();
    void ReadyGun();
}

