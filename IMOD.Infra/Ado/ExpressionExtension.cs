using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IMOD.Infra.Ado
{
    public static class ExpressionExtension
    {
        public static string GetMethodName<T>(this Expression<Func<T>> expression)
        {
            var body = (MethodCallExpression)expression.Body;
            return body.Method.Name;
        }

        public static ReadOnlyCollection<Expression> GetInnerArguments<T>(this Expression<Func<T>> expression)
        {
            var body = (MethodCallExpression)expression.Body;
            return body.GetInnerArguments();
        }

        public static ReadOnlyCollection<Expression> GetInnerArguments(this MethodCallExpression expression)
        {
            var args = new List<Expression>();

            var arguments = expression.Arguments;

            foreach (var a in arguments)
            {
                var methodCallExpression = a.AsMethodCallExpression();
                if (methodCallExpression != null && methodCallExpression.Arguments.Count > 0)
                {
                    args.AddRange(methodCallExpression.GetInnerArguments());
                }
                else
                {
                    args.Add(a);
                }
            }

            return new ReadOnlyCollection<Expression>(args.Where(a => a.NodeType == ExpressionType.MemberAccess).ToList());
        }

        public static MethodCallExpression AsMethodCallExpression(this Expression expression)
        {
            return expression as MethodCallExpression;
        }
    }
}
