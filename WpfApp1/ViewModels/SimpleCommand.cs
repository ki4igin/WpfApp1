using System;
using System.ComponentModel;

namespace WpfApp1.ViewModels;

public class SimpleCommand<T> : CommandBase
{
    private readonly Action<T> _execute;
    private readonly Func<T, bool> _canExecute;

    public SimpleCommand(Action execute) :
        this(execute, () => true)
    {
    }

    public SimpleCommand(Action<T> execute) :
        this(execute, () => true)
    {
    }

    public SimpleCommand(Action execute, Func<bool> canExecute) :
        this(_ => execute(), _ => canExecute())
    {
    }

    public SimpleCommand(Action<T> execute, Func<bool> canExecute) :
        this(execute, _ => canExecute())
    {
    }

    public SimpleCommand(Action execute, Func<T, bool> canExecute) :
        this(_ => execute(), canExecute)
    {
    }

    public SimpleCommand(Action<T> execute, Func<T, bool> canExecute)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    protected override bool CanExecute(object? parameter) =>
        _canExecute(ConvertParameter(parameter));

    protected override void Execute(object? parameter)
    {
        T param = ConvertParameter(parameter);
        if (CanExecute(param) is false)
            return;

        _execute(param);
    }

    private static T ConvertParameter(object? parameter)
    {
        switch (parameter)
        {
            case null:
                throw new ArgumentNullException($"Сommand has no parameters");
            case T result:
                return result;
        }

        Type commandType = typeof(T);
        Type parameterType = parameter.GetType();

        if (commandType.IsAssignableFrom(parameterType))
            return (T) parameter;

        TypeConverter commandTypeConverter = TypeDescriptor.GetConverter(commandType);
        if (commandTypeConverter.CanConvertFrom(parameterType))
            return (T) commandTypeConverter.ConvertFrom(parameter)!;

        TypeConverter? parameterConverter = TypeDescriptor.GetConverter(parameterType);
        if (parameterConverter.CanConvertTo(commandType))
            return (T) parameterConverter.ConvertFrom(parameter)!;

        return default!;
    }
}

public class SimpleCommand : CommandBase
{
    private readonly Action<object?> _execute;
    private readonly Func<object?, bool> _canExecute;

    public SimpleCommand(Action execute) :
        this(execute, () => true)
    {
    }

    public SimpleCommand(Action<object?> execute) :
        this(execute, () => true)
    {
    }

    public SimpleCommand(Action execute, Func<bool> canExecute) :
        this(_ => execute(), _ => canExecute())
    {
    }

    public SimpleCommand(Action<object?> execute, Func<bool> canExecute) :
        this(execute, _ => canExecute())
    {
    }

    public SimpleCommand(Action execute, Func<object?, bool> canExecute) :
        this(_ => execute(), canExecute)
    {
    }

    public SimpleCommand(Action<object?> execute, Func<object?, bool> canExecute)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    protected override bool CanExecute(object? parameter) =>
        _canExecute(parameter);

    protected override void Execute(object? parameter)
    {
        if (CanExecute(parameter) is false)
            return;

        _execute(parameter);
    }
}