using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Card : MonoBehaviour, IComparable<Card>
{

    const int MAX_VAL = 13;

    [Header("Card Properties")]
    public string rankName;
    public string suit;
    private int suitval;
    public int value;
    public bool isFaceUp;
    public bool isSelected;


    [Header("Selection Properties")]
    public SelectionOutline selectionOutline;
    public Color selectionColour;

    private Renderer cRenderer;

    void Start()
    {
        selectionOutline = FindObjectOfType<SelectionOutline>();
        cRenderer = GetComponent<Renderer>();
        Initialize();
    }

    private void Update()
    {
        cRenderer.material.SetColor("_Color", isSelected ? selectionColour : Color.white);
    }

    public void Flip()
    {
        isFaceUp = !isFaceUp;
        transform.position = new Vector3(transform.position.x, 7.5f, transform.position.z);
        transform.localRotation = Quaternion.Euler(0.0f, 0.0f, (isFaceUp) ? 0.0f : 180.0f); // ternary operator
        isSelected = isFaceUp;
    }

    public string toString()
    {
        return $"{rankName} of {suit}s";
    }

    public void Initialize()
    {
        //Remove "(Clone)". 
        //For some reason when spawned in they have clone and this breaks the split
        if (this.gameObject.name.Contains("("))
        {
            this.gameObject.name = this.gameObject.name.Substring(0, this.gameObject.name.IndexOf("("));
        }

        isSelected = false;
        isFaceUp = false;

        var suitRankStrings = name.Split("_");

        var numberWords = new[]
        {
            "Zero", "Ace", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Jack", "Queen", "King"
        };

        suit = suitRankStrings[0];

        switch (suitRankStrings[1])
        {
            case "A":
                value = 1;
                break;
            case "J":
                value = 11;
                break;
            case "Q":
                value = 12;
                break;
            case "K":
                value = 13;
                break;
            default:
                suit = suitRankStrings[0];
                Int32.TryParse(suitRankStrings[1], out value); // convert to an int

                break;
        }

        rankName = numberWords[value];

        UpdateSuitVal();
    }

    void OnMouseEnter()
    {
        EnableSelectionOutline();
    }

    void OnMouseOver()
    {
        if(GameController.Instance != null)
        {
            if (Input.GetMouseButtonDown(0) && !isSelected && !GameController.Instance.beingDelayed)
            {
                Flip();
                GameController.Instance.SelectCard(this.gameObject);
            }
        } else
        {
            if (Input.GetMouseButtonDown(0) && SortingSearchingSceneController.Instance.CanSelect && !isSelected)
            {
                SortingSearchingSceneController.Instance.SelectCard(this.gameObject);
            }
        }
    }

    void OnMouseExit()
    {
        DisableSelectionOutline();
    }

    // External Code

    private void EnableSelectionOutline()
    {
        Ray ray = selectionOutline.cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            selectionOutline.TargetRenderer = hit.transform.GetComponent<Renderer>();
            if (selectionOutline.lastTarget == null) selectionOutline.lastTarget = selectionOutline.TargetRenderer;
            if (selectionOutline.SelectionMode == SelMode.AndChildren)
            {
                if (selectionOutline.ChildrenRenderers != null)
                {
                    Array.Clear(selectionOutline.ChildrenRenderers, 0, selectionOutline.ChildrenRenderers.Length);
                }

                selectionOutline.ChildrenRenderers = hit.transform.GetComponentsInChildren<Renderer>();
            }


            if (selectionOutline.TargetRenderer != selectionOutline.lastTarget || !selectionOutline.Selected)
            {
                selectionOutline.SetTarget();
            }

            selectionOutline.lastTarget = selectionOutline.TargetRenderer;
        }
        else
        {
            selectionOutline.TargetRenderer = null;
            selectionOutline.lastTarget = null;
            if (selectionOutline.Selected)
            {
                selectionOutline.ClearTarget();
            }
        }
    }

    private void DisableSelectionOutline()
    {
        if (selectionOutline.Selected)
        {
            selectionOutline.ClearTarget();
        }
    }

    private int UpdateSuitVal()
    {
        suitval = 0;
        switch(suit.ToLower())
        {
            case "diamond":
                suitval = 1;
                break;
            case "club":
                suitval = 2;
                break;
            case "heart":
                suitval = 3;
                break;
        }
        return suitval;
    }

    //Get value for sorting from Suit and Value
    public int GetAbsValue()
    {
        return suitval + (value * MAX_VAL);
    }

    public int CompareTo(Card other)
    {
        if (this.GetAbsValue() < other.GetAbsValue())
        {
            return 1;
        }
        else if (this.GetAbsValue() > other.GetAbsValue())
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }
}

