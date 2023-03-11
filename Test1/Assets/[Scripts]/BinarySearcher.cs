using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinarySearcher : MonoBehaviour
{
    public static BinarySearcher Instance;

    public List<GameObject> cardsToSearch;

    bool working = false;

    private int _index = -2;

    private int key, min, max;

    private Queue<Vector2> steps;

    /*
    public int GetIndex()
    {
        if (working)
        {
            return -666;
        }
        else
        {
            return _index;
        }
        
    }



    public void DoBinarySearch(Card c, GameObject[] cs)
    {
        if (working) return;
        this.key = c.GetAbsValue();
        this.cardsToSearch = new List<GameObject>(cs);
        min = 0;
        max = cardsToSearch.Count;
        _index = -1;
        working = true;

        foreach(GameObject g in cardsToSearch)
        {
            g.SetActive(true);
        }

        Invoke("BinarySearchRecursive", 1.0f);
    }
    public void BinarySearchRecursive()
    {
        int mid = (min + max) / 2;
        if (min > max)
        {
            _index = -1;
            working = false;

        }
        else
        {
            if (key == cardsToSearch[mid].GetComponent<Card>().GetAbsValue())
            {
                _index = mid;
                working = false;
                for (int i = 0; i < cardsToSearch.Count; i++)
                {
                    if (i == _index)
                    {
                        cardsToSearch[i].SetActive(true);
                    }
                    else
                    {
                        cardsToSearch[i].SetActive(false);
                    }
                }
            }
            else if (key < cardsToSearch[mid].GetComponent<Card>().GetAbsValue())
            {
                for (int i = mid; i < max; i++)
                {
                    cardsToSearch[i].SetActive(false);
                }
                max = mid - 1;
                Invoke("BinarySearchRecursive", 1.0f);
            }
            else
            {
                for (int i = mid; i > min; i--)
                {
                    cardsToSearch[i].SetActive(false);
                }
                min = mid + 1;
                Invoke("BinarySearchRecursive", 1.0f);
            }
        }
    }*/

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