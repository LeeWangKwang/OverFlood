using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

//싱글톤
public class NetworkGameManager : NetworkBehaviour {

    static public List<PlayerAttribute> sPlayer = new List<PlayerAttribute>();

    public bool _decremeting = true;
    protected bool _running = true;

    #region Water

    public GameObject[] waterPrefabs;
    //public GameObject _water;
    public int _spawntime = 10;
    public int _floor = 4;
    public int _per_water = 4;
    public int _xterm = 8;
    public int _yterm = 3;

    private float width;
    private float height;
    private float min_y;
    private int[] _order;

    #endregion

    // Use this for initialization
    void Start ()
    {
        width = GameObject.Find("Ground01").transform.localScale.x;
        height = GameObject.Find("Ground02").transform.position.y - GameObject.Find("Ground01").transform.position.y;
        min_y = GameObject.Find("Ground01").transform.position.y;

        Debug.Log(width + " " + height);

        PositionSetting(_floor * _per_water);
    }

    /*
        Water가 스폰순서를 랜덤으로 정한다.
    */

    void PositionSetting(int count)
    {
        _order = new int[count];

        for (int i = 0; i < count; i++) _order[i] = i;

        for (int i = 0, j = count-1 ; j > 0 ; i++)  // 두번 셔플
        {
            if (i >= count)
            {
                Swap(j, Random.Range(0, count - 1));
                j--;
            }
            else
                Swap(i, Random.Range(0, count - 1));
        }

        for(int i = 0; i < count; i++) Debug.Log( i +  " : " +_order[i]);

        if(isServer)
            StartCoroutine("Spawn_Water");
    }

    void Swap(int num1, int num2)
    {
        int temp;

        temp = _order[num1];
        _order[num1] = _order[num2];
        _order[num2] = temp;

    }

    // Update is called once per frame
    [ServerCallback]
    void Update () {
        if (!_running) // 게임이 끝날경우에만 실행되게.
            return;

        //게임이 끝날경우 로비로

    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        foreach (GameObject obj in waterPrefabs)
        {
            ClientScene.RegisterPrefab(obj);
        }
    }

    IEnumerator Spawn_Water()
    {
        int count = _floor * _per_water;

        for (int i = 0; i < count; i++)
        {
            yield return new WaitForSeconds(_spawntime);
            Debug.Log(i);

            GameObject water;

            if (waterPrefabs.Length < 1)
            {
                 water = Instantiate(waterPrefabs[0], new Vector2(((_order[i] % _per_water) + _xterm * (_order[i] % _per_water)) - (width / 2),
                (min_y + (_order[i] / _per_water) * height + _yterm)), Quaternion.Euler(0, 0, 0)) as GameObject;
            }
            else
            {
                 water = Instantiate(waterPrefabs[waterPrefabs.Length - 1], new Vector2(((_order[i] % _per_water) + _xterm * (_order[i] % _per_water)) - (width / 2),
                (min_y + (_order[i] / _per_water) * height + _yterm)), Quaternion.Euler(0, 0, 0)) as GameObject;
            }

            NetworkServer.Spawn(water);
        }
    }
}
