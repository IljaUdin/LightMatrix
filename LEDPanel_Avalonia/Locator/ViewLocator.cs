using Avalonia.Controls;
using Avalonia.Controls.Templates;
using ReactiveUI;
using System;

namespace LEDPanel_Avalonia.Locator
{
    internal class ViewLocator : IDataTemplate, IViewLocator
    {
        public Control Build(object? data)
        {
            var name = data?.GetType().FullName!.Replace("ViewModel", "View");
            var type = Type.GetType(name);

            if (type != null)
            {
                // Пример создания с использованием конструктора с параметрами
                var constructor = type.GetConstructor(new[] { typeof(string) });
                if (constructor != null)
                {
                    return (Control)constructor.Invoke(new object[] { "parameterValue" });
                }
                else
                {
                    return (Control)Activator.CreateInstance(type)!;
                }
            }
            else
            {
                return new TextBlock { Text = "Not Found: " + name };
            }
        }

        public bool Match(object? data)
        {
            return data is IReactiveObject;
        }

        public IViewFor? ResolveView<T>(T? viewModel, string? contract = null)
        {
            var name = viewModel?.GetType().FullName!.Replace("ViewModel", "View");
            var type = Type.GetType(name);

            if (type != null)
            {
                return Activator.CreateInstance(type) as IViewFor;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(viewModel));
            }
        }
    }
}
