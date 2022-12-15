using System;
using System.Collections;
using System.Collections.Generic;
using Codebase.Helpers;
using Codebase.Models;
using Codebase.Models.Interfaces;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Codebase.Presenters
{
    public class CardPresenter : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public IReadonlyCard Card => _card;
        public bool IsAnimating { get; private set; }
        [SerializeField] private TextMeshProUGUI HealthLabel;
        [SerializeField] private TextMeshProUGUI ManaLabel;
        [SerializeField] private TextMeshProUGUI StrengthLabel;
        [SerializeField] private TextMeshProUGUI NameLabel;
        [SerializeField] private TextMeshProUGUI DescriptionLabel;
        [SerializeField] private Image Icon;
        [SerializeField] private Outline Outline;
        private IReadonlyCard _card;
        private Vector3 _savedPosition;
        private Vector3 _savedRotation;
        private bool _isTweening;
        private Coroutine _followingRoutine;
        private Dictionary<StatType, TextMeshProUGUI> _statViews;
        private SortedCardsContainer _sortedCardsContainer;

        private void Awake()
        {
            _statViews = new Dictionary<StatType, TextMeshProUGUI>
            {
                [StatType.Health] = HealthLabel,
                [StatType.Strength] = StrengthLabel,
                [StatType.Mana] = ManaLabel
            };
        }

        public void SetParent(SortedCardsContainer sortedCardsContainer)
        {
            if (_sortedCardsContainer != null)
            {
                _sortedCardsContainer.RemoveCard(this);
            }

            _sortedCardsContainer = sortedCardsContainer;
            _sortedCardsContainer.AddCard(this);
        }

        public void Link(IReadonlyCard card)
        {
            if (_card != null)
            {
                Unlink();
            }

            _card = card;
            _card.AnyFieldChanged += AnimateCount;
            UpdateView();
        }

        private void Unlink()
        {
            if (_card == null)
            {
                return;
            }

            _card.AnyFieldChanged -= AnimateCount;
        }

        public void Translate(Vector3 position)
        {
            _isTweening = true;
            transform.DOMove(position, Vector2.Distance(transform.position, position) / 5).OnComplete(() =>
            {
                _savedPosition = transform.position;
                _isTweening = false;
            });
        }

        public void Rotate(Vector3 rotation)
        {
            transform.DORotate(rotation, Constants.Durations.RotateDuration);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_followingRoutine != null || _isTweening || IsAnimating)
            {
                return;
            }

            Outline.Enable();
            _followingRoutine = StartCoroutine(FollowMouse());
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_followingRoutine != null)
            {
                StopCoroutine(_followingRoutine);
                Outline.Disable();
                _followingRoutine = null;
            }

            Translate(_savedPosition);
            Rotate(_savedRotation);
        }

        private void AnimateCount(StatType statType, int previousValue)
        {
            IsAnimating = true;
            MoveUp();
            var value = _card.GetStat(statType);
            StartCoroutine(TextAnimation(_statViews[statType], previousValue, value, statType));
        }

        private IEnumerator TextAnimation(TextMeshProUGUI textMeshProUGUI, int previousValue, int value,
            StatType statType)
        {
            var colorBackup = textMeshProUGUI.color;
            bool isMoreThanPreviousValue = value > previousValue;
            textMeshProUGUI.DOColor(isMoreThanPreviousValue ? Color.green : Color.red, 0f);
            for (int i = previousValue; i != value; i += isMoreThanPreviousValue ? 1 : -1)
            {
                textMeshProUGUI.transform.DOPunchScale(Constants.PunchScaleMedium, Constants.Durations.PunchDuration);
                textMeshProUGUI.text = i.ToString();
                if (statType == StatType.Health && i == 0)
                {
                    ReturnDefaults(textMeshProUGUI, colorBackup);
                    ReturnToSavedPosition();
                    yield return new WaitUntil(() => _isTweening == false);
                    MoveDown(() => gameObject.SetActive(false));
                    yield break;
                }

                yield return new WaitForSeconds(Constants.Durations.TextAnimationDuration);
            }

            ReturnDefaults(textMeshProUGUI, colorBackup);
            yield return new WaitUntil(() => _isTweening == false);
            ReturnToSavedPosition();
        }

        private void ReturnDefaults(TextMeshProUGUI textMeshProUGUI, Color colorBackup)
        {
            textMeshProUGUI.DOColor(colorBackup, 0f);
            IsAnimating = false;
        }

        public void SetNewSavedPosition(Vector3 position) => _savedPosition = position;
        public void SetNewSavedRotation(Vector3 rotation) => _savedRotation = rotation;

        public void MoveUp() => Move(true);
        public void MoveDown(Action onComplete = null) => Move(false, onComplete);

        private void Move(bool toUpper, Action onComplete = null)
        {
            if (_isTweening)
            {
                return;
            }

            _isTweening = true;
            var newPosition = transform.position;
            newPosition += UpPositionOffset() * (toUpper ? 1 : -1);
            transform.DOMove(newPosition, Constants.Durations.PopupAnimationDuration).OnComplete(() =>
            {
                _isTweening = false;
                onComplete?.Invoke();
            });
        }

        private Vector3 UpPositionOffset()
        {
            return transform.up * (transform.localScale.y * 1.1f);
        }

        public void ReturnToSavedPosition()
        {
            Translate(_savedPosition);
        }

        public void ReturnToSavedRotation()
        {
            Rotate(_savedRotation);
        }

        private IEnumerator FollowMouse()
        {
            _savedRotation = transform.eulerAngles;
            transform.eulerAngles = Vector3.zero;
            while (true)
            {
                var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                transform.position = new Vector3(mousePosition.x, mousePosition.y, -50);
                yield return null;
            }
        }

        private void UpdateView()
        {
            HealthLabel.text = _card.GetStat(StatType.Health).ToString();
            ManaLabel.text = _card.GetStat(StatType.Mana).ToString();
            StrengthLabel.text = _card.GetStat(StatType.Strength).ToString();
            NameLabel.text = _card.Name;
            DescriptionLabel.text = _card.Description;
            Icon.sprite = _card.Icon;
        }

        private void OnDestroy()
        {
            Unlink();
        }
    }
}