using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using TMPro;


public class MovementStats : MonoBehaviour
{
    private TMP_Text _dashText;
    private TMP_Text _queuedDashText;
    private TMP_Text _rechargeDashText;
    private TMP_Text _rechargeTimeText;
    private TMP_Text _jumpText;
    
    [SerializeField] private GameObject _player;
     private PlayerMovement _movement;

    // Start is called before the first frame update
    void Start()
    {
        _dashText = transform.Find("DashLabel").gameObject.GetComponent<TMP_Text>();
        _queuedDashText = transform.Find("QDashLabel").gameObject.GetComponent<TMP_Text>();
        _rechargeDashText = transform.Find("ReDashLabel").gameObject.GetComponent<TMP_Text>();
        _rechargeTimeText = transform.Find("ReTimeLabel").gameObject.GetComponent<TMP_Text>();
        _jumpText = transform.Find("JumpLabel").gameObject.GetComponent<TMP_Text>();

        _movement = _player.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        _jumpText.text = "Jumps: " + _movement._playerJumps.ToString();
    }
}
