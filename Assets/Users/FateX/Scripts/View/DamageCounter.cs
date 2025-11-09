using Lean.Pool;
using TMPro;
using UnityEngine;
using DG.Tweening;

namespace Users.FateX.Scripts.View
{
    public class DamageCounter : MonoBehaviour, IPoolable
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private CanvasGroup _canvasGroup;

        private Tween _tween;
        private Vector3 _baseScale;

        private void Awake()
        {
            _baseScale = transform.localScale; // сохраняем исходный масштаб
        }

        public void Init(float damage)
        {
            _text.text = ((int)(damage * 10)).ToString();

            _tween?.Kill();

            // Сбрасываем состояние
            _canvasGroup.alpha = 0f;
            transform.localScale = Vector3.zero;

            // Анимация появления и растворения
            _tween = DOTween.Sequence()
                // 1. Появление до 130% от базового масштаба
                .Append(transform.DOScale(_baseScale * 1.3f, 0.15f).SetEase(Ease.OutBack))
                // 2. Лёгкое уменьшение до исходного
                .Append(transform.DOScale(_baseScale, 0.1f).SetEase(Ease.InOutSine))
                // 3. Fade-in + задержка + подъем и растворение
                .Join(_canvasGroup.DOFade(1f, 0.05f))
                .AppendInterval(0.4f)
                .Append(transform.DOMoveY(transform.position.y + 1f, 0.6f).SetEase(Ease.OutCubic))
                .Join(_canvasGroup.DOFade(0f, 0.6f))
                // 4. Возврат в пул
                .OnComplete(() => LeanPool.Despawn(gameObject));
        }

        public void OnSpawn() { }

        public void OnDespawn()
        {
            _tween?.Kill();
            _text.text = "";
            _canvasGroup.alpha = 0f;
            transform.localScale = _baseScale;
        }
    }
}