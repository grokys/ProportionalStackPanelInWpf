﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace WpfApp1
{
    /// <summary>
    /// Represents a control that lets the user change the size of elements in a <see cref="ProportionalStackPanel"/>.
    /// </summary>
    public class ProportionalStackPanelSplitter : Thumb
    {
        private Size _previousParentSize;

        /// <summary>
        /// Defines the Proportion attached property.
        /// </summary>
        public static readonly DependencyProperty ProportionProperty =
            DependencyProperty.RegisterAttached(
                "Proportion",
                typeof(double),
                typeof(ProportionalStackPanelSplitter),
                new FrameworkPropertyMetadata(double.NaN));

        /// <summary>
        /// Gets the value of the Proportion attached property on the specified control.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <returns>The Proportion attached property.</returns>
        public static double GetProportion(FrameworkElement control)
        {
            return (double)control.GetValue(ProportionProperty);
        }

        /// <summary>
        /// Sets the value of the Proportion attached property on the specified control.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">The value of the Proportion property.</param>
        public static void SetProportion(FrameworkElement control, double value)
        {
            control.SetValue(ProportionProperty, value);
        }

        /// <summary>
        /// Defines the <see cref="Thickness"/> property.
        /// </summary>
        public static readonly DependencyProperty ThicknessProperty =
            DependencyProperty.Register(
                nameof(Thickness),
                typeof(double),
                typeof(ProportionalStackPanelSplitter),
                new FrameworkPropertyMetadata(4.0));

        /// <summary>
        /// Gets or sets the thickness (height or width, depending on orientation).
        /// </summary>
        /// <value>The thickness.</value>
        public double Thickness
        {
            get => (double)GetValue(ThicknessProperty);
            set => SetValue(ThicknessProperty, value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProportionalStackPanelSplitter" /> class.
        /// </summary>
        public ProportionalStackPanelSplitter()
        {
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (GetPanel() is ProportionalStackPanel panel)
            {
                if (panel.Orientation == Orientation.Vertical)
                {
                    return new Size(0, Thickness);
                }
                else
                {
                    return new Size(Thickness, 0);
                }
            }

            return new Size();
        }

        ///// <inheritdoc/>
        //protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        //{
        //    base.OnAttachedToVisualTree(e);

        //    var panel = GetPanel();
        //    if (panel == null)
        //    {
        //        return;
        //    }

        //    _previousParentSize = panel.Bounds.Size;

        //    UpdateHeightOrWidth();
        //}

        private void SetTargetProportion(double dragDelta)
        {
            //var target = GetTargetElement();
            //var panel = GetPanel();
            //if (target == null || panel == null)
            //{
            //    return;
            //}

            //var children = panel.GetChildren();

            //int index = children.IndexOf(this) + 1;

            //var child = children[index];

            //var targetElementProportion = GetProportion(target);
            //var neighbourProportion = GetProportion(child);

            //var dProportion = dragDelta / (panel.Orientation == Orientation.Vertical ? panel.Bounds.Height : panel.Bounds.Width);

            //if (targetElementProportion + dProportion < 0)
            //{
            //    dProportion = -targetElementProportion;
            //}

            //if (neighbourProportion - dProportion < 0)
            //{
            //    dProportion = +neighbourProportion;
            //}

            //targetElementProportion += dProportion;
            //neighbourProportion -= dProportion;

            //var minProportion = GetValue(DockProperties.MinimumProportionSizeProperty) / (panel.Orientation == Orientation.Vertical ? panel.Bounds.Height : panel.Bounds.Width);

            //if (targetElementProportion < minProportion)
            //{
            //    dProportion = targetElementProportion - minProportion;
            //    neighbourProportion += dProportion;
            //    targetElementProportion -= dProportion;

            //}
            //else if (neighbourProportion < minProportion)
            //{
            //    dProportion = neighbourProportion - minProportion;
            //    neighbourProportion -= dProportion;
            //    targetElementProportion += dProportion;
            //}

            //SetProportion(target, targetElementProportion);

            //SetProportion(child, neighbourProportion);

            //panel.InvalidateArrange();
        }

        private void UpdateHeightOrWidth()
        {
            //if (GetPanel() is ProportionalStackPanel panel)
            //{
            //    if (panel.Orientation == Orientation.Vertical)
            //    {
            //        Height = Thickness;
            //        Width = double.NaN;
            //        Cursor = new Cursor(StandardCursorType.SizeNorthSouth);
            //        PseudoClasses.Add(":horizontal");
            //    }
            //    else
            //    {
            //        Width = Thickness;
            //        Height = double.NaN;
            //        Cursor = new Cursor(StandardCursorType.SizeWestEast);
            //        PseudoClasses.Add(":vertical");
            //    }
            //}
        }

        private ProportionalStackPanel GetPanel()
        {
            //if (Parent is ContentPresenter presenter)
            //{
            //    if (presenter.GetVisualParent() is ProportionalStackPanel panel)
            //    {
            //        return panel;
            //    }
            //}
            //else if (this.GetVisualParent() is ProportionalStackPanel psp)
            //{
            //    return psp;
            //}

            return null;
        }

        private Control GetTargetElement()
        {
            //if (Parent is ContentPresenter presenter)
            //{
            //    if (!(presenter.GetVisualParent() is Panel panel))
            //    {
            //        return null;
            //    }

            //    int index = panel.Children.IndexOf(Parent);
            //    if (index > 0 && panel.Children.Count > 0)
            //    {
            //        if (panel.Children[index - 1] is ContentPresenter contentPresenter)
            //        {
            //            return contentPresenter.Child as Control;
            //        }
            //        else
            //        {
            //            return null;
            //        }
            //    }
            //}
            //else
            //{
            //    var panel = GetPanel();
            //    if (panel != null)
            //    {
            //        int index = panel.Children.IndexOf(this);
            //        if (index > 0 && panel.Children.Count > 0)
            //        {
            //            return panel.Children[index - 1] as Control;
            //        }
            //    }
            //}

            return null;
        }
    }
}
