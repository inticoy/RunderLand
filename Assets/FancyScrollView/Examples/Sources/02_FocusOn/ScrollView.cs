/*
 * FancyScrollView (https://github.com/setchi/FancyScrollView)
 * Copyright (c) 2020 setchi
 * Licensed under MIT (https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
 */

using System;
using System.Collections.Generic;
using UnityEngine;
using EasingCore;

namespace FancyScrollView.Example02
{
    class ScrollView : FancyScrollView<ItemData, Context>
    {
        [SerializeField] GameLoadManager GLM;
        [SerializeField] int idx;
        [SerializeField] Scroller scroller = default;
        [SerializeField] GameObject cellPrefab = default;
        [SerializeField] GameObject cellPrefab2 = default;
        [SerializeField] GameObject cellPrefab3 = default;

        Action<int> onSelectionChanged;

        protected override GameObject CellPrefab => cellPrefab;
        protected override GameObject CellPrefab2 => cellPrefab2;
        protected override GameObject CellPrefab3 => cellPrefab3;

        protected override void Initialize()
        {
            base.Initialize();

            Context.OnCellClicked = SelectCell;

            scroller.OnValueChanged(UpdatePosition);
            scroller.OnSelectionChanged(UpdateSelection);
        }

        void UpdateSelection(int index)
        {
            if (Context.SelectedIndex == index)
            {
                return;
            }
            GLM.SetIdx(index);
            idx = index;
            Context.SelectedIndex = index;
            Refresh();

            onSelectionChanged?.Invoke(index);
        }

        public void UpdateData(IList<ItemData> items)
        {
            UpdateContents(items);
            scroller.SetTotalCount(items.Count);
        }

        public void OnSelectionChanged(Action<int> callback)
        {
            onSelectionChanged = callback;
        }

        public void SelectNextCell()
        {
            SelectCell(Context.SelectedIndex + 1);
        }

        public void SelectPrevCell()
        {
            SelectCell(Context.SelectedIndex - 1);
        }

        public void SelectCell(int index)
        {
            if (index < 0 || index >= ItemsSource.Count || index == Context.SelectedIndex)
            {
                return;
            }

            UpdateSelection(index);
            scroller.ScrollTo(index, 0.35f, Ease.OutCubic);
        }
    }
}
