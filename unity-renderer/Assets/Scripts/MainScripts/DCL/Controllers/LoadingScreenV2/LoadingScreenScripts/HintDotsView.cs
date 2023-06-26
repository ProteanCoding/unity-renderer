using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

namespace DCL.LoadingScreen.V2
{
    public class HintDotsView : MonoBehaviour
    {
        [SerializeField] private RectTransform[] dotsList;
        private List<Image> dotsImages;
        private int currentDotIndex = 0;
        private int previousDotIndex = 0;
        private int dotsCount = 0;

        private Vector2 bigSize = new Vector2(12, 12);
        private Vector2 smallSize = new Vector2(8, 8);
        private Color activeColor = new Color(1, 0.1764706f, 0.3333333f);
        private Color inactiveColor = Color.white;

        public void Initialize(int hintsAmount)
        {
            if (dotsList == null)
                throw new WarningException("HintDotsView - DotsList is not assigned!");

            if (dotsCount <= 0)
                throw new WarningException("HintDotsView - DotsCount is not valid!");

            dotsImages = new List<Image>();
            for (int i = 0; i < dotsCount; i++)
            {
                dotsList[i].gameObject.SetActive(true);
                dotsImages.Add(dotsList[i].GetComponent<Image>());
            }

            currentDotIndex = 0;
            UpdateActiveDot(currentDotIndex);
        }

        public void UpdateActiveDot(int index)
        {
            if (index < 0 || index > dotsCount)
                return;

            previousDotIndex = currentDotIndex;
            currentDotIndex = index;

            // Set current dot to active state
            dotsList[index].sizeDelta = bigSize;
            dotsImages[index].GetComponent<Image>().color = activeColor;

            if (currentDotIndex != previousDotIndex)
            {
                // Set previous dot to inactive state
                dotsList[previousDotIndex].sizeDelta = smallSize;
                dotsImages[previousDotIndex].GetComponent<Image>().color = inactiveColor;
            }
        }
    }
}
