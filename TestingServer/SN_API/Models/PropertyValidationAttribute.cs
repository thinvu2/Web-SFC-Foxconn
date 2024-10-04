using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace SN_API.Models
{
    public abstract class PropertyValidationAttribute : Attribute
    {
        public abstract void validate(object obj, string propertyName);

        public object getObjectValue(object obj, string propertyName)
        {
            PropertyDescriptor pd = TypeDescriptor.GetProperties(obj)[propertyName];
            return pd.GetValue(obj);
        }

        public void setObjectValue(object obj, string propertyName, object value)
        {
            PropertyDescriptor pd = TypeDescriptor.GetProperties(obj)[propertyName];
            pd.SetValue(obj, value);
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class LengthAttribute : PropertyValidationAttribute
    {
        private int minLength;
        private int maxLength;
        public LengthAttribute(int minLength, int maxLength)
        {
            this.minLength = minLength;
            this.maxLength = maxLength;
        }

        public override void validate(object obj, string propertyName)
        {
            int length = 0;
            object value = getObjectValue(obj, propertyName);
            if (value != null)
            {
                length = value.ToString().Length;
            }
            if (length < minLength || length > maxLength)
            {
                throw new ArgumentOutOfRangeException(propertyName, string.Format("Data length must in range of {0} and {1}", minLength, maxLength));
            }
        }
    }
    [AttributeUsage(AttributeTargets.Property)]
    public class DefaultValueAttribute : PropertyValidationAttribute
    {
        private object defaultValue;

        public DefaultValueAttribute(object defaultValue)
        {
            this.defaultValue = defaultValue;
        }
        public override void validate(object obj, string propertyName)
        {
            object value = getObjectValue(obj, propertyName);
            if (value == null)
            {
                setObjectValue(obj, propertyName, defaultValue);
            }
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class RangeAttribute : PropertyValidationAttribute
    {
        private int min;
        private int max;

        public RangeAttribute(int min, int max)
        {
            this.min = min;
            this.max = max;
        }

        public override void validate(object obj, string propertyName)
        {
            object value = getObjectValue(obj, propertyName);
            int intValue = (int)value;
            if (intValue < min || intValue > max)
            {
                throw new ArgumentOutOfRangeException(propertyName, string.Format("min={0},max={1}", min, max));
            }
        }
    }
}