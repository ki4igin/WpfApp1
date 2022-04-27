using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace WpfApp1.ViewModels;

public class MyClass<T> : ViewModelBase, INotifyDataErrorInfo where T : MyClass<T>
{
    private readonly List<Rule> _rules = new();

    protected void AddRule(string propertyName, Func<T, bool> validator, string errorMessage) =>
        _rules.Add(new Rule(propertyName, validator, errorMessage));

    private record struct Rule(string PropertyName, Func<T, bool> Validator, string ErrorMessage);

    public IEnumerable GetErrors(string? propertyName) =>
        _rules
            .Where(x => x.PropertyName == propertyName)
            .Where(x => x.Validator((T) this) is false)
            .Select(x => x.ErrorMessage);

    public bool HasErrors => _rules.Any(x => x.Validator((T) this) is false);
    
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        base.OnPropertyChanged(propertyName);
        OnErrorsChangedChanged(propertyName);
        base.OnPropertyChanged(nameof(HasErrors));
    }
    
    protected virtual void OnErrorsChangedChanged([CallerMemberName] string propertyName = "")
    {
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }
    
}