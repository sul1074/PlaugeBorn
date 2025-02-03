using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 플레이어 스탯은 여러 곳에서 그 값을 가져올 필요성이 높고
 * 씬이 변경되어도 유지될 필요성이 있음.
 * 따라서 플레이어 스탯은 별도의 싱글톤 오브젝트로 관리.
 */

public class StatsManager : MonoBehaviour
{
    private static StatsManager _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(_instance.gameObject);
            InitStats();
        }
        else
        {
            if (_instance != this)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public static StatsManager Instance
    {
        get
        {
            // get 호출시에 _instace가 없다는 것은, statsManager 게임 오브젝트가 생성되지 않았다는 의미이므로 생성 해주어야 함
            if (_instance == null)
            {
                _instance = new GameObject("StatsManager").AddComponent<StatsManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    // 플레이어 스탯 변수
    private int hp;
    private int armor;
    private int attackPower;
    private int speed;


    // 프로퍼티
    public int Hp
    {
        get { return hp; }
        set { hp = value; }
    }
    public int Armor
    {
        get { return armor; }
        set { armor = value; }
    }
    public int AttackPower
    {
        get { return attackPower; }
        set { attackPower = value; }
    }
    public int Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitStats()
    {
        Hp = 100;
        Armor = 10;
        AttackPower = 10;
        Speed = 10;
    }
}
