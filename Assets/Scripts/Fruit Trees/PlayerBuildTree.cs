using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class PlayerBuildTree : MonoBehaviour
{
    public static PlayerBuildTree Instance { get; private set; }

    public List<GameObject> FruitTrees = new();

    bool isPlacing = false;
    bool canPlace = false;

    GameObject objectToPlace;
    GameObject currentHologram;

    public GameObject treeHologram;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (isPlacing)
        {
            HologramToMouse();
            CheckValidity();
        }
    }

    [ContextMenu("Place Tree/Apple Tree")]
    public void PlaceAppleTree()
    {
        EnterBuildMode(FruitTrees.Find(obj => obj.name == "AppleTree"));
    }

    [ContextMenu("Place Tree/Pomegranate Tree")]
    public void PlacePomegranateTree()
    {
        EnterBuildMode(FruitTrees.Find(obj => obj.name == "PomegranateTree"));
    }


    public void EnterBuildMode(GameObject tree)
    {
        if (isPlacing)
            return;
        GameObject newObj = Instantiate(treeHologram, transform);
        newObj.GetComponent<SpriteRenderer>().sprite = tree.GetComponent<FruitTree>().sprite;
        BoxCollider2D box = newObj.AddComponent<BoxCollider2D>();
        box.isTrigger = true;
        currentHologram = newObj;
        objectToPlace = tree;
        Player.Instance.interact = new PlaceTree();
        isPlacing = true;
    }

    [ContextMenu("Exit Build Mode")]
    public void ExitBuildMode()
    {
        Destroy(currentHologram);
        objectToPlace = null;
        currentHologram = null;
        Player.Instance.interact = new Closest();
        isPlacing = false;
    }

    public void Place()
    {
        if (!canPlace)
        {
            return;
        }
        GameObject newObj = Instantiate(objectToPlace);
        newObj.transform.position = currentHologram.transform.position;
        ExitBuildMode();
    }

    void HologramToMouse()
    {
        Vector3 lookVector = Camera.main.ScreenToWorldPoint(PlayerInputManager.Instance.GetAimInput());
        lookVector.z = 0f;
        Vector3 direction = (lookVector - new Vector3(transform.position.x, transform.position.y, 0f)).normalized;
        currentHologram.transform.position = transform.position + (direction * 2f);
    }

    void CheckValidity()
    {
        GameObject[] objects = FruitTree.All
            .Select(c => c.gameObject)
            .ToArray();

        if (IsOverlapping(objects))
        {
            canPlace = false;
            currentHologram.GetComponent<SpriteRenderer>().color = Color.red;
        }
        else
        {
            canPlace = true;
            currentHologram.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }

    bool IsOverlapping(GameObject[] objects)
    {
        Collider2D col = currentHologram.GetComponent<Collider2D>();
        foreach (GameObject obj in objects)
        {
            if (col.bounds.Intersects(obj.GetComponent<Collider2D>().bounds))
            {
                return true;
            }
        }
        return false;
    }
}
