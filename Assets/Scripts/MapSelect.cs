using UnityEngine;
using System.Collections;

public class MapSelect : MonoBehaviour {

    public static MapSelect instance;

    public int number;

    public int number_of_calls;

    public bool IsHost = false;

    public GameObject test_Environment;
    public GameObject collosium;
    public GameObject desert;


    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);
    }

    public void Map(Transform ArenaHolder)
    {
        number_of_calls++;

        if (IsHost == true)
        {
            if (number == 0)
            {
                Vector3 pos0 = new Vector3(0, -1, 0);
                GameObject _map0 = (GameObject)Instantiate(test_Environment, pos0, ArenaHolder.rotation);
                _map0.transform.SetParent(ArenaHolder);
                Debug.Log("map_test");
            }
            else if (number == 1)
            {
                Vector3 pos1 = new Vector3(0, 1, 0);
                GameObject _map1 = (GameObject)Instantiate(collosium, pos1, ArenaHolder.rotation);
                _map1.transform.SetParent(ArenaHolder);
                Debug.Log("map_collosium");
            }
            else if (number == 2)
            {
                Vector3 pos2 = new Vector3(0, -2, 0);
                GameObject _map2 = (GameObject)Instantiate(desert, pos2, ArenaHolder.rotation);
                _map2.transform.SetParent(ArenaHolder);
                Debug.Log("map_desert");
            }
        }

        if (IsHost==false && number_of_calls>1) {
            if (number == 0)
            {
                Vector3 pos0 = new Vector3(0, -1, 0);
                GameObject _map0 = (GameObject)Instantiate(test_Environment, pos0, ArenaHolder.rotation);
                _map0.transform.SetParent(ArenaHolder);
                Debug.Log("map_test");
            }
            else if (number == 1)
            {
                Vector3 pos1 = new Vector3(0, 1, 0);
                GameObject _map1 = (GameObject)Instantiate(collosium, pos1, ArenaHolder.rotation);
                _map1.transform.SetParent(ArenaHolder);
                Debug.Log("map_collosium");
            }
            else if (number == 2)
            {
                Vector3 pos2 = new Vector3(0, -2, 0);
                GameObject _map2 = (GameObject)Instantiate(desert, pos2, ArenaHolder.rotation);
                _map2.transform.SetParent(ArenaHolder);
                Debug.Log("map_desert");
            }
        }
    }

    public void SetMapNumber(int _number)
    {
        number = _number;
    }

    public int GetMapNumber()
    {
        return number ;
    }

    public void ISHOST(bool state)
    {
        IsHost = state;
    }

    public void setNoCalls(int calls)
    {
        number_of_calls = calls;
    }

    public bool HOST()
    {
        return IsHost;
    }
}
