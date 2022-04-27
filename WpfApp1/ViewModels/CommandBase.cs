using System;
using System.Windows.Input;

namespace WpfApp1.ViewModels;

public abstract class CommandBase : ICommand, IDisposable
{
    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    protected virtual bool CanExecute(object? parameter) => true;

    protected abstract void Execute(object? parameter);

    #region ICommand implementations

    bool ICommand.CanExecute(object? parameter) =>
        CanExecute(parameter);

    void ICommand.Execute(object? parameter) =>
        Execute(parameter);

    #endregion


    #region IDisposable implementations

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private bool _disposed;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing || _disposed)
            return;
        _disposed = true;
    }

    #endregion
}