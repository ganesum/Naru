using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Naru.WPF.MVVM
{
    /// <summary>
    /// Provides support for extracting property information based on a property expression.
    /// Taken from PRISM.
    /// </summary>
    public static class PropertyExtensions
    {
        /// <summary>
        /// Extracts the property name from a property expression.
        /// </summary>
        /// <typeparam name="T">The object type containing the property specified in the expression.</typeparam><param name="propertyExpression">The property expression (e.g. p =&gt; p.PropertyName)</param>
        /// <returns>
        /// The name of the property.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">Thrown if the <paramref name="propertyExpression"/> is null.</exception><exception cref="T:System.ArgumentException">Thrown when the expression is:<br/>
        ///                 Not a <see cref="T:System.Linq.Expressions.MemberExpression"/><br/>
        ///                 The <see cref="T:System.Linq.Expressions.MemberExpression"/> does not represent a property.<br/>
        ///                 Or, the property is static.
        ///             </exception>
        public static string ExtractPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException("propertyExpression");
            }

            var memberExpression = propertyExpression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new ArgumentException("PropertySupport_NotMemberAccessExpression_Exception", "propertyExpression");
            }

            var propertyInfo = memberExpression.Member as PropertyInfo;
            if (propertyInfo == null)
            {
                throw new ArgumentException("PropertySupport_ExpressionNotProperty_Exception", "propertyExpression");
            }

            if (propertyInfo.GetGetMethod(true).IsStatic)
            {
                throw new ArgumentException("PropertySupport_StaticExpression_Exception", "propertyExpression");
            }
            else
            {
                return memberExpression.Member.Name;
            }
        }
    }
}