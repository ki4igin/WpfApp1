using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows.Input;
using System.Windows.Threading;

namespace WpfApp1.ViewModels;

public class MainViewModel : MyClass<MainViewModel>
{
    public string Name { get; set; }
    public int Age { get; set; }
    public int Time { get; set; }

    public MainViewModel()
    {
        Name = "Artem";
        Age = 30;

        AddRule(x => x.Age is > 10 and < 100, nameof(Age), "Errrrrooooorrr");
        AddRule(x => x.Time % 10 is > 2 and < 8, nameof(Time), "Errrrroroooorrr");
    

    // ErrorsChanged += (_, arg) =>
    // {
    //     if (arg.PropertyName is nameof(Time))
    //        
    // };

    // Rules.Add(new DelegateRule<MainViewModel>(
    //     nameof(Name),
    //     "Name cannot be empty.",
    //     x => !string.IsNullOrEmpty(x.Name)));
    //
    // // Rules.Add(new DelegateRule<MainViewModel>(
    // //     nameof(Age),
    // //     "Name cannot be empty.",
    // //     x => x.Age is > 10 and < 100));
    //
    // Rules.Add(
    //     nameof(Age),
    //     "Name cannot be empty.",
    //     x => x.Age is > 10 and < 100);

    var timer = new Timer(1000);
    timer.Elapsed += (_, _) => Time++;
    timer.Start();

    var timer1 = new DispatcherTimer(
        TimeSpan.FromMilliseconds(1000),
        DispatcherPriority.Normal,
        (sender, args) => CommandManager.InvalidateRequerySuggested(),
        Dispatcher.CurrentDispatcher);
}

private SimpleCommand? _testCommand2;
public SimpleCommand Click => _testCommand2 ??= new SimpleCommand(
    execute: () => Age++,
    canExecute: () => !HasErrors
);

}