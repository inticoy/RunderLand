/*
 * FancyScrollView (https://github.com/setchi/FancyScrollView)
 * Copyright (c) 2020 setchi
 * Licensed under MIT (https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
 */

using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace FancyScrollView.Example02
{
    class Example02 : MonoBehaviour
    {
        [SerializeField] ScrollView scrollView = default;

        void Start()
        { 
            var items = Enumerable.Range(0, 3)
                .Select(i => new ItemData(i == 0 ? "싱글플레이" : i == 1 ? "기록과 플레이" : "멀티플레이"))
                .ToArray();

            scrollView.UpdateData(items);
            scrollView.SelectCell(0);
        }
    }
}
