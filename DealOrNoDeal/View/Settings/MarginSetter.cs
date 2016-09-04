using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace DealOrNoDeal.View.Settings
{
    /// <summary>
    ///     Responsible for setting margins that can be applied to various GUI controls.
    /// </summary>
    public class MarginSetter
    {
        #region Data members

        /// <summary>
        ///     Using a DependencyProperty as the backing store for Margin.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty MarginProperty =
            DependencyProperty.RegisterAttached("Margin", typeof(Thickness), typeof(MarginSetter),
                new PropertyMetadata(new Thickness(), CreateThicknessForChildren));

        #endregion

        #region Methods

        /// <summary>
        ///     Gets the margin.
        /// </summary>
        /// <param name="control">The GUI object that getting the margin from.</param>
        /// <returns></returns>
        public static Thickness GetMargin(DependencyObject control)
        {
            return (Thickness) control.GetValue(MarginProperty);
        }

        /// <summary>
        ///     Sets the margin.
        /// </summary>
        /// <param name="control">The GUI object to set the margin of.</param>
        /// <param name="thickness">The value.</param>
        public static void SetMargin(DependencyObject control, Thickness thickness)
        {
            control.SetValue(MarginProperty, thickness);
        }

        /// <summary>
        /// Sets the thickness for all the children elements of the control.
        /// </summary>
        /// <param name="sender">Control that was sender of the event.</param>
        /// <param name="e">Event args</param>
        public static void CreateThicknessForChildren(object sender, DependencyPropertyChangedEventArgs e)
        {
            var panel = sender as Panel;

            if (panel == null)
            {
                return;
            }

            foreach (var child in panel.Children)
            {
                var frameworkElement = child as FrameworkElement;

                if (frameworkElement == null)
                {
                    continue;
                }

                frameworkElement.Margin = GetMargin(panel);
            }
        }

        #endregion
    }
}