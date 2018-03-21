using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Stat_Power))]
public abstract class MeleeScript : MonoBehaviour, IWeapons
{
    public AudioSource SwingWeaponAudioSource { get; private set; }
    public Stat_Power Power { get; private set; }
    public float SwingRotationStartAngle
    {
        get { return _swingRotationStartAngle; }
        set
        {
            _totalAttackRotationAmount += Mathf.Abs(value) - Mathf.Abs(_swingRotationStartAngle);
            _swingRotationStartAngle = value;
        }
    }
    public float SwingRotationEndAngle
    {
        get { return _swingRotationEndAngle; }
        set
        {
            _totalAttackRotationAmount += Mathf.Abs(value) - Mathf.Abs(_swingRotationEndAngle);
            _swingRotationEndAngle = value;
        }
    }
    public float SwingRotationSpeed { get; set; }
    public float WeaponLength { get; set; }
    // Put this script directly on the weapon
    private Transform weapon;

    [SerializeField]
	private Transform shoulder;

	private GameObject enemy;
	// The melee weapon should have a collider, if no weapon then collider determines if we are in range for attack
	private bool inRange = false;
    private bool attacking = false;
    // This is a value that is set true only when we are currently doing an attack, to keep a delay between attacks

    private float _amountRotatedThisAttack;
    private float _totalAttackRotationAmount;
    private float _swingRotationStartAngle;
    private float _swingRotationEndAngle;
    
    [SerializeField]
    private float _attackCooldown;

    private float _cooldownLeft;

    // Timer to delay the time between attacks
    //private float attack_Delay = .25f;
    //private float attack_Timer = 0f;

    private string _name = "Melee";
	//private int _power = 4;

	public string WeaponName
	{
		get{ return _name; }
		set{ _name = value; }
	}
    
    protected virtual void Awake()
    {
        weapon = this.transform;

        _amountRotatedThisAttack = 0;

        _swingRotationStartAngle = 0;

        _swingRotationEndAngle = 0;

        _totalAttackRotationAmount = 0;

        SwingWeaponAudioSource = this.GetComponent<AudioSource>();

        Power = this.GetComponent<Stat_Power>();

        _cooldownLeft = 0;
    }

    // If we hit the trigger we are in range of the enemy
    void OnTriggerEnter2D(Collider2D collision)
	{
        var health = collision.gameObject.GetComponent<Stat_Health>();
		if(health != null && attacking)
		{
            health.ChangeHealth(Power.GetCurrentValue() * -1);
            EndAttack();
            //Debug.LogError("fkjdlsfjls");
			// Set the current enemy to this collision
			//enemy = collision.gameObject;
			// Set in range to true
			//inRange = true;
		}

	 }
    /*
	// If we exit the collider range, it will tell the inRange value we are not within range
	void OnTriggerExit2D(Collider2D col)
	{
		inRange = false;
	}
    */
	public void Attack ()
    {
        EventManager.Instance.ExecuteObjectSpecificEvent(EventType.MELEE_START, this.gameObject);
        
        attacking = true;
        
        shoulder.transform.localRotation = Quaternion.Euler(0, 0, SwingRotationStartAngle);

        /*
		// If we are in range we can attack our enemy
		if (inRange)
		{
			// Here we would do a MoveWeapon function which would move the sword on a hinge joint.
			// DECREASE IN HEALTH GOES HERE TO gameobject enemy
			Destroy (enemy);
		}
        */
    }

    private void EndAttack()
    {
        attacking = false;

        _amountRotatedThisAttack = 0;

        _cooldownLeft = _attackCooldown;

        EventManager.Instance.ExecuteObjectSpecificEvent(EventType.MELEE_END, this.gameObject);
    }

    private void FixedUpdate()
    {
        if(attacking)
        {
            var rotateAmount = _totalAttackRotationAmount * Time.fixedDeltaTime * SwingRotationSpeed;

            var currentZRotation = shoulder.transform.localRotation.eulerAngles.z;

            shoulder.transform.localRotation = Quaternion.Euler(0, 0, currentZRotation - rotateAmount);

            _amountRotatedThisAttack += rotateAmount;

            if (_amountRotatedThisAttack >= _totalAttackRotationAmount)
            {
                EndAttack();
            }
        }
    }

    // Update is called once per frame
    private void Update ()
	{
        if (_cooldownLeft > 0)
        {
            _cooldownLeft = _cooldownLeft - Time.deltaTime < 0
                ? 0
                : _cooldownLeft - Time.deltaTime;
        }
        else if (this.WillAttack() && !this.attacking)
		{
            Attack();
		}
	}

    protected abstract bool WillAttack();
}