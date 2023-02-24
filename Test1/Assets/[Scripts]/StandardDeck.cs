using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using UnityEngine;

public class StandardDeck : Stack<GameObject>
{
    // Private Instance Members (fields)
    private readonly string[] _suits;
    private readonly string[] _ranks;
    private List<GameObject> _cardPrefabs;

    private Transform _cardParent;
    private Transform _cardPrefabParent;

    // Constructor
    public StandardDeck()
    {
        _suits = new[] { "Club", "Diamond", "Heart", "Spade" };
        _ranks = new[] { "2", "3", "4", "5", "6", "7", "8", "9", "10", "A", "J", "Q", "K" };
        _cardPrefabs = new List<GameObject>(); // creates an empty container of List<GameObject> type
        _cardParent = GameObject.Find("[CARDS]").transform;
        _cardPrefabParent = GameObject.Find("[CARD PREFABS]").transform;

        Load();
        Initialize();
    }

    // Public Properties (getters and setters)

    // Public Methods
    public void Initialize()
    {

        Shuffle();

        foreach (var card in _cardPrefabs)
        {
            var tempCard = MonoBehaviour.Instantiate(card);
            tempCard.SetActive(false);
            tempCard.name = card.name;
            tempCard.transform.SetParent(_cardParent);
            Push(tempCard);
        }
        
    }

    public void Display()
    {
        foreach (var card in this)
        {
            MonoBehaviour.print(card);
        }
    }

    public void Clean()
    {
        foreach (var card in this)
        {
            MonoBehaviour.Destroy(card);
        }

        Clear();
    }

    // Private Methods
    private void Load()
    {
        for (var i = 0; i < _suits.Length; i++)
        {
            for (var j = 0; j < _ranks.Length; j++)
            {
                var cardPrefab = Resources.Load<GameObject>($"Prefabs/Cards/{_suits[i]}_{_ranks[j]}");
                var card = MonoBehaviour.Instantiate(cardPrefab);
                card.SetActive(false);
                card.transform.SetParent(_cardPrefabParent);
                card.name = $"{_suits[i]}_{_ranks[j]}";
                _cardPrefabs.Add(card);
            }
        }
    }

    private void Shuffle()
    {
        // Shuffle the _cardPrefabs List

        for (var i = 0; i < _cardPrefabs.Count; i++)
        {
            var randomIndex = Random.Range(0, _cardPrefabs.Count);
            if (i != randomIndex)
            {
                (_cardPrefabs[i], _cardPrefabs[randomIndex]) = (_cardPrefabs[randomIndex], _cardPrefabs[i]);
            }
        }
    }

    //Get Remaining Cards
    public int CardsRemaining()
    {
        return Count;
    }
    //Draw card at slot [0]
    public GameObject DrawNextCard()
    {
        return DrawSpecific(0);
    }

    //Draw Random card
    public GameObject DrawRandomCard()
    {
        int index = Random.Range(0, this.Count);
        return DrawSpecific(index);
    }

    //Draw specific - makes the top two methods use less code
    public GameObject DrawSpecific(int index)
    {
        //Make list of cards
        List<GameObject> listOfCards = this.ToList();
        //Get the card we want
        GameObject card = listOfCards[index];
        //Remove that card
        listOfCards.RemoveAt(0);
        //Clear stack
        this.Clear();
        //Add all back to stack
        foreach (GameObject c in listOfCards)
        {
            this.Push(c);
        }
        //Return selected card;
        return card;
    }
}
