// Ultimate WPF Event Method Binding implementation by Mike Marynowski
// View the article here: http://www.singulink.com/CodeIndex/post/building-the-ultimate-wpf-event-method-binding-extension

// Licensed under the Code Project Open License: http://www.codeproject.com/info/cpol10.aspx

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Singulink.Windows.Data
{
    public class MethodBindingExtension : MarkupExtension
    {
        private static readonly List<DependencyProperty> StorageProperties = new List<DependencyProperty>();

        public string MethodName { get; }
        public PropertyPath MethodTargetPath { get; }

        private readonly object[] _methodArguments;
        private DependencyProperty _methodTargetProperty;
        private readonly List<DependencyProperty> _argumentProperties = new List<DependencyProperty>();

        public MethodBindingExtension()
        {
        }
        
        public MethodBindingExtension(string path) : this(path, null) { }
        public MethodBindingExtension(string path, object argument) : this(path, new object[] { argument }) { }
        public MethodBindingExtension(string path, object arg0, object arg1) : this(path, new object[] { arg0, arg1 }) { }
        public MethodBindingExtension(string path, object arg0, object arg1, object arg2) : this(path, new object[] { arg0, arg1, arg2 }) { }
        public MethodBindingExtension(string path, object arg0, object arg1, object arg2, object arg3) : this(path, new object[] { arg0, arg1, arg2, arg3 }) { }
        public MethodBindingExtension(string path, object arg0, object arg1, object arg2, object arg3, object arg4) : this(path, new object[] { arg0, arg1, arg2, arg3, arg4 }) { }

        public MethodBindingExtension(string path, object[] arguments)
        {
            if (path == null)
                throw new ArgumentNullException("path");
            
            _methodArguments = arguments ?? new object[0];
            
            int pathSeparatorIndex = path.LastIndexOf('.');
                        
            if (pathSeparatorIndex != -1)
            {
                MethodTargetPath = new PropertyPath(path.Substring(0, pathSeparatorIndex), null);
            }
            
            MethodName = path.Substring(pathSeparatorIndex + 1);
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var provideValueTarget = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            var target = provideValueTarget.TargetObject as FrameworkElement;
            var eventInfo = provideValueTarget.TargetProperty as EventInfo;

            if (target == null || eventInfo == null)
            {
                return this;
            }

            _methodTargetProperty = GetUnusedStorageProperty(target);

            var methodTargetBinding = new Binding();
            methodTargetBinding.Path = MethodTargetPath;
            target.SetBinding(_methodTargetProperty, methodTargetBinding);

            foreach (var argument in _methodArguments)
            {
                var argumentProperty = GetUnusedStorageProperty(target);
                var markupExtension = argument as MarkupExtension;

                if (markupExtension != null)
                {
                    var value = markupExtension.ProvideValue(new ServiceProvider(target, argumentProperty));
                    target.SetValue(argumentProperty, value);
                }
                else
                {
                    target.SetValue(argumentProperty, argument);
                }

                _argumentProperties.Add(argumentProperty);
            }

            return CreateEventHandler(target, eventInfo);
        }

        private Delegate CreateEventHandler(FrameworkElement target, EventInfo eventInfo)
        {
            EventHandler handler = (sender, eventArgs) =>
            {
                var methodTarget = target.GetValue(_methodTargetProperty);

                if (methodTarget == null)
                {
                    Debug.WriteLine("[MethodBinding] Target could not be resolved.");
                    return;
                }

                var arguments = new object[_argumentProperties.Count];

                for (int i = 0; i < _argumentProperties.Count; i++)
                {
                    var argValue = target.GetValue(_argumentProperties[i]);

                    if (argValue is EventSenderExtension)
                    {
                        argValue = sender;
                    }
                    else if (argValue is EventArgsExtension)
                    {
                        argValue = eventArgs;
                    }

                    arguments[i] = argValue;
                }

                var methodTargetType = methodTarget.GetType();

                // Try invoking the method by resolving it based on the arguments provided

                try
                {
                    methodTargetType.InvokeMember(MethodName, BindingFlags.InvokeMethod, null, methodTarget, arguments);
                    return;
                }
                catch (Exception ex) { }
                //catch (MissingMethodException) { }
                // Couldn't match a method with the raw arguments, so check if we can find a method with the same name
                // and parameter count and try to convert any XAML string arguments to match the method parameter types

                var method = methodTargetType.GetMethods().SingleOrDefault(m => m.Name == MethodName && m.GetParameters().Length == arguments.Length);
                
                if (method != null)
                {
                    var parameters = method.GetParameters();

                    for (int i = 0; i < _methodArguments.Length; i++)
                    {
                        if (arguments[i] == null)
                        {
                            if (parameters[i].ParameterType.IsValueType)
                                method = null;
                                break;
                        }
                        else if (_methodArguments[i] is string && parameters[i].ParameterType != typeof(string))
                        {
                            // The original value provided for this argument was a XAML string so try to convert it
                            
                            arguments[i] = TypeDescriptor.GetConverter(parameters[i].ParameterType).ConvertFromString((string)_methodArguments[i]);
                        }
                        else if (!parameters[i].ParameterType.IsInstanceOfType(arguments[i]))
                        {
                            method = null;
                            break;
                        }
                    }

                    method?.Invoke(methodTarget, arguments);
                }
                
                
                Debug.WriteLine($"[MethodBinding] Could not find a method '{MethodName}' on target type '{methodTarget.GetType()}' that accepts the parameters provided.");
            };

            return Delegate.CreateDelegate(eventInfo.EventHandlerType, handler.Target, handler.Method);
        }

        private DependencyProperty GetUnusedStorageProperty(DependencyObject obj)
        {
            foreach (var property in StorageProperties)
            {
                if (obj.ReadLocalValue(property) == DependencyProperty.UnsetValue)
                    return property;
            }

            var newProperty = DependencyProperty.RegisterAttached("Storage" + StorageProperties.Count, typeof(object), typeof(MethodBindingExtension), new PropertyMetadata());
            StorageProperties.Add(newProperty);

            return newProperty;
        }

        private class ServiceProvider : IServiceProvider, IProvideValueTarget
        {
            public object TargetObject { get; set; }
            public object TargetProperty { get; set; }

            public ServiceProvider(object targetObject, object targetProperty)
            {
                TargetObject = targetObject;
                TargetProperty = targetProperty;
            }

            public object GetService(Type serviceType)
            {
                if (serviceType.IsInstanceOfType(this))
                    return this;

                return null;
            }
        }
    }
    
    public class EventSenderExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
    
    public class EventArgsExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
