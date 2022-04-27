// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.Collections.ObjectModel;
// using System.ComponentModel;
// using System.Diagnostics;
// using System.Linq;
// using System.Reflection;
// using System.Runtime.CompilerServices;
// using System.Windows.Input;
//
// namespace WpfApp1.ViewModels;
//
// public class ViewModelDataErrorInfo<T> : ViewModelBase, INotifyDataErrorInfo where T : ViewModelDataErrorInfo<T>
// {
//     private readonly Dictionary<string, List<string>> _errors = new();
//
//     public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
//
//
//     // public IObservable<string> WhenErrorsChanged
//     // {
//     //     get
//     //     {
//     //         return Observable
//     //             .FromEventPattern<DataErrorsChangedEventArgs>(
//     //                 h => this.errorsChanged += h,
//     //                 h => this.errorsChanged -= h)
//     //             .Select(x => x.EventArgs.PropertyName);
//     //     }
//     // }
//
//     public virtual bool HasErrors => _errors.Count > 0;
//
//     protected static RuleCollection<T> Rules { get; } = new();
//
//
//     public IEnumerable GetErrors(string? propertyName)
//     {
//         Debug.Assert(
//             string.IsNullOrEmpty(propertyName) ||
//             GetType().GetRuntimeProperty(propertyName) != null,
//             "Check that the property name exists for this instance.");
//
//         IEnumerable result = _errors.ContainsKey(propertyName!) ? _errors[propertyName!] : new List<string>();
//         return result;
//     }
//
//     protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
//     {
//         base.OnPropertyChanged(propertyName);
//         ApplyRules(propertyName);
//         base.OnPropertyChanged(nameof(HasErrors));
//     }
//
//     private void OnErrorsChanged([CallerMemberName] string propertyName = "")
//     {
//         Debug.Assert(
//             string.IsNullOrEmpty(propertyName) ||
//             GetType().GetRuntimeProperty(propertyName) != null,
//             "Check that the property name exists for this instance.");
//
//         ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
//     }
//
//     private void ApplyRules(string propertyName)
//     {
//         var propertyErrors = Rules.Apply((T) this, propertyName).ToList();
//
//         if (propertyErrors.Count > 0)
//         {
//             if (_errors.ContainsKey(propertyName))
//             {
//                 _errors[propertyName].Clear();
//             }
//             else
//             {
//                 _errors[propertyName] = new List<string>();
//             }
//
//             _errors[propertyName].AddRange(propertyErrors);
//             OnErrorsChanged(propertyName);
//         }
//         else if (_errors.ContainsKey(propertyName))
//         {
//             _errors.Remove(propertyName);
//             OnErrorsChanged(propertyName);
//         }
//     }
//
//     // private void InitializeErrors()
//     // {
//     //     if (_errors == null)
//     //     {
//     //         _errors = new Dictionary<string, List<object>>();
//     //
//     //         ApplyRules();
//     //     }
//     // }
// }
//
// public abstract class Rule<T>
// {
//     public string PropertyName { get; }
//     public string Error { get; }
//
//     protected Rule(string propertyName, string error)
//     {
//         PropertyName = propertyName;
//         Error = error;
//     }
//
//     public abstract bool Apply(T obj);
// }
//
// public sealed class DelegateRule<T> : Rule<T>
// {
//     private readonly Func<T, bool> _rule;
//
//     public DelegateRule(string propertyName, string error, Func<T, bool> rule)
//         : base(propertyName, error) =>
//         _rule = rule;
//
//     public override bool Apply(T obj) => _rule(obj);
// }
//
// public sealed class RuleCollection<T> : Collection<Rule<T>>
// {
//     public void Add(string propertyName, string error, Func<T, bool> rule)
//     {
//         Add(new DelegateRule<T>(propertyName, error, rule));
//         
//     }
//
//     public IEnumerable<string> Apply(T obj, string propertyName)
//     {
//         return this
//             .Where(rule => rule.PropertyName.Equals(propertyName))
//             .Where(rule => rule.Apply(obj) is false)
//             .Select(rule => rule.Error)
//             .ToList();
//     }
// }