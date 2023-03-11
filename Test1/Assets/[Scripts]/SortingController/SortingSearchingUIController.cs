using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingSearchingUIController : MonoBehaviour
{
    public static SortingSearchingUIController Instance;

    public Transform cardParent;

    public bool sorted;

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

    public void OnStartButton_Pressed()
    {
        AudioController.Instance.PlaySound(CLIPS.SHUFFLE);

        Deal(SortingSearchingSceneController.Instance.fourByFourLayout, 16);

        startButton.GetComponent<UnityEngine.UI.Button>().interactable = false;
        resetButton.GetComponent<UnityEngine.UI.Button>().interactable = true;
        sortButton.GetComponent<UnityEngine.UI.Button>().interactable = true;
    }
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
    public void OnSortButton_Pressed()
    {
        SortingSearchingSceneController.Instance.BubbleSortQueue();
        
        sortButton.GetComponent<UnityEngine.UI.Button>().interactable = false;
        searchButton.GetComponent<UnityEngine.UI.Button>().interactable = true;

        SortingSearchingSceneController.Instance.PerformQueueExternal();
        sorted = true;
    }
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
            SortingSearchingSceneController.Instance.SelectCard(c);
            SortingSearchingSceneController.Instance.selected.SetActive(true);
        }

        resetSearchButton.GetComponent<UnityEngine.UI.Button>().interactable = true;

        BinarySearcher.Instance.DoBinarySearch(SortingSearchingSceneController.Instance.selected.GetComponent<Card>(), SortingSearchingSceneController.Instance.cardObjectReferences.ToArray());

    }

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
