using UnityEngine;
using System.Collections;

public class Killfeed : MonoBehaviour {

    [SerializeField]
    GameObject KillfeedItemPrefab;

    void Start ()
    {
        GameManagerCar.instance.onPlayerKilledCallback += OnKill;
	}

    public void OnKill(string player, string source)
    {
        //Debug.Log(source +" Killed "+ player );
        GameObject go = (GameObject)Instantiate(KillfeedItemPrefab, this.transform);
        go.GetComponent<KillfeedItem>().Setup(player,source);

        Destroy(go, 4f);
    }
	
	
}
