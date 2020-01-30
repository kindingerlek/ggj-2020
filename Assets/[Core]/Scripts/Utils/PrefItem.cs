using System;
using UnityEngine;
using UnityEngine.Events;

public class PrefItem<T>
{

    private T _value;
    private bool loaded;

    public string Key { get; private set; }
    public T DefaultValue { get; private set; }
    public UnityAction<T> OnValueChanged { get; set; }

    public T Value
    {
        get { return loaded ? _value : (_value = (T)LoadValue(Key, DefaultValue)); }
        set
        {
            SetValue(Key, _value = value);

            try { OnValueChanged(_value); }
            catch (Exception e) { Debug.LogException(e); }
        }
    }

    public PrefItem(string key)
    {
        Key = key;
        OnValueChanged = value => { };
    }

    public PrefItem(string key, T defaultValue)
    {
        Key = key;
        DefaultValue = defaultValue;
        OnValueChanged = value => { };
    }

    private void SetValue(string key, object value)
    {
        if (value is int || value is Enum)
            PlayerPrefs.SetInt(key, (int)value);
        else if (value is float)
            PlayerPrefs.SetFloat(key, (float)value);
        else if (value is bool)
            PlayerPrefs.SetInt(key, (bool)value ? 1 : 0);
        else if (value is string || value is DateTime)
            PlayerPrefs.SetString(key, value.ToString());
        else
            PlayerPrefs.SetString(key, JsonUtility.ToJson(value));
    }

    private object LoadValue(string key, object defaultValue)
    {
        try
        {
            if (loaded)
                return _value;

            var typeOfT = typeof(T);

            if (defaultValue == null)
            {
                if (typeOfT == typeof(int) || typeOfT.IsEnum)
                    defaultValue = 0;
                else if (typeOfT == typeof(float))
                    defaultValue = 0f;
                else if (typeOfT == typeof(bool))
                    defaultValue = false;
                else if (typeOfT == typeof(string))
                    defaultValue = string.Empty;
            }

            if (typeOfT == typeof(int) || typeOfT.IsEnum)
                return PlayerPrefs.GetInt(key, (int)defaultValue);
            else if (typeOfT == typeof(float))
                return PlayerPrefs.GetFloat(key, (float)defaultValue);
            else if (typeOfT == typeof(bool))
                return PlayerPrefs.GetInt(key, (bool)defaultValue ? 1 : 0) != 0;
            else if (typeOfT == typeof(string))
                return PlayerPrefs.GetString(key, (string)defaultValue);
            else if (typeOfT == typeof(DateTime))
                return DateTime.Parse(PlayerPrefs.GetString(key, defaultValue.ToString()));
            else
                return JsonUtility.FromJson<T>(PlayerPrefs.GetString(key, JsonUtility.ToJson(defaultValue)));
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed to retrieve saved value");
            Debug.LogException(e);
            return DefaultValue;
        }
        finally
        {
            loaded = true;

            //try { OnValueChanged(_value); }
            //catch(Exception e) { Debug.LogException(e); }
        }
    }

    public static implicit operator T(PrefItem<T> pref)
    {
        return pref.Value;
    }

    public override string ToString()
    {
        return Value.ToString();
    }

}