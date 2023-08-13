using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine.InputSystem;
public class Player: MonoBehaviour
{
    // // Upgrade Stats
    // [Header("Player Upgrades")]
    // public bool overrideUpgradeStats;
    // [SerializeField] private int _jumpUpgradeLevel = 0;
    // [SerializeField] private int _dashUpgradeLevel = 0;
    // [SerializeField] private int _rechargeUpgradeLevel = 0;


    // Player Speed

    private PlayerMovement _movement;
    [Header("Player Speed")]
    [SerializeField] private bool _movementEnabled = true;
    public bool MovementEnabled
    {
        get
        {
            return _movementEnabled;
        }

        set
        {
            _movementEnabled = value;
            _movement.isEnabled = _movementEnabled;
        }
    }

    [SerializeField] private float _movementSpeed = 10;
    public float MovementSpeed
    {
        get
        {
            return _movementSpeed;
        }

        set
        {
            _movementSpeed = value;
            _movement.moveSpeed = _movementSpeed;
        }
    }
    // [SerializeField] private float _playerAcceleration = 10;
    // [SerializeField] private float _playerVelocity = 10;

    // public float _playerAcceleration
    // {
    //     get
    //     {
    //         return __playerAcceleration;
    //     }

    //     set
    //     {
    //         Debug.Log("Called");
    //         __playerAcceleration = value;
    //         _movement.moveSpeed = __playerAcceleration;
    //     }
    // }



    // Player Jump
    [Header("Player Jump")]
    [SerializeField] private int _maxJumps;
    public int MaxJumps {
        get
        {
            return _maxJumps;
        }

        set
        {
            _maxJumps = value;
            _movement.maxJumps = _maxJumps;
        }
    }

    [SerializeField] private float _jumpHeight = 10f;
    public float JumpHeight {
        get
        {
            return _jumpHeight;
        }

        set
        {
            _jumpHeight = value;
            _movement.jumpHeight = _jumpHeight;
        }
    }

    // Player Dash
    // [Header("Player Dash")]
    // [SerializeField] private float _dashDuration = 0.3f;
    // public float DashDuration {
    //     get
    //     {
    //         return _dashDuration;
    //     }

    //     set
    //     {
    //         _dashDuration = value;
    //         _movement.dashDuration = _dashDuration;
    //     }
    // }

    // private int _maxDashes;
    // public int MaxDashes {
    //     get
    //     {
    //         return _maxDashes;
    //     }

    //     set
    //     {
    //         _maxDashes = value;
    //         _movement.maxDashes = _maxDashes;
    //     }
    // }

    // [SerializeField] private float _dashSpeed = 50.0f;
    // public float DashSpeed {
    //     get
    //     {
    //         return _dashSpeed;
    //     }

    //     set
    //     {
    //         _dashSpeed = value;
    //         _movement.dashSpeed = _dashSpeed;
    //     }
    // }

    // private float _rechargeDuration;
    // public float RechargeDuration {
    //     get
    //     {
    //         return _rechargeDuration;
    //     }

    //     set
    //     {
    //         _rechargeDuration = value;
    //         _movement.rechargeDuration = _rechargeDuration;
    //     }
    // }

    // Player Look
    private PlayerLook _look;
    [Header("Player Look")]
    [SerializeField] private bool _lookEnabled = true;
    public bool LookEnabled
    {
        get
        {
            return _lookEnabled;
        }

        set
        {
            _lookEnabled = value;
            _look.isEnabled = _lookEnabled;
        }
    }

    [SerializeField] private float _playerCamDistance;
    public float PlayerCamDistance {
        get
        {
            return _playerCamDistance;
        }

        set
        {
            _playerCamDistance = value;
            _look.playerCamDistance = _playerCamDistance;
        }
    }
    [SerializeField] private float _turnSpeed = 0.5f;
    public float TurnSpeed {
        get
        {
            return _turnSpeed;
        }

        set
        {
            _turnSpeed = value;
            _look._turnSpeed = _turnSpeed;
        }
    }
    [SerializeField] private float _minTurnAngle = -90.0f;
    public float MinTurnAngle {
        get
        {
            return _minTurnAngle;
        }

        set
        {
            _minTurnAngle = value;
            _look._minTurnAngle = _minTurnAngle;
        }
    }

    [SerializeField] private float _maxTurnAngle = 90.0f;
    public float MaxTurnAngle {
        get
        {
            return _maxTurnAngle;
        }

        set
        {
            _maxTurnAngle = value;
            _look._maxTurnAngle = _maxTurnAngle;
        }
    }


    // private PlayerCollect _collect;
    // [Header("Player Weapon")]
    // [SerializeField] private bool _weaponEnabled = true;
    // public bool WeaponEnabled
    // {
    //     get
    //     {
    //         return _weaponEnabled;
    //     }

    //     set
    //     {
    //         _weaponEnabled = value;
    //         _collect.isEnabled = _weaponEnabled;
    //     }
    // }
    // // Player Harvesting
    // [SerializeField] private float _numRotations = 1.0f; // Per duration
    // public float NumRotations {
    //     get
    //     {
    //         return _numRotations;
    //     }

    //     set
    //     {
    //         _numRotations = value;
    //         _collect._numRotations = _numRotations;
    //     }
    // }
    // [SerializeField] private float _duration = 0.25f;
    // public float Duration {
    //     get
    //     {
    //         return _duration;
    //     }

    //     set
    //     {
    //         _duration = value;
    //         _collect._duration = _duration;
    //     }
    // }
    // [SerializeField] public float _weaponDelay = 0.2f;
    // public float WeaponDelay{
    //     get
    //     {
    //         return _weaponDelay;
    //     }

    //     set
    //     {
    //         _weaponDelay = value;
    //         _collect._weaponDelay = _weaponDelay;
    //     }
    // }


    void Start()
    {
        // Player Component Variables
        _movement = GetComponent<PlayerMovement>();
        _look = GetComponent<PlayerLook>();
        // _collect = GetComponent<PlayerCollect>();

        // Enable components
        MovementEnabled = _movementEnabled;
        LookEnabled = _lookEnabled;
        // WeaponEnabled = _weaponEnabled;

        // Player Movement
        MovementSpeed = _movementSpeed;

        // Player Jump
        MaxJumps = _maxJumps;
        JumpHeight = _jumpHeight;

        // Player Dash
        // DashDuration = _dashDuration;
        // DashSpeed = _dashSpeed;


        // Player Look
        PlayerCamDistance = _playerCamDistance;
        TurnSpeed = _turnSpeed;
        MinTurnAngle = _minTurnAngle;
        MaxTurnAngle = _maxTurnAngle;


        // Player Weapon
        // NumRotations = _numRotations;
        // Duration = _duration;
        // WeaponDelay = _weaponDelay;

        // PlayerData playerData = GameSession.Instance.Player;

        //Adjusts player's max ability stats to match what they have purchased from upgrades store
        //Unless I specifically tell it not to, in which case it will use the maximum stats instead.
        // if (overrideUpgradeStats)
        // {
            // MaxJumps = (playerData.upgrades["Jump"] as UpgradeData<int>).GetValue(_jumpUpgradeLevel);
            // MaxDashes = (playerData.upgrades["Dash"] as UpgradeData<int>).GetValue(_dashUpgradeLevel);
            // RechargeDuration = (playerData.upgrades["DashRecharge"] as UpgradeData<float>).GetValue(_rechargeUpgradeLevel);
        // }
        // else
        // {
        //     MaxJumps = (playerData.upgrades["Jump"] as UpgradeData<int>).GetValue();
        //     MaxDashes = (playerData.upgrades["Dash"] as UpgradeData<int>).GetValue();
        //     RechargeDuration = (playerData.upgrades["DashRecharge"] as UpgradeData<float>).GetValue();
        // }

    }

    // private void OnValidate(){
    //     MovementSpeed = _movementSpeed;
    // }
}