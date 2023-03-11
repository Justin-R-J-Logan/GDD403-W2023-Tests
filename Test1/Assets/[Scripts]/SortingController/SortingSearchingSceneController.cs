using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class SortingSearchingSceneController : MonoBehaviour
{

    //Singleton instance of this controller. Multiples will glitch the system.
    public static SortingSearchingSceneController Instance { get; private set; }

    public Transform selectedPosition;

    public List<Transform> fourByFourLayout;
    public List<GameObject> cardObjectReferences;

    public GameObject selected;

    public StandardDeck deck;

    public float SortingSpeed = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        deck = new StandardDeck(); // example of composition
        cardObjectReferences = new List<GameObject>();
    }

    public void SelectCard(GameObject cardObject)
    {
        if (selected != null)
        {
            Destroy(selected);
        }
        selected = GameObject.Instantiate(cardObject);
        selected.transform.position = selectedPosition.position;
        selected.GetComponent<Card>().isSelected = true;
    }

    public bool CanSelect = true;
    Queue<Vector2> swaps = new Queue<Vector2>();
    
    public void BubbleSortQueue()
    {
        CanSelect = false;
        //swaps.Enqueue(new Vector2(j, j+1));
        //cardObjectReferences[j].GetComponent<Card>().GetAbsValue()

        List<int> lst = new List<int>();

        for (int i = 0; i < cardObjectReferences.Count; i++)
        {
            lst.Add(cardObjectReferences[i].GetComponent<Card>().GetAbsValue());
        }

        int[] intArray = lst.ToArray();
        int cardCount = cardObjectReferences.Count;

        for (int i = 0; i < cardCount - 1; i++)
        {
            for (int i2 = 0; i2 < cardCount - i - 1; i2++)
            {
                if (intArray[i2] > intArray[i2 + 1])
                {
                    // Enqueue the swap for later, swap the array items now.
                    swaps.Enqueue(new Vector2(i2, i2+1));
                    int temp = intArray[i2];
                    intArray[i2] = intArray[i2 + 1];
                    intArray[i2 + 1] = temp;
                }
            }
        }

    }


    //External call for PerformQueue
    public void PerformQueueExternal()
    {
        Invoke("PerformQueue", SortingSpeed);
    }

    //Internal, recursive call for Perform Enqueue
    public void PerformQueue()
    {
        if (swaps.Count < 1)
        {
            CanSelect = true;
            return;
        }
        Vector2 swap = swaps.Dequeue();
        SwapPositions((int)swap.x, (int)swap.y);
        Invoke("PerformQueue", SortingSpeed);
    }


    public void SwapPositions(int one, int two)
    {
        Vector3 position1 = new Vector3(fourByFourLayout[one].transform.position.x, fourByFourLayout[one].transform.position.y, fourByFourLayout[one].transform.position.z);
        Vector3 position2 = new Vector3(fourByFourLayout[two].transform.position.x, fourByFourLayout[two].transform.position.y, fourByFourLayout[two].transform.position.z);

        cardObjectReferences[one].transform.position = position2;
        cardObjectReferences[two].transform.position = position1;

        GameObject GOTWO = cardObjectReferences[two];
        cardObjectReferences[two] = cardObjectReferences[one];
        cardObjectReferences[one] = GOTWO;
    }


    public void BinarySearch()
    {
        if (selected == null)
        {
            return;
        }
        else
        {
            PerformBinarySearchRecursive(cardObjectReferences, selected, 0, cardObjectReferences.Count-1);
        }
    }

    public int PerformBinarySearchRecursive(List<GameObject> cards, GameObject card, int low, int high)
    {
        return 0;
    }
}
