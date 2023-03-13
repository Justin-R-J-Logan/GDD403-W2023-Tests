using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

//File:        SortingSearchingSceneController.cs
//Project:     Midterm Test (Test 1)
//College:     Canadore College
//Course #:    GDD-403
//Prof:        Tom Tsiliopoulos
//Date:        2023-03-12
//Author:      Justin Logan
//Student #:   A00066979
//Sorting and Searching Game Controller

public class SortingSearchingSceneController : MonoBehaviour
{

    //Singleton instance of this controller. Multiples will glitch the system
    //only one should ever be used
    public static SortingSearchingSceneController Instance { get; private set; }

    //Selected or Random card position (to place card), for searching
    public Transform selectedPosition;

    //Layout and object references
    public List<Transform> gameLayout;
    public List<GameObject> cardObjectReferences;

    //Selected card
    public GameObject selected;

    //Standard deck, copied from GameController
    public StandardDeck deck;

    //Search speed and default
    public float SortingSpeed = 0.5f;

    //Can we select a card, or is a swap or search happening
    public bool CanSelect = true;

    //Queue up all the swaps required, then after we are done we do all of them
    Queue<Vector2> swaps = new Queue<Vector2>();
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;            //Create the singleton
        deck = new StandardDeck(); 
        cardObjectReferences = new List<GameObject>(); //Create empty list
    }
    //Selects a card, and destroyes previously selected card.
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
    

    //Bubble sort algorithm for the cards
    public void BubbleSortQueue()
    {
        CanSelect = false;

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
    //Performs all the on-screen swaps in order with a sorting speed.
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

    //Swaps two card positions and lets them drop to the table again
    public void SwapPositions(int one, int two)
    {
        Vector3 position1 = new Vector3(gameLayout[one].transform.position.x, gameLayout[one].transform.position.y, gameLayout[one].transform.position.z);
        Vector3 position2 = new Vector3(gameLayout[two].transform.position.x, gameLayout[two].transform.position.y, gameLayout[two].transform.position.z);

        cardObjectReferences[one].transform.position = position2;
        cardObjectReferences[two].transform.position = position1;

        GameObject GOTWO = cardObjectReferences[two];
        cardObjectReferences[two] = cardObjectReferences[one];
        cardObjectReferences[one] = GOTWO;
    }

    //Binary search caller
    //Unused, see BinarySearch class.
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
    //Unused, see BinarySearch class.
    public int PerformBinarySearchRecursive(List<GameObject> cards, GameObject card, int low, int high)
    {
        return 0;
    }
}
