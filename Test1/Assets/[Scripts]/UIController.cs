using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;

public class UIController : MonoBehaviour
{
    //Singleton instance of this controller. Multiples will glitch the system.
    public static UIController Instance { get; private set; }

    //UI Stuff from in-class excersize 
    public TMP_Dropdown difficultyDropdown;
    public Difficulty difficulty;
    public Transform cardParent;
    public GameObject startButton;

    //Text objects for outcome and score.
    private TMP_Text outcomeText;
    private TMP_Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        difficultyDropdown = FindObjectOfType<TMP_Dropdown>();
        difficulty = Difficulty.EASY;
        cardParent = GameObject.Find("[CARDS]").transform;

        //Setup my UI extensions
        SetupExtraUI();
    }

    public void OnDifficulty_Changed()
    {
        difficulty = (Difficulty)difficultyDropdown.value;
    }

    public void OnStartButton_Pressed()
    {
        //Play shuffle sound.
        AudioController.Instance.PlaySound(CLIPS.SHUFFLE);

        //Reset match score
        GameController.Instance.currentMatches = 0;
        GameController.Instance.currentFails = 0; 

        //Setup difficulty.
        switch (difficulty)
        {
            case Difficulty.EASY: 
                Deal(GameController.Instance.twoByFourLayout, 8);
                GameController.Instance.maxMatches = GameController.Instance.twoByFourLayout.Count / 2;
                break;
            case Difficulty.NORMAL:
                Deal(GameController.Instance.fourByFourLayout, 16);
                GameController.Instance.maxMatches = GameController.Instance.fourByFourLayout.Count / 2;
                break;
            case Difficulty.HARD:
                Deal(GameController.Instance.sixBySixLayout, 36);
                GameController.Instance.maxMatches = GameController.Instance.sixBySixLayout.Count / 2;
                break;
        }
        startButton.SetActive(false);

        //Update Match Text
        UpdateScoreUI();
    }

    public void OnResetButton_Pressed()
    {
        GameController.Instance.deck.Clean();

        foreach (Transform child in cardParent)
        {
            Destroy(child.gameObject);
        }

        GameController.Instance.deck.Initialize();
        GameController.Instance.beingDelayed = false;

        startButton.SetActive(true);

        outcomeText.gameObject.SetActive(false);

        scoreText.gameObject.SetActive(false);
    }

    private void Deal(List<Transform> layout, int cardNumber)
    {
        for (var i = 0; i < layout.Count; i++)
        {
            var randomIndex = Random.Range(0, layout.Count);
            if (i != randomIndex)
            {
                (layout[i], layout[randomIndex]) = (layout[randomIndex], layout[i]);
            }
        }

        for (var i = 0; i < cardNumber; i++)
        {
            if (i == 0 || i % 2 == 0)
            {
                var firstCard = GameController.Instance.deck.Pop();
                firstCard.SetActive(true);
                firstCard.GetComponent<Card>().Flip();
                var secondCard = Instantiate(firstCard);
                secondCard.transform.SetParent(cardParent);
                firstCard.transform.position = layout[i].position;
                secondCard.transform.position = layout[i + 1].position;
            }
        }
    }


    private void SetupExtraUI()
    {
        outcomeText = GameObject.Find("WLL").GetComponent<TMP_Text>();
        outcomeText.gameObject.SetActive(false);

        scoreText = GameObject.Find("Progress").GetComponent<TMP_Text>();
        scoreText.gameObject.SetActive(false);
    }

    public void UpdateScoreUI()
    {
        scoreText.text = "" + GameController.Instance.currentMatches + " of " + GameController.Instance.maxMatches + " matches!";
        scoreText.gameObject.SetActive(true);
    }

    //Shows win loss text and plays relevant sound.
    public void SetOutcome(bool hasWon) 
    {
        if(hasWon)
        {
            outcomeText.text = "Winner!";
            AudioController.Instance.PlaySound(CLIPS.WIN);
        }
        else
        {
            outcomeText.text = "Lost Match.";
            AudioController.Instance.PlaySound(CLIPS.LOSE);
        }
        outcomeText.gameObject.SetActive(true);
    }
}
