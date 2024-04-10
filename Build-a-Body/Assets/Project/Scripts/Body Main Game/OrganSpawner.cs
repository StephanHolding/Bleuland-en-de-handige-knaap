using System.Collections.Generic;
using UnityEngine;

public class OrganSpawner : MonoBehaviour
{

    /*    [NonSerialized]
        public Dictionary<string, Organ> organDict = new Dictionary<string, Organ>();

        public HashSet<string> finishedOrgan = new HashSet<string>();

        public bool cameraMoving = false;
        public Vector3 defaultCameraPosition;
        public float cameraShiftVelocity = 1f;*/

    public void SpawnLockedOrgans()
    {
        //using the name of the gameobject to find it in resources

        if (!Blackboard.HasKey(BlackboardKeys.LOCKED_ORGANS)) return;

        List<string> lockedOrgans = Blackboard.Read<List<string>>(BlackboardKeys.LOCKED_ORGANS);
        if (lockedOrgans != null)
        {
            foreach (string organ in lockedOrgans)
            {
                GameObject prefab = Resources.Load<GameObject>("Prefabs/" + organ);
                Organ prefabComponent = prefab.GetComponent<Organ>();
                GameObject spawnedObject = Instantiate(prefab, prefabComponent.targetPosition, prefab.transform.rotation);
                Organ spawnedComponent = spawnedObject.GetComponent<Organ>();

                spawnedComponent.draggable = false;
            }
        }
    }

    public void SpawnMoveableOrgans()
    {
        //again, same name as the gameobject in prefabs
        string lastFinishedMinigame = Blackboard.Read<string>(BlackboardKeys.LAST_FINISHED_MINIGAME);
        GameObject prefab = Resources.Load<GameObject>("Prefabs/" + lastFinishedMinigame);
        GameObject spawnedObject = Instantiate(prefab, prefab.transform.position, prefab.transform.rotation);
        Organ spawnedComponent = spawnedObject.GetComponent<Organ>();

        spawnedComponent.draggable = true;
    }

    /*    public void Update()
        {
            if (SceneManager.GetActiveScene().name != "MainScene") return;

            CameraShift();
            foreach (var item in organDict)
            {
                if (GameManager.instance.placementFinish)
                    item.Value.draggable = false;
                else if (item.Key != GameManager.instance.currentWinMinigame)
                    item.Value.draggable = false;
                else
                    item.Value.draggable = true;

                if (item.Key == GameManager.instance.currentWinMinigame || finishedOrgan.Contains(item.Key))
                    item.Value.gameObject.SetActive(true);
                else
                    item.Value.gameObject.SetActive(false);
            }
        }

        public void CameraShift()
        {
            Vector3 targetPosition;
            if (GameManager.instance.placementFinish || !organDict.ContainsKey(GameManager.instance.currentWinMinigame))
                targetPosition = defaultCameraPosition;
            else
                targetPosition = organDict[GameManager.instance.currentWinMinigame].targetCameraPosition;

            if ((targetPosition - Camera.main.transform.position).magnitude < cameraShiftVelocity)
            {
                Camera.main.transform.position = targetPosition;
                cameraMoving = false;
            }
            else
            {
                Camera.main.transform.position += cameraShiftVelocity * (targetPosition - Camera.main.transform.position).normalized;
                cameraMoving = true;
            }
        }

        public void FinishPlacement(string organID)
        {
            finishedOrgan.Add(organID);
            GameManager.instance.placementFinish = true;
            UIManager.instance.popup(organID + " Placed");
        }*/
}
