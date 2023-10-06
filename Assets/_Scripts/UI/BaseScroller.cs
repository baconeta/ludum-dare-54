using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GardenMeta
{
    public class BaseScroller : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [SerializeField] private RectTransform m_navigationTransform;
        [SerializeField] private float m_transitionTime;
        [SerializeField] private float m_transitionEasePower;
        [SerializeField] private float m_screenWidth;
        [SerializeField] private float m_swipeMinDistance;
        [SerializeField] private bool m_canWrap;
        [SerializeField] private GameObject m_scrollLeftButton;
        [SerializeField] private GameObject m_scrollRightButton;
        [SerializeField] private float m_yAnchorOffset = 0.0f;

        private bool m_dragStarted;
        private Vector2 m_dragStartPosition;
        private int m_currentScreenIndex;

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            if (!m_dragStarted)
            {
                StopAllCoroutines();
                m_dragStarted = true;
                m_dragStartPosition = m_navigationTransform.anchoredPosition;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (m_dragStarted)
            {
                var oldPosX = m_navigationTransform.anchoredPosition.x;
                var minX = -(m_navigationTransform.childCount - (m_canWrap ? 0 : 1)) * m_screenWidth;
                var newPosX = Mathf.Clamp(oldPosX + eventData.delta.x, minX, 0.0f);
                m_navigationTransform.anchoredPosition = new Vector2(newPosX, m_yAnchorOffset);
            }
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            if (m_dragStarted)
            {
                m_dragStarted = false;

                var diffX = m_navigationTransform.anchoredPosition.x - m_dragStartPosition.x;
                var diffScreens = 0;

                if (Mathf.Abs(diffX) > m_swipeMinDistance)
                {
                    diffScreens = diffX < 0 ? 1 : -1;
                }

                Scroll(m_currentScreenIndex + diffScreens);
            }
        }

        public virtual void ScrollImmediate(int index)
        {
            var clampedIndex = HandleEdges(index);
            m_currentScreenIndex = clampedIndex;
            m_navigationTransform.anchoredPosition = new Vector2(-m_currentScreenIndex * m_screenWidth * Vector2.right.x, m_yAnchorOffset);
            SetScreensActive(m_currentScreenIndex, m_currentScreenIndex, true);
        }

        public virtual void Scroll(int index)
        {
            var clampedIndex = HandleEdges(index);
            StopAllCoroutines();
            StartCoroutine(ScrollToIndex(clampedIndex));
        }

        private int HandleEdges(int index)
        {
            var maxIndex = m_navigationTransform.childCount - 1;
            var newIndex = Mathf.Clamp(index, 0, maxIndex);

            if (m_scrollLeftButton != null)
            {
                m_scrollRightButton.SetActive(newIndex > 0);
            }

            if (m_scrollLeftButton != null)
            {
                m_scrollRightButton.SetActive(newIndex < maxIndex);
            }

            if (m_canWrap)
            {
                return index > maxIndex ? 0 : newIndex;
            }

            return newIndex;
        }

        protected virtual IEnumerator ScrollToIndex(int index)
        {
            SetScreensActive(m_currentScreenIndex, index);
            m_currentScreenIndex = index;
            var timer = 0f;
            var from = m_navigationTransform.anchoredPosition.x;
            var to = -m_currentScreenIndex * m_screenWidth;

            while (timer < m_transitionTime)
            {
                timer += Time.deltaTime;
                var t = Mathf.Clamp01(1.0f - Mathf.Pow(1.0f - timer / m_transitionTime, m_transitionEasePower));
                m_navigationTransform.anchoredPosition = new Vector2(Mathf.Lerp(from, to, t) * Vector2.right.x, m_yAnchorOffset);

                yield return new WaitForEndOfFrame();
            }

            SetScreensActive(m_currentScreenIndex, m_currentScreenIndex);
        }

        protected virtual void SetScreensActive(int indexA, int indexB, bool isImmediate = false)
        {
            var padding = 1;
            var startIndex = (indexA < indexB ? indexA : indexB) - padding;
            var endIndex = (indexA > indexB ? indexA : indexB) + padding;

            for (var i = 0; i < m_navigationTransform.childCount; i++)
            {
                m_navigationTransform.GetChild(i).gameObject.SetActive(i >= startIndex && i <= endIndex);
            }
        }

        public virtual void AddScreen(Transform screen, bool position = false)
        {
            screen.localPosition = m_navigationTransform.childCount * m_screenWidth * Vector3.right;
            screen.SetParent(m_navigationTransform, position);
        }

        public void OnScrollButtonClicked(bool isLeft)
        {
            var index = m_currentScreenIndex + (isLeft ? -1 : 1);
            Scroll(index);
        }

        public void HideButtons()
        {
            m_scrollLeftButton.transform.localScale = Vector3.zero;
            m_scrollRightButton.transform.localScale = Vector3.zero;
        }

        public IEnumerator ShowButtons()
        {
            var scaleTime = 0.5f;
            var scaleTweenLeft = LeanTween.scale(m_scrollLeftButton.gameObject, Vector3.one, scaleTime);
            scaleTweenLeft.setEaseInCubic();
            var scaleTweenRight = LeanTween.scale(m_scrollRightButton.gameObject, Vector3.one, scaleTime);
            scaleTweenRight.setEaseInCubic();
            yield return new WaitForSeconds(scaleTime);
        }

        public void EnableScrollLooping(bool enable)
        {
            m_canWrap = enable;
        }

        protected void ScrollToNext()
        {
            var clampedIndex = HandleEdges(m_currentScreenIndex + 1);
            StopAllCoroutines();
            StartCoroutine(ScrollToIndex(clampedIndex));
        }

        public virtual void RemoveChild(int i)
        {
            m_navigationTransform.GetChild(i).gameObject.SetActive(false);
            for (int j = i + 1; j < m_navigationTransform.childCount; j++)
            {
                // Move the rest of the children to the left
                var position = m_navigationTransform.GetChild(j).localPosition;
                position.x -= m_screenWidth;
                m_navigationTransform.GetChild(j).localPosition = position;
            }

            Destroy(m_navigationTransform.GetChild(i).gameObject);
        }

        protected GameObject GetChild(int i)
        {
            return m_navigationTransform.GetChild(i).gameObject;
        }
    }
}