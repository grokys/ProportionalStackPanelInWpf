using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    /// <summary>
    /// A Panel that stacks controls either horizontally or verically, with proportional resizing.
    /// </summary>
    public class ProportionalStackPanel : Panel
    {
        private static readonly Size SizeInfinity = new Size(double.PositiveInfinity, double.PositiveInfinity);

        /// <summary>
        /// Defines the <see cref="Orientation"/> property.
        /// </summary>
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register(
                nameof(Orientation),
                typeof(Orientation),
                typeof(ProportionalStackPanel),
                new FrameworkPropertyMetadata(Orientation.Vertical));

        /// <summary>
        /// Gets or sets the orientation in which child controls will be layed out.
        /// </summary>
        public Orientation Orientation
        {
            get => (Orientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        internal IList<FrameworkElement> GetChildren()
        {
            var result = new List<FrameworkElement>();

            foreach (var c in Children)
            {
                result.Add((FrameworkElement)c);
            }

            return result;
        }

        /// <inheritdoc/>
        //protected override void ChildrenChanged(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    base.ChildrenChanged(sender, e);

        //    switch (e.Action)
        //    {
        //        case NotifyCollectionChangedAction.Add:
        //            foreach (var item in e.NewItems.OfType<IControl>())
        //            {
        //                ProportionalStackPanelSplitter.SetProportion(item as IControl, double.NaN);
        //            }
        //            break;
        //    }
        //}

        private void AssignProportions()
        {
            double assignedProportion = 0;
            int unassignedProportions = 0;

            foreach (Control element in GetChildren())
            {
                if (!(element is ProportionalStackPanelSplitter))
                {
                    var proportion = ProportionalStackPanelSplitter.GetProportion(element);

                    if (double.IsNaN(proportion))
                    {
                        unassignedProportions++;
                    }
                    else
                    {
                        assignedProportion += proportion;
                    }
                }
            }

            if (unassignedProportions > 0)
            {
                var toAssign = assignedProportion;
                foreach (Control element in GetChildren().Where(c => double.IsNaN(ProportionalStackPanelSplitter.GetProportion(c))))
                {
                    if (!(element is ProportionalStackPanelSplitter))
                    {
                        ProportionalStackPanelSplitter.SetProportion(element, (1.0 - toAssign) / unassignedProportions);
                        assignedProportion += (1.0 - toAssign) / unassignedProportions;
                    }
                }
            }

            if (assignedProportion < 1)
            {
                var numChildren = (double)GetChildren().Where(c => !(c is ProportionalStackPanelSplitter)).Count();

                var toAdd = (1.0 - assignedProportion) / numChildren;

                foreach (var child in GetChildren().Where(c => !(c is ProportionalStackPanelSplitter)))
                {
                    ProportionalStackPanelSplitter.SetProportion(child, ProportionalStackPanelSplitter.GetProportion(child) + toAdd);
                }
            }
            else if (assignedProportion > 1)
            {
                var numChildren = (double)GetChildren().Where(c => !(c is ProportionalStackPanelSplitter)).Count();

                var toRemove = (assignedProportion - 1.0) / numChildren;

                foreach (var child in GetChildren().Where(c => !(c is ProportionalStackPanelSplitter)))
                {
                    ProportionalStackPanelSplitter.SetProportion(child, ProportionalStackPanelSplitter.GetProportion(child) - toRemove);
                }
            }
        }

        private double GetTotalSplitterThickness()
        {
            var result = GetChildren().OfType<ProportionalStackPanelSplitter>().Sum(c => c.Thickness);

            return double.IsNaN(result) ? 0 : result;
        }

        /// <inheritdoc/>
        protected override Size MeasureOverride(Size constraint)
        {
            if (constraint == SizeInfinity)
            {
                var stackSize = new Size();

                AssignProportions();

                foreach (Control element in GetChildren())
                {
                    element.Measure(SizeInfinity);

                    stackSize.Width += element.DesiredSize.Width;
                    stackSize.Height += element.DesiredSize.Height;
                }

                return stackSize;
            }

            double usedWidth = 0.0;
            double usedHeight = 0.0;
            double maximumWidth = 0.0;
            double maximumHeight = 0.0;

            var splitterThickness = GetTotalSplitterThickness();

            AssignProportions();

            // Measure each of the Children
            foreach (Control element in GetChildren())
            {
                // Get the child's desired size
                Size remainingSize = new Size(
                    Math.Max(0.0, constraint.Width - usedWidth - splitterThickness),
                    Math.Max(0.0, constraint.Height - usedHeight - splitterThickness));

                var proportion = ProportionalStackPanelSplitter.GetProportion(element);

                if (!(element is ProportionalStackPanelSplitter))
                {
                    switch (Orientation)
                    {
                        case Orientation.Horizontal:
                            var c1 = constraint;
                            c1.Width = Math.Max(0, (constraint.Width - splitterThickness) * proportion);
                            element.Measure(c1);
                            break;

                        case Orientation.Vertical:
                            var c2 = constraint;
                            c2.Height = Math.Max(0, (constraint.Height - splitterThickness) * proportion);
                            element.Measure(c2);
                            break;
                    }
                }
                else
                {
                    element.Measure(remainingSize);
                }

                var desiredSize = element.DesiredSize;

                // Decrease the remaining space for the rest of the children
                switch (Orientation)
                {
                    case Orientation.Horizontal:
                        maximumHeight = Math.Max(maximumHeight, usedHeight + desiredSize.Height);

                        if (element is ProportionalStackPanelSplitter)
                        {
                            usedWidth += desiredSize.Width;
                        }
                        else
                        {
                            usedWidth += Math.Max(0, (constraint.Width - splitterThickness) * proportion);
                        }
                        break;
                    case Orientation.Vertical:
                        maximumWidth = Math.Max(maximumWidth, usedWidth + desiredSize.Width);

                        if (element is ProportionalStackPanelSplitter)
                        {
                            usedHeight += desiredSize.Height;
                        }
                        else
                        {
                            usedHeight += Math.Max(0, (constraint.Height - splitterThickness) * proportion);
                        }
                        break;
                }
            }

            maximumWidth = Math.Max(maximumWidth, usedWidth);
            maximumHeight = Math.Max(maximumHeight, usedHeight);
            return new Size(maximumWidth, maximumHeight);
        }

        /// <inheritdoc/>
        protected override Size ArrangeOverride(Size arrangeSize)
        {
            double left = 0.0;
            double top = 0.0;
            double right = 0.0;
            double bottom = 0.0;

            var splitterThickness = GetTotalSplitterThickness();

            // Arrange each of the Children
            var children = GetChildren();
            int index = 0;

            foreach (var element in children)
            {
                // Determine the remaining space left to arrange the element
                Rect remainingRect = new Rect(
                    left,
                    top,
                    Math.Max(0.0, arrangeSize.Width - left - right),
                    Math.Max(0.0, arrangeSize.Height - top - bottom));

                // Trim the remaining Rect to the docked size of the element
                // (unless the element should fill the remaining space because
                // of LastChildFill)
                if (index < children.Count())
                {
                    var desiredSize = element.DesiredSize;
                    var proportion = ProportionalStackPanelSplitter.GetProportion(element);

                    switch (Orientation)
                    {
                        case Orientation.Horizontal:
                            if (element is ProportionalStackPanelSplitter)
                            {
                                left += desiredSize.Width;
                                remainingRect.Width = desiredSize.Width;
                            }
                            else
                            {
                                remainingRect.Width = Math.Max(0, (arrangeSize.Width - splitterThickness) * proportion);
                                left += Math.Max(0, (arrangeSize.Width - splitterThickness) * proportion);
                            }
                            break;
                        case Orientation.Vertical:
                            if (element is ProportionalStackPanelSplitter)
                            {
                                top += desiredSize.Height;
                                remainingRect.Height = desiredSize.Height;
                            }
                            else
                            {
                                remainingRect.Height = Math.Max(0, (arrangeSize.Height - splitterThickness) * proportion);
                                top += Math.Max(0, (arrangeSize.Height - splitterThickness) * proportion);
                            }
                            break;
                    }
                }

                if (!(element is ProportionalStackPanelSplitter))
                {
                    element.Measure(remainingRect.Size);
                }

                element.Arrange(remainingRect);
                index++;
            }

            return arrangeSize;
        }
    }
}
