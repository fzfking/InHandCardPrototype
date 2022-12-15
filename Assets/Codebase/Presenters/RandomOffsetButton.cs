using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Codebase.Helpers;
using Codebase.Models;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Codebase.Presenters
{
    public class RandomOffsetButton : MonoBehaviour, IPointerClickHandler
    {
        private const int MinimalOffset = -2;
        private const int MaximalOffset = 10;
        [SerializeField] private Image Icon;
        [SerializeField] private bool IsLoopingBySelf;
        private bool _isOffsetting;
        private SortedCardsContainer _container;
        private List<Card> _cards;

        public void Init(SortedCardsContainer sortedCardsContainer)
        {
            _container = sortedCardsContainer;
            _cards = _container.Cards.OfType<Card>().ToList();
            ObserveCardsChanges(_cards);
        }

        private void ObserveCardsChanges(IEnumerable<Card> cards)
        {
            foreach (var card in cards)
            {
                card.AnyFieldChanged += (statType, previousValue) =>
                    ObserveCardFieldChanges(statType, previousValue, card);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            var tween = DOTween.Sequence(this);
            if (TryAnimateError(tween)) return;

            AnimateColorChange(tween);
            StartCoroutine(OffsetCardValuesWithDelay());
        }

        private bool TryAnimateError(Sequence tween)
        {
            if (_isOffsetting)
            {
                Icon.color = Color.red;

                tween.Append(transform.DOPunchScale(Constants.PunchScaleDefault, Constants.Durations.PunchDuration));
                tween.Insert(0, Icon.DOColor(Color.white, Constants.Durations.ColorSmoothDuration));
                return true;
            }

            return false;
        }

        private void AnimateColorChange(Sequence tween)
        {
            tween.Append(Icon.DOColor(Color.grey, 0.2f));
            tween.Append(Icon.DOColor(Color.white, 0.2f));
        }

        private IEnumerator OffsetCardValuesWithDelay()
        {
            UpdateCardsArray();
            
            if (_cards == null || _cards.Count == 0)
            {
                yield break;
            }
            _isOffsetting = true;

            var cards = _container.Cards.OfType<Card>().ToArray();
            foreach (var card in cards)
            {
                var statType = GetRandomStatType();
                var offset = Random.Range(MinimalOffset, MaximalOffset);

                var animationTime = CalculateAnimationTime(offset, card);

                card.OffsetStat(statType, offset);
                yield return new WaitForSeconds(animationTime);
            }


            if (IsLoopingBySelf)
            {
                StartCoroutine(OffsetCardValuesWithDelay());
                yield break;
            }

            _isOffsetting = false;
        }

        private static StatType GetRandomStatType()
        {
            return (StatType)Random.Range((int)StatType.Health, ((int)StatType.Mana) + 1);
        }

        private void UpdateCardsArray()
        {
            var newCards = _container.Cards.OfType<Card>().Where(card => !_cards.Contains(card));
            var enumerable = newCards as Card[] ?? newCards.ToArray();
            ObserveCardsChanges(enumerable);
            _cards.AddRange(enumerable);
        }

        private static float CalculateAnimationTime(int offset, Card card)
        {
            var animationTime = Constants.Durations.TextAnimationDuration * Math.Abs(offset) +
                                Constants.Durations.PopupAnimationDuration;
            var value = card.GetStat(StatType.Health);
            if (value + offset <= 1)
            {
                animationTime = Constants.Durations.TextAnimationDuration * Math.Abs(value) +
                                Constants.Durations.PopupAnimationDuration;
            }

            return animationTime;
        }

        private void ObserveCardFieldChanges(StatType statType, int previousValue, Card card)
        {
            if (statType == StatType.Health && card.GetStat(statType) < 1)
            {
                DeleteCard(card);
            }
        }

        private void DeleteCard(Card card)
        {
            _cards.Remove(card);
            card.Dispose();
        }

        private void OnDestroy()
        {
            foreach (var card in _cards.ToArray())
            {
                DeleteCard(card);
            }
        }
    }
}