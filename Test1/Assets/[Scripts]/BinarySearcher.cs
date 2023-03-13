using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//File:        BinarySearcher.cs
//Project:     Midterm Test (Test 1)
//College:     Canadore College
//Course #:    GDD-403
//Prof:        Tom Tsiliopoulos
//Date:        2023-03-12
//Author:      Justin Logan
//Student #:   A00066979
//Doing binary search stuff in here.

public class BinarySearcher : MonoBehaviour
{
    //Self instance
    public static BinarySearcher Instance;

    //Searching cards
    public List<GameObject> cardsToSearch;

    //Are we doing binary search?
    bool working = false;

    //Our index
    private int _index = -2;

    //Key, min, max for binary search
    private int key, min, max;

    //Queue for steps, used later
    private Queue<Vector2> steps;

    //Get the index back if needed later.
    public int Index
    {
        get
        {
            return
                Index;
        }
    }
    private void Start()
    {
        Instance = this;
        cardsToSearch = new List<GameObject>();
        steps = new Queue<Vector2>();
    }
    //Setup for the binary search
    public void DoBinarySearch(Card c, GameObject[] cs)
    {
        if (working) return;
        key = c.GetAbsValue();
        cardsToSearch = new List<GameObject>(cs);
        min = 0;
        max = cardsToSearch.Count;
        _index = -1;
        working = true;

        foreach (GameObject g in cardsToSearch)
        {
            g.SetActive(true);
        }

        BinarySearchRecursive(key, min, max);
        Invoke("DoSteps", 1.0f);
    }

    //Do the actual search itself.
    public void BinarySearchRecursive(int card, int min, int max)
    {

        int mid = (min + max) / 2;
        if (min > max)
        {
            _index = -1;
            working = false;
            Debug.Log("Did not find");
        }
        else
        {
            if (card == cardsToSearch[mid].GetComponent<Card>().GetAbsValue())
            {
                _index = mid;
                working = false;
                Debug.Log("Found at position " + _index + " With:" + card + ":" + SortingSearchingSceneController.Instance.selected.GetComponent<Card>().GetAbsValue());
                steps.Enqueue(new Vector2(-1, _index - 1));
                steps.Enqueue(new Vector2(_index+1, this.max));
            }
            else if (card < cardsToSearch[mid].GetComponent<Card>().GetAbsValue())
            {
                steps.Enqueue(new Vector2(mid, max));
                max = mid - 1;
                BinarySearchRecursive(card, min, max);
            }
            else
            {
                steps.Enqueue(new Vector2(min, mid));
                min = mid + 1;
                BinarySearchRecursive(card, min, max);
            }
        }
    }
    //Repeat the steps to show we searched.
    public void DoSteps()
    {
        Vector2 step = steps.Dequeue();

        int start = (int)step.x, end = (int)step.y;

        for (int i = start; i <= end; i++)
        {
            if (i >= 0 && i < SortingSearchingSceneController.Instance.cardObjectReferences.Count)
            {
                SortingSearchingSceneController.Instance.cardObjectReferences[i].gameObject.SetActive(false);
                Debug.Log(start + ":" + i + ":" + end);
            }
        }
        if (steps.Count > 0)
        {
            if (step.x == -1)
            {
                Invoke("DoSteps", 0.001f);
            }
            else
            {
                Invoke("DoSteps", 1.0f);
            }
        }
    }
}