using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace WpfApp1.ViewModels;

public class MyClass<T> : ViewModelBase, INotifyDataErrorInfo where T : MyClass<T>
{
    private readonly List<Rule> _rules = new();
    private readonly List<string> _validateProps = new();

    protected void AddRule(Func<T, bool> validator, string propertyName, string errorMessage)
    {
        _rules.Add(new Rule(validator, propertyName, errorMessage));
        if (_validateProps.Contains(propertyName) is false)
            _validateProps.Add(propertyName);
    }

    private record struct Rule(Func<T, bool> Validator, string PropertyName, string ErrorMessage);

    public IEnumerable GetErrors(string? propertyName)
    {
        switch (propertyName)
        {
            case not null:
                Debug.WriteLine(propertyName);
                return _rules.Where(x => x.PropertyName == propertyName)
                    .Where(x => x.Validator((T)this) is false)
                    .Select(x => x.ErrorMessage);
            case null:
                return _rules.Where(x => x.Validator((T)this) is false).Select(x => x.ErrorMessage);
        }
    }

    public bool HasErrors => _rules.Any(x => x.Validator((T)this) is false);

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        base.OnPropertyChanged(propertyName);
        if (_validateProps.Contains(propertyName))
        {
            Debug.WriteLine($"current prop {propertyName}");
            //OnErrorsChangedChanged(propertyName);
            //base.OnPropertyChanged(nameof(HasErrors));
        }
    }

    protected virtual void OnErrorsChangedChanged([CallerMemberName] string propertyName = "")
    {
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }
}