using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class ModularWorldGenerator : MonoBehaviour {
	public Module[] Modules;
	public Module StartModule;
    public GameObject[] items;

    public GameObject slim;
    public GameObject goblin;
    public GameObject boss;

    public GameObject gunner;
    public GameObject mage;
    static public GameObject player;
    static public int playerChoice = 1;
	public int Iterations = 5;


	void Start() {

        if (playerChoice == 1) {
            player = gunner; 
        } else if (playerChoice == 2) {
            player = mage;
        }


        var startModule = (Module) Instantiate(StartModule, transform.position, transform.rotation);
		var pendingExits = new List<ModuleConnector>(startModule.GetExits());
		var enemySpots = new List<EnemyConnector>();
		var playerSpawns = new List<PlayerConnector>(startModule.getPlayer());
		var playerSpawn = playerSpawns.FirstOrDefault();
        var itemSpots = new List<ItemConnector>();
        

        for (int iteration = 0; iteration < Iterations; iteration++) {
			var newExits = new List<ModuleConnector>();
			var newEnemies = new List<EnemyConnector>();
            var newItems = new List<ItemConnector>();

            foreach (var pendingExit in pendingExits) {
				var newTag = GetRandom(pendingExit.Tags);
				var newModulePrefab = GetRandomWithTag(Modules, newTag);
				var newModule = (Module) Instantiate(newModulePrefab);
				var newModuleExits = newModule.GetExits();
				var newModuleEnemies = newModule.GetEnemies();
                var newModuleItems = newModule.getItems();
                var exitToMatch = newModuleExits.FirstOrDefault(x => x.IsDefault) ?? GetRandom(newModuleExits);
				MatchExits(pendingExit, exitToMatch);
				newExits.AddRange(newModuleExits.Where(e => e != exitToMatch));
				newEnemies.AddRange(newModuleEnemies);
                newItems.AddRange(newModuleItems);
            }
			enemySpots = newEnemies;
			pendingExits = newExits;
            itemSpots = newItems;
        }

		if (startModule.Tags.Contains ("Boss")) {
			var newEnemies = new List<EnemyConnector>();
			var newModuleEnemies = startModule.GetEnemies();
			enemySpots.AddRange (newModuleEnemies);
		}


		foreach (var remainingExit in pendingExits) {
			var curModule = (Module) Instantiate (StartModule, transform.position, transform.rotation);
			var curModuleExits = curModule.GetExits ();
			var exitToMatch = curModuleExits.FirstOrDefault (x => x.IsDefault) ?? GetRandom (curModuleExits);
			MatchExits (remainingExit, exitToMatch);
		}

		foreach(var enemySpot in enemySpots) {
			if(enemySpot.Tags.Contains("Slim")) {
				var currentEnemy =(GameObject) Instantiate(slim);
				currentEnemy.transform.position = enemySpot.transform.position;
			} else if(enemySpot.Tags.Contains("Goblin")) {
				var currentEnemy =(GameObject) Instantiate(goblin);
				currentEnemy.transform.position = enemySpot.transform.position;
			} else if(enemySpot.Tags.Contains("Boss")) {
				var currentEnemy =(GameObject) Instantiate(boss);
				currentEnemy.transform.position = enemySpot.transform.position;
			}
		}

        foreach (var itemSpot in itemSpots) {
            var curItem = GetRandom(items);
            var newItem = Instantiate(curItem);
            newItem.transform.position = itemSpot.transform.position;
        }

		player.transform.position = playerSpawn.transform.position;
    }

	private void MatchExits(ModuleConnector oldExit, ModuleConnector newExit) {
		var newModule = newExit.transform.parent;
		var forwardVectorToMatch = -oldExit.transform.forward;
		var correctiveRotation = Azimuth(forwardVectorToMatch) - Azimuth(newExit.transform.forward);
		newModule.RotateAround(newExit.transform.position, Vector3.up, correctiveRotation);
		var correctiveTranslation = oldExit.transform.position - newExit.transform.position;
		newModule.transform.position += correctiveTranslation;
	}


	private static TItem GetRandom<TItem>(TItem[] array) {
		return array[(Random.Range(0, array.Length))];
	}


	private static Module GetRandomWithTag(IEnumerable<Module> modules, string tagToMatch) {
		var matchingModules = modules.Where(m => m.Tags.Contains(tagToMatch)).ToArray();
		return GetRandom(matchingModules);
	}


	private static float Azimuth(Vector3 vector) {
		return Vector3.Angle(Vector3.forward, vector) * Mathf.Sign(vector.x);
	}
}