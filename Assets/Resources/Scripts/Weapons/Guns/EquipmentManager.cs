using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

public class EquipmentManager : MonoBehaviour
{
    public float WeaponLength
    {
        get
        {
            if(_canMelee)
            {
                return _meleeWeaponCollider.size.x;
            }
            else
            {
                //Debug.LogError("If you want this object to use guns, add a Gun gameobject with a Sprite Renderer and a Gun Shot Script");

                throw new System.Exception("If you want this object to use guns, add a Gun gameobject with a Sprite Renderer and a Gun Shot Script");
            }
        }
    }

    private Transform _gunTransform;
    private Transform _gunBarrelTransform;
    private SpriteRenderer _gunSpriteRenderer;
    private GunShotScript _shootGunScript;
    private PointGun _gunAimer;

    private Transform _meleeTransform;
    private SpriteRenderer _meleeWeaponSpriteRenderer;
    private CapsuleCollider2D _meleeWeaponCollider;
    private MeleeScript _meleeWeaponScript;
    private UnityAction _onMeleeStart;
    private UnityAction _onMeleeEnd;

    [SerializeField]
    protected List<Firearm> _ownedFirearms;
    protected IFirearm _equipedFirearm;
    protected int _indexOfEquiped;
    private bool _canShoot;

    [SerializeField]
    protected List<MeleeWeapon> _ownedMeleeWeapons;
    private IMeleeWeapon _equipedMeleeWeapon;
    private bool _canMelee;

    // Use this for initialization
    protected virtual void Start()
    {
        if (this.GetComponentInChildren(typeof(GunShotScript)) as GunShotScript != null)
        {
            this.InitilizeGuns();
        }
        else
        {
            _canShoot = false;
        }

        if (this.GetComponentInChildren<MeleeScript>() != null)
        {
            this.InitilizeMelee();
        }
        else
        {
            _canMelee = false;
        }
        
        if(this._canShoot && this._ownedFirearms.Any())
        {
            this.Equip(this._ownedFirearms.First());
        }
        else if(!this._canShoot && this._ownedFirearms.Any())
        {
            Debug.LogError("If you want this object to use guns, add a Gun gameobject with a Sprite Renderer and a Gun Shot Script");
        }

        if (this._canMelee && this._ownedMeleeWeapons.Any())
        {
            this.Equip(this._ownedMeleeWeapons.First());
        }
        else if (!this._canMelee && this._ownedMeleeWeapons.Any())
        {
            Debug.LogError("If you want this object to use melee weapons, add a Melee Weapon gameobject with a Sprite Renderer and a Melee Script");
        }

        //Cheat();
    }

    
    public void AddWeapon(Firearm toObtain)
    {
        if(this._canShoot)
        {
            this._ownedFirearms.Add(toObtain);

            if(this._equipedFirearm == null)
            {
                this.Equip(toObtain);
            }
        }
        else
        {
            Debug.LogError("If you want this object to use guns, add a Gun gameobject with a Sprite Renderer and a Gun Shot Script");
        }
    }
    
    public void AddWeapon(MeleeWeapon toObtain)
    {
        if (this._canMelee)
        {
            this._ownedMeleeWeapons.Add(toObtain);

            if (this._equipedMeleeWeapon == null)
            {
                this.Equip(toObtain);
            }
        }
        else
        {
            Debug.LogError("If you want this object to use guns, add a Gun gameobject with a Sprite Renderer and a Gun Shot Script");
        }
        _ownedMeleeWeapons.Add(toObtain);
    }

    protected void Equip(IFirearm toEquip)
    {
        var localPosition = _gunBarrelTransform.localPosition;
        var differance = _equipedFirearm != null
            ? _equipedFirearm.LengthOfBarrel - toEquip.LengthOfBarrel
            : 0;

        localPosition.x -= differance;
        
        _gunBarrelTransform.localPosition = localPosition;

        _gunSpriteRenderer.sprite = toEquip.FirearmSprite;
        _gunSpriteRenderer.flipX = toEquip.ShouldFlip;

        _shootGunScript.BulletForce = toEquip.BulletSpeed;
        _shootGunScript.GunShotSound.clip = toEquip.FireGunAudioClip;

        _equipedFirearm = toEquip;
    }

    protected void Equip(IMeleeWeapon toEquip)
    {
        _meleeWeaponSpriteRenderer.sprite = toEquip.WeaponSprite;
        _meleeWeaponSpriteRenderer.flipX = toEquip.ShouldFlip;
        _meleeWeaponSpriteRenderer.enabled = false;

        _meleeWeaponCollider.size = toEquip.ColliderDimensions;
        _meleeWeaponCollider.offset = toEquip.ColliderOffset;

        _meleeWeaponScript.SwingWeaponAudioSource.clip = toEquip.SwingWeaponAudioClip;
        _meleeWeaponScript.Power.SetBaseValue(toEquip.Power);
        _meleeWeaponScript.Power.CalculateCurrentValue();
        _meleeWeaponScript.SwingRotationStartAngle = toEquip.SwingRotationStartAngle;
        _meleeWeaponScript.SwingRotationEndAngle = toEquip.SwingRotationEndAngle;
        _meleeWeaponScript.SwingRotationSpeed = toEquip.SwingRotationSpeed;
        _meleeWeaponScript.WeaponLength = this.WeaponLength;
        
        _equipedMeleeWeapon = toEquip;
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
        var rifle = Resources.Load("Scriptable Objects\\Created Assets\\Firearms\\Rifle") as Firearm;

        this.AddWeapon(rifle);
    }

    private void StartMelee()
    {
        if(_canShoot)
        {
            _gunSpriteRenderer.enabled = false;

            _shootGunScript.enabled = false;

            _gunAimer.enabled = false;
        }

        _meleeWeaponSpriteRenderer.enabled = true;
    }

    private void EndMelee()
    {
        _meleeWeaponSpriteRenderer.enabled = false;

        if(_canShoot)
        {
            _gunSpriteRenderer.enabled = true;

            _shootGunScript.enabled = true;

            _gunAimer.enabled = true;
        }
    }

    private void InitilizeGuns()
    {
        _shootGunScript = this.GetComponentInChildren(typeof(GunShotScript)) as GunShotScript;

        var gun = _shootGunScript.gameObject;

        _gunTransform = gun.transform;

        _gunBarrelTransform = FindGunBarrel();

        _gunSpriteRenderer = gun.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;

        _gunAimer = this.GetComponent(typeof(PointGun)) as PointGun;

        _canShoot = true;
    }

    private void InitilizeMelee()
    {
        _meleeWeaponScript = this.GetComponentInChildren<MeleeScript>();

        var meleeWeapon = _meleeWeaponScript.gameObject;

        _meleeTransform = meleeWeapon.transform;

        _meleeWeaponSpriteRenderer = meleeWeapon.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;

        _meleeWeaponCollider = meleeWeapon.GetComponent<CapsuleCollider2D>();

        _onMeleeStart = () => StartMelee();

        _onMeleeEnd = () => EndMelee();

        EventManager.Instance.AddObjectSpecificListener(EventType.MELEE_START, _onMeleeStart, meleeWeapon);

        EventManager.Instance.AddObjectSpecificListener(EventType.MELEE_END, _onMeleeEnd, meleeWeapon);

        _canMelee = true;
    }
}
