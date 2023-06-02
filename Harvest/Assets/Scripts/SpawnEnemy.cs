using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [SerializeField] public Transform[] spawnPoints;
    [SerializeField] public GameObject Enemy;
    [SerializeField] public GameObject nextGate;
    [SerializeField] public GameObject prevGate;
    

    private List<GameObject> enemyList = new List<GameObject>();
    private bool enemiesSpawned = false;
    private ParticleSystem implosion;
    private bool gatesOpened = false;

    void Update() {
        if (enemiesSpawned && CheckIfAllDead() && !gatesOpened) {
            gatesOpened = true;
            nextGate.GetComponent<Gates>().OpenGate();
        }
    }

    void OnTriggerEnter(Collider other) {
        if (!enemiesSpawned) {
            enemiesSpawned = true;
            StartCoroutine(SpawnEnemies());
            if(prevGate != null) {
                prevGate.GetComponent<Gates>().CloseGate();
            }
        }
    }

    IEnumerator SpawnEnemies() {
        foreach(Transform spawnPoint in spawnPoints) {
            implosion = spawnPoint.GetComponentInChildren<ParticleSystem>();
            implosion.Play();
            
            yield return new WaitForSeconds(0.3f);

            GameObject newEnemy = Instantiate(Enemy, spawnPoint.position, spawnPoint.rotation);
            enemyList.Add(newEnemy);
            RandomizeGear(newEnemy);
        }
    }

    void RandomizeGear(GameObject enemy) {
        //Chest
        if (Random.Range(0, 2) == 1) {
            enemy.transform.Find("Armors/StarterClothes/Starter_Chest").gameObject.SetActive(true);
        } else {
            enemy.transform.Find("Armors/Plate1/PlateSet1_Chest").gameObject.SetActive(true);
        }

        //Pants
        if(Random.Range(0, 2) == 1) {
            enemy.transform.Find("Armors/StarterClothes/Starter_Pants").gameObject.SetActive(true);
        } else {
            enemy.transform.Find("Armors/Plate1/PlateSet1_Pants").gameObject.SetActive(true);
        }

        //Shoes
        switch (Random.Range(0,3)) {
            case 1:
                enemy.transform.Find("Armors/StarterClothes/Starter_Boots").gameObject.SetActive(true);
                break;
            case 2:
                enemy.transform.Find("Armors/Plate1/PlateSet1_Boots").gameObject.SetActive(true);
                break;
            default:
                enemy.transform.Find("Mesh/Body/Feet").gameObject.SetActive(true);
                break;
        }

        //Hair/Helmet
        switch (Random.Range(0,3)) {
            case 1:
                enemy.transform.Find("Mesh/Customization/Hair/Hair7").gameObject.SetActive(true);
                break;
            case 2:
                enemy.transform.Find("Armors/Plate1/PlateSet1_Helmet").gameObject.SetActive(true);
                break;
            default:
                break;
        }

        //Beard
        if(Random.Range(0,2) == 1) {
            enemy.transform.Find("Mesh/Customization/Beards/Beard6").gameObject.SetActive(true);
        }

        //Moustache
        if(Random.Range(0,2) == 1) {
            enemy.transform.Find("Mesh/Customization/Moustaches/Moustache4").gameObject.SetActive(true);
        }

        //Gloves
        if(Random.Range(0,2) == 1) {
            enemy.transform.Find("Mesh/Body/Hands").gameObject.SetActive(false);
            enemy.transform.Find("Armors/Plate1/PlateSet1_Gloves").gameObject.SetActive(true);
        }

        //Shoulders
        if(Random.Range(0,2) == 1) {
            enemy.transform.Find("Armors/Plate1/PlateSet1_Shoulders").gameObject.SetActive(true);
        }
    }

    bool CheckIfAllDead() {
        bool allDead = true;

        foreach(GameObject enemy in enemyList) {
            Enemy EnemyScript = enemy.GetComponent<Enemy>();

            if (!EnemyScript.checkIfDead()) {
                allDead = false;
                break;
            }
        }

        return allDead;
    }
}
