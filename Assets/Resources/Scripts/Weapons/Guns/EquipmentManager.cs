using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EquipmentManager : MonoBehaviour
{
    private Transform _gunTransform;
    private Transform _gunBarrelTransform;
    private SpriteRenderer _gunSpriteRenderer;
    private GunShotScript _shootGunScript;
    private PointGun _gunAimer;

    private IList<IFirearm> _ownedFirearms;
    private IFirearm _equiped;
    private int _indexOfEquiped;

    // Use this for initialization
    private void Awake()
    {
        _shootGunScript = this.GetComponentInChildren(typeof(GunShotScript)) as GunShotScript;

        var gun = _shootGunScript.gameObject;

        _gunTransform = gun.transform;

        _gunBarrelTransform = FindGunBarrel();

        _gunSpriteRenderer = gun.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;

        _gunAimer = this.GetComponent(typeof(PointGun)) as PointGun;

        _ownedFirearms = new List<IFirearm>();

        var starterPistol = Resources.Load("StarterPistol") as Firearm;
        this.AddWeapon(starterPistol);
        this.Equip(starterPistol);

        Cheat();
    }

    // Update is called once per frame
    private void Update()
    {
        var userInput = Input.GetAxis("Mouse ScrollWheel");
        var userScrollingUp = userInput > 0;
        var userScrollingDown = userInput < 0;
        
        if (userScrollingUp || userScrollingDown)
        {
            var nextIndex = userScrollingUp
                ? _ownedFirearms.GetNextPositiontWrap(_indexOfEquiped)
                : _ownedFirearms.GetPreviousPositiontWrap(_indexOfEquiped);

            var nextWeapon = _ownedFirearms.ElementAt(nextIndex);

            this.Equip(nextWeapon);

            _equiped = nextWeapon;

            _indexOfEquiped = nextIndex;
        }
    }

    public void AddWeapon(IFirearm toObtain)
    {
        _ownedFirearms.Add(toObtain);
    }
    
    private void Equip(IFirearm toEquip)
    {
        var localPosition = _gunBarrelTransform.localPosition;
        var differance = _equiped != null
            ? _equiped.LengthOfBarrel - toEquip.LengthOfBarrel
            : 0;
        localPosition.x -= differance;

        _gunBarrelTransform.localPosition = localPosition;

        _gunSpriteRenderer.sprite = toEquip.FirearmSprite;
        _gunSpriteRenderer.flipX = toEquip.ShouldFlip;

        _shootGunScript.BulletForce = toEquip.BulletSpeed;
    }

    private Transform FindGunBarrel()
    {
        foreach(Transform child in _gunTransform)
        {
            if(child.tag == "Gun Barrel")
            {
                return child;
            }
        }

        Debug.LogError("You must have a gun barrel on every gun. Add a child object to the gun and tag it as a gun barrel.");

        return null;
    }

    private void Cheat()
    {
        var rifle = Resources.Load("Rifle") as Firearm;

        this.AddWeapon(rifle);
    }
}
