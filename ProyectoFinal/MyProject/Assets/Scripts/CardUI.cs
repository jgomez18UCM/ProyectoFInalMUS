using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace Combat
{
public class CardUI : MonoBehaviour
{
    public Card card;
	public TMP_Text cardTitleText;
	public TMP_Text cardDescriptionText;
	public TMP_Text cardCostText;
    public Image cardImage;
    public GameObject discardEffect;
    BattleSceneManager battleSceneManager;
    Animator animator;

    private void Awake()
    {
        battleSceneManager = FindObjectOfType<BattleSceneManager>();
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        animator.Play("HoverOffCard");
    }

    public void LoadCard(Card c)
    {
        card = c;

        gameObject.GetComponent<RectTransform>().localScale=new Vector3(1,1,1);

        cardTitleText.text = card.GetCardName();
        cardDescriptionText.text = card.GetCardDescription();
        cardCostText.text = card.GetCardCost().ToString();

        cardImage.sprite = card.cardIcon;
    }
    public void SelectCard()
    {
        battleSceneManager.selectedCard = this;
    }
    public void DeselectCard()
    {
        battleSceneManager.selectedCard = null;
        animator.Play("HoverOffCard");
    }
    public void HoverCard()
    {
        if (battleSceneManager.selectedCard == null)
            animator.Play("HoverOnCard");
    }
    public void DropCard()
    {
        if (battleSceneManager.selectedCard == null)
            animator.Play("HoverOffCard");
    }

    public void HandleEndDrag()
    {
        if (battleSceneManager.energy<card.GetCardCost())
            return;

        if (card.getCardType() == Card.CardType.Attack)
        {
            battleSceneManager.PlayCard(this);
            animator.Play("HoverOffCard");
        }

        else if (card.getCardType() != Card.CardType.Attack)
        {
            animator.Play("HoverOffCard");
            battleSceneManager.PlayCard(this);
        }
    }
}
}
