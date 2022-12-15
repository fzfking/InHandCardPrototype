using System.Collections.Generic;
using System.Linq;
using Codebase.Infrastructure.Services.Factories;
using Codebase.Models;
using Codebase.Models.Interfaces;
using UnityEngine;

namespace Codebase.Presenters
{
    public class SortedCardsContainer : MonoBehaviour
    {
        private const float DefaultLeftZAngle = 25f;
        private const float MinimalOffsetBetweenCards = 0.1f;
        public IReadonlyCard[] Cards => _activeCards.Keys.ToArray();
        [SerializeField] private Transform CardsContainer;
        private float _bordersLength;
        private Dictionary<IReadonlyCard, CardPresenter> _activeCards;
        private CardPresenterFactory _cardPresenterFactory;

        public void Awake()
        {
            _activeCards = new Dictionary<IReadonlyCard, CardPresenter>();
            _bordersLength = CardsContainer.transform.lossyScale.x;
        }

        public SortedCardsContainer Init(CardPresenterFactory cardPresenterFactory)
        {
            _cardPresenterFactory = cardPresenterFactory;
            return this;
        }

        public void AddCard(IReadonlyCard card)
        {
            if (_cardPresenterFactory == null)
            {
                return;
            }
            var freeCard = _cardPresenterFactory.GetFreeCardPresenter();
            freeCard.Link(card);
            freeCard.gameObject.SetActive(true);
            freeCard.SetParent(this);
        }
        
        public void AddCard(CardPresenter card)
        {
            _activeCards.Add(card.Card, card);
            Resort();
            MoveAllCardsToSavedValues(card);
            card.Card.Disposed += OnCardDisposed;
        }

        public void RemoveCard(CardPresenter cardPresenter)
        {
            cardPresenter.Card.Disposed -= OnCardDisposed;
            _activeCards.Remove(cardPresenter.Card);
            Resort();
            MoveAllCardsToSavedValues();
        }

        public void Resort()
        {
            var activeCards = _activeCards.Values.ToArray();
            int cardsAmount = _activeCards.Count;
            var offsetBetweenCards = CalculatePositionOffset(cardsAmount);
            float rotationOffset = CalculateRotationOffset(cardsAmount);
            UpdateCardPositions(cardsAmount, activeCards, offsetBetweenCards, rotationOffset);
        }

        private void UpdateCardPositions(int cardsAmount, CardPresenter[] activeCards, float offsetBetweenCards,
            float rotationOffset)
        {
            for (int i = 0; i < cardsAmount; i++)
            {
                var transformPosition = activeCards[i].transform.position;
                var transformRotation = activeCards[i].transform.localEulerAngles;
                transformPosition = CalculateNewPosition(transformPosition, i, offsetBetweenCards);
                transformRotation = CalculateNewRotation(CardsContainer, transformRotation, i, rotationOffset);
                SetNewCardPosition(activeCards[i], transformPosition);
                SetNewCardRotation(activeCards[i], transformRotation);
            }
        }

        private static float CalculateRotationOffset(int cardsAmount)
        {
            if (cardsAmount == 1)
            {
                return DefaultLeftZAngle;
            }

            return DefaultLeftZAngle / (cardsAmount / 2f);
        }

        private float CalculatePositionOffset(int cardsAmount)
        {
            if (cardsAmount == 1)
            {
                return _bordersLength / 2;
            }

            return _bordersLength / cardsAmount + MinimalOffsetBetweenCards;
        }

        public void MoveAllCardsToSavedValues(CardPresenter except = null)
        {
            foreach (var card in _activeCards.Values)
            {
                if (card == except)
                {
                    continue;
                }
                card.ReturnToSavedPosition();
                card.ReturnToSavedRotation();
            }
        }

        private void SetNewCardRotation(CardPresenter activeCard, Vector3 transformRotation)
        {
            activeCard.SetNewSavedRotation(transformRotation);
        }

        private void SetNewCardPosition(CardPresenter activeCard, Vector3 transformPosition)
        {
            activeCard.SetNewSavedPosition(transformPosition);
        }

        private static Vector3 CalculateNewRotation(Transform holder, Vector3 transformRotation, int i, float rotationOffset)
        {
            transformRotation.z = holder.transform.localEulerAngles.z + DefaultLeftZAngle - ((i+1) * rotationOffset);
            return transformRotation;
        }

        private Vector3 CalculateNewPosition(Vector3 transformPosition, int i, float offsetBetweenCards)
        {
            transformPosition.z = -i;
            transformPosition.x = (-_bordersLength / 2 + (i+1 * offsetBetweenCards));
            transformPosition.y = CardsContainer.transform.position.y;
            return transformPosition;
        }

        private void OnCardDisposed(Card card)
        {
            _activeCards.Remove(card);
            Resort();
            MoveAllCardsToSavedValues();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.TryGetComponent(out CardPresenter cardPresenter))
            {
                if (!_activeCards.ContainsValue(cardPresenter))
                {
                    if (cardPresenter.IsAnimating)
                    {
                        return;
                    }
                    cardPresenter.SetParent(this);
                }
            }
        }

        public void Dispose()
        {
            Destroy(this);
        }

        private void OnDestroy()
        {
            _activeCards.Clear();
        }
    }
}