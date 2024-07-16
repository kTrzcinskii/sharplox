﻿using System.Globalization;
using sharplox.Errors;
using sharplox.Expressions;
using sharplox.Tokens;

namespace sharplox.Services;

public class Interpreter : IExpressionVisitor<object?>
{
    public void Interpret(BaseExpression expression)
    {
        try
        {
            var value = Evaluate(expression);
            Console.WriteLine(Stringify(value));
        }
        catch (RuntimeException ex)
        {
            Lox.RuntimeError(ex);
        }
    }
    
    private object? Evaluate(BaseExpression expression)
    {
        return expression.Accept(this);
    }
    
    public object? VisitBinaryExpression(BinaryExpression binaryExpression)
    {
        var left = Evaluate(binaryExpression.Left);
        var right = Evaluate(binaryExpression.Right);

        switch (binaryExpression.Operator.Type)
        {
            case TokenType.MINUS:
                AssertNumberOperands(binaryExpression.Operator, left, right);
                return (double)left - (double)right;
            case TokenType.PLUS:
                if (left is double ld && right is double rd)
                    return ld + rd;
                if (left is string ls && right is string rs)
                    return ls + rs;
                throw new RuntimeException(binaryExpression.Operator, "Operands must be both strings or both numbers");
            case TokenType.SLASH:
                AssertNumberOperands(binaryExpression.Operator, left, right);
                AssertLegalDivision(binaryExpression.Operator, (double)left, (double)right);
                return (double)left / (double)right;
            case TokenType.STAR:
                AssertNumberOperands(binaryExpression.Operator, left, right);
                return (double)left * (double)right;
            case TokenType.GREATER:
                AssertNumberOperands(binaryExpression.Operator, left, right);
                return (double)left > (double)right;
            case TokenType.GREATER_EQUAL:
                AssertNumberOperands(binaryExpression.Operator, left, right);
                return (double)left >= (double)right;
            case TokenType.LESS:
                AssertNumberOperands(binaryExpression.Operator, left, right);
                return (double)left < (double)right;
            case TokenType.LESS_EQAUL:
                AssertNumberOperands(binaryExpression.Operator, left, right);
                return (double)left <= (double)right;
            case TokenType.EQUAL_EQUAL:
                return AreEqual(left, right);
            case TokenType.BANG_EQAUl:
                return !AreEqual(left, right);
        }

        // This code should never be reached
        return null;
    }

    public object? VisitGroupingExpression(GroupingExpression groupingExpression)
    {
        return Evaluate(groupingExpression.BaseExpression);
    }

    public object? VisitLiteralExpression(LiteralExpression literalExpression)
    {
        return literalExpression.Value;
    }

    public object? VisitUnaryExpression(UnaryExpression unaryExpression)
    {
        var right = Evaluate(unaryExpression.Right);

        switch (unaryExpression.Operator.Type)
        {
            case TokenType.MINUS:
                AssertNumberOperand(unaryExpression.Operator, right);
                return -(double)right;
            case TokenType.BANG:
                return !IsTruthy(right);
        }
        
        // This code should never be reached
        return null;
    }

    
    // Bool -> just value
    // Nil -> always false
    // Everything else -> always true
    private bool IsTruthy(object? value)
    {
        if (value == null)
            return false;
        if (value is bool b)
            return b;
        return true;
    }

    private bool AreEqual(object? lhs, object? rhs)
    {
        if (lhs == null && rhs == null)
            return true;
        if (lhs == null || rhs == null)
            return false;
        return lhs.Equals(rhs);
    }

    private void AssertNumberOperand(Token op, object? operand)
    {
        if (operand is double)
            return;
        throw new RuntimeException(op, "Operand must be a number.");
    }

    private void AssertNumberOperands(Token op, object? lhs, object? rhs)
    {
        if (lhs is double && rhs is double)
            return;
        throw new RuntimeException(op, "Operands must be numbers.");
    }

    private void AssertLegalDivision(Token op, double lhs, double rhs)
    {
        if (rhs != 0)
            return;
        throw new RuntimeException(op, "Division by 0 is forbidden.");
    }

    private string Stringify(object? value)
    {
        if (value == null)
            return "nil";

        if (value is double d)
        {
            var text = d.ToString(CultureInfo.InvariantCulture);
            if (text.EndsWith(".0"))
                text = text.Substring(0, text.Length - 2);
            return text;
        }
        
        return value.ToString()!;
    }
}