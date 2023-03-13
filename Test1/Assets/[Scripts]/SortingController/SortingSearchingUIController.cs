using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//File:        SortingSearchingUIController.cs
//Project:     Midterm Test (Test 1)
//College:     Canadore College
//Course #:    GDD-403
//Prof:        Tom Tsiliopoulos
//Date:        2023-03-12
//Author:      Justin Logan
//Student #:   A00066979
//Sorting and Searching UI controller
public class SortingSearchingUIController : MonoBehaviour
{
    //Singleton instance of this controller. Multiples will glitch the system
    //only one should ever be used
    public static SortingSearchingUIController Instance;

    //Card parent, copied from UIController.
    //Used to place all the cards into.
    public Transform cardParent;

    //Have we actually sorted yet?
    //Probably unnecessary, but an extra precautiuon
    public bool sorted;

    //References to all of our buttons
    public GameObject startButton;
    public GameObject resetButton;
    public GameObject sortButton;
    public GameObject searchButton;
    public GameObject resetSearchButton;

    public void Start()
    {
        Instance = this;
        cardParent = GameObject.Find("[CARDS]").transform;
    }
    //Starts the game, plays shuffle, deals cards.
    public void OnStartButton_Pressed()
    {
        AudioController.Instance.PlaySound(CLIPS.SHUFFLE);

        Deal(SortingSearchingSceneController.Instance.gameLayout, 16);

        startButton.GetComponent<UnityEngine.UI.Button>().interactable = false;
        resetButton.GetComponent<UnityEngine.UI.Button>().interactable = true;
        sortButton.GetComponent<UnityEngine.UI.Button>().interactable = true;
    }
    //Deletes the game.
    public void OnResetButton_Pressed()
    {
        SortingSearchingSceneController.Instance.deck.Clean();

        foreach (Transform child in cardParent)
        {
            Destroy(child.gameObject);
        }

        SortingSearchingSceneController.Instance.deck.Initialize();

        startButton.GetComponent<UnityEngine.UI.Button>().interactable = true;
        sortButton.GetComponent<UnityEngine.UI.Button>().interactable = false;
        searchButton.GetComponent<UnityEngine.UI.Button>().interactable = false;
        resetButton.GetComponent<UnityEngine.UI.Button>().interactable = false;
        resetSearchButton.GetComponent<UnityEngine.UI.Button>().interactable = false;

        SortingSearchingSceneController.Instance.cardObjectReferences.Clear();
        Destroy(SortingSearchingSceneController.Instance.selected);
        sorted = false;
    }
    //Uses the bubble sort algorithm to sort cards, then does the sort.
    public void OnSortButton_Pressed()
    {
        SortingSearchingSceneController.Instance.BubbleSortQueue();
        
        sortButton.GetComponent<UnityEngine.UI.Button>().interactable = false;
        searchButton.GetComponent<UnityEngine.UI.Button>().interactable = true;

        SortingSearchingSceneController.Instance.PerformQueueExternal();
        sorted = true;
    }
    //Searches for a card. If one isn't chosen, choose a random card.
    public void OnSearchButton_Pressed()
    {
        if (!sorted) return;
        searchButton.GetComponent<UnityEngine.UI.Button>().interactable = false;
        if(SortingSearchingSceneController.Instance.selected == null)
        {
            List<GameObject> g = new List<GameObject>();
            foreach (Transform child in cardParent)
            {
                g.Add(child.gameObject);
            }
            GameObject c = GameObject.Instantiate(g[Random.Range(0, g.Count-1)]);
            c.GetComponent<Card>().Initialize();
            SortingSearchingSceneController.Instance.SelectCard(c);
            SortingSearchingSceneController.Instance.selected.SetActive(true);
            
        }

        resetSearchButton.GetComponent<UnityEngine.UI.Button>().interactable = true;

        BinarySearcher.Instance.DoBinarySearch(SortingSearchingSceneController.Instance.selected.GetComponent<Card>(), SortingSearchingSceneController.Instance.cardObjectReferences.ToArray());

    }
    //Reset the search so we can do the search with the same cards.
    public void OnResetSearchButton_Pressed()
    {
        Destroy(SortingSearchingSceneController.Instance.selected);
        foreach (GameObject card in SortingSearchingSceneController.Instance.cardObjectReferences)
        {
            card.SetActive(true);
        }
        resetSearchButton.GetComponent<UnityEngine.UI.Button>().interactable = false;
        searchButton.GetComponent<UnityEngine.UI.Button>().interactable = true;
    }

    private void Deal(List<Transform> layout, int cardNumber)
    {
        for (var i = 0; i < cardNumber; i++)
        {
            var card = SortingSearchingSceneController.Instance.deck.Pop();
            card.SetActive(true);
            card.GetComponent<Card>().Flip();
            card.transform.position = layout[i].position;
            card.GetComponent<Card>().Flip();

            SortingSearchingSceneController.Instance.cardObjectReferences.Add(card);
        }
    }
}
