// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumToBooleanConverter.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

#nullable enable

namespace RayTracerChallenge.App.Wpf.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    /// Converts an enum value to a boolean if the parameter matches the current value.
    /// </summary>
    public class EnumToBooleanConverter : IValueConverter
    {
        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public Type? EnumType { get; set; }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (EnumType == null)
            {
                throw new InvalidOperationException($"{nameof(EnumType)} has not been set.");
            }

            if (!(parameter is string enumString))
            {
                throw new ArgumentException("Parameter must be an enum value string");
            }

            object enumValue = Enum.Parse(EnumType, enumString);
            return enumValue.Equals(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
