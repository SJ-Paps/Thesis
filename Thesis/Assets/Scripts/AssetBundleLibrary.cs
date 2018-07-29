using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System;

public struct FileName
{
    public readonly string nameWithoutExtension;
    public readonly string extension;
    public readonly string nameWithExtension;

    public FileName(string _nameWithExtension)
    {
        nameWithExtension = _nameWithExtension;

        string[] parts = nameWithExtension.Split('.');

        nameWithoutExtension = parts[0];

        if(parts.Length > 1)
        {
            extension = parts[1];
        }
        else
        {
            extension = string.Empty;
        }
    }
}

public class AssetBundleLibrary
{
    #region SINGLETON_PATTERN
    private static AssetBundleLibrary instance;

    public static AssetBundleLibrary Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new AssetBundleLibrary();
            }

            return instance;
        }
    }
    #endregion

    //creo un diccionario que utiliza como key el nombre del archivo del assetbundle
    private Dictionary<FileName, AssetBundle> assetBundles;

    private AssetBundleLibrary()
    {
        assetBundles = new Dictionary<FileName, AssetBundle>();

        LoadBundles();
    }

    private void LoadBundles()
    {
        //obtengo todos los archivos de un directorio
        string[] files = Directory.GetFiles(Reg.assetBundleDataPath);

        foreach(string file in files)
        {
            //me fijo si son assetbundles, si lo son los agrego al diccionario
            AssetBundle bundle = AssetBundle.LoadFromFile(file);

            if(bundle != null)
            {
                assetBundles.Add(new FileName(Path.GetFileName(file)), bundle);
            }
        }
    }

    public AssetBundle GetAssetBundleByNameWithoutExtension(string name)
    {
        foreach(KeyValuePair<FileName, AssetBundle> bundle in assetBundles)
        {
            if(bundle.Key.nameWithoutExtension == name)
            {
                return bundle.Value;
            }
        }

        return null;
    }

    public AssetBundle[] GetAssetBundlesByNameWithoutExtension(string name)
    {
        List<AssetBundle> list = new List<AssetBundle>();

        foreach (KeyValuePair<FileName, AssetBundle> bundle in assetBundles)
        {
            if (bundle.Key.nameWithoutExtension == name)
            {
                list.Add(bundle.Value);
            }
        }

        return list.ToArray();
    }

    public AssetBundle GetAssetBundleByFileExtension(string extension)
    {
        foreach (KeyValuePair<FileName, AssetBundle> bundle in assetBundles)
        {
            if (bundle.Key.extension == extension)
            {
                return bundle.Value;
            }
        }

        return null;
    }

    public AssetBundle[] GetAssetBundlesByFileExtension(string extension)
    {
        List<AssetBundle> list = new List<AssetBundle>();

        foreach (KeyValuePair<FileName, AssetBundle> bundle in assetBundles)
        {
            if (bundle.Key.extension == extension)
            {
                list.Add(bundle.Value);
            }
        }

        return list.ToArray();
    }

    public AssetBundle GetAssetBundleByNameWithExtension(string name)
    {
        foreach (KeyValuePair<FileName, AssetBundle> bundle in assetBundles)
        {
            if (bundle.Key.nameWithExtension == name)
            {
                return bundle.Value;
            }
        }

        return null;
    }

    public AssetBundle[] GetAssetBundlesByNameWithExtension(string name)
    {
        List<AssetBundle> list = new List<AssetBundle>();

        foreach (KeyValuePair<FileName, AssetBundle> bundle in assetBundles)
        {
            if (bundle.Key.nameWithExtension == name)
            {
                list.Add(bundle.Value);
            }
        }

        return list.ToArray();
    }

    public bool ExistsNameWithExtension(string name)
    {
        foreach (KeyValuePair<FileName, AssetBundle> bundle in assetBundles)
        {
            if (bundle.Key.nameWithExtension == name)
            {
                return true;
            }
        }

        return false;
    }

    public bool ExistsNameWithoutExtension(string name)
    {
        foreach (KeyValuePair<FileName, AssetBundle> bundle in assetBundles)
        {
            if (bundle.Key.nameWithoutExtension == name)
            {
                return true;
            }
        }

        return false;
    }

    public bool ExistsExtension(string extension)
    {
        foreach (KeyValuePair<FileName, AssetBundle> bundle in assetBundles)
        {
            if (bundle.Key.extension == extension)
            {
                return true;
            }
        }

        return false;
    }
}
