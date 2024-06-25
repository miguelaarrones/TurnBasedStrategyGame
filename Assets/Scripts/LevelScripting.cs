using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelScripting : MonoBehaviour
{
    [SerializeField] private Door door1;
    [SerializeField] private Door door2;
    [SerializeField] private Door door3;

    [SerializeField] private List<GameObject> hidersList1;
    [SerializeField] private List<GameObject> hidersList2;
    [SerializeField] private List<GameObject> hidersList3;

    [SerializeField] private List<GameObject> enemylist1;
    [SerializeField] private List<GameObject> enemylist2;
    [SerializeField] private List<GameObject> enemylist3;

    // Start is called before the first frame update
    void Start()
    {
        door1.OnDoorOpened += (object sender, System.EventArgs e) =>
        {
            SetActiveGameObjectsList(hidersList1, false);
            SetActiveGameObjectsList(enemylist1, true);
        };

        door2.OnDoorOpened += (object sender, System.EventArgs e) =>
        {
            SetActiveGameObjectsList(hidersList2, false);
            SetActiveGameObjectsList(enemylist2, true);
        };

        door3.OnDoorOpened += (object sender, System.EventArgs e) =>
        {
            SetActiveGameObjectsList(hidersList3, false);
            SetActiveGameObjectsList(enemylist3, true);
        };
    }

    private void SetActiveGameObjectsList(List<GameObject> gameObjects, bool isActive)
    {
        foreach (GameObject go in gameObjects)
        {
            go.SetActive(isActive);
        }
    }
}
