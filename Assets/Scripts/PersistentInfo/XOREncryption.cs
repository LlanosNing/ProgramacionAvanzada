using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

public static class XOREncryption
{
    const char KEY = 'H';

    //Metodo por defecto, usando la key declarada en la constante
    //Tambien puede ponerse directamente el char hardcodeado en lugar de hacer una const
    public static string EncryptDecrypt(string input)
    {
        char[] inputArray = input.ToCharArray();
        for (int i = 0; i < inputArray.Length; i++)
        {
            inputArray[i] ^= KEY;
        }
        return new string(inputArray);
    }
    
    //Overload por si se quiere utilizar una clave en especifico para la encriptacion
    public static string EncryptDecrypt(string input, char key)
    {
        char[] inputArray = input.ToCharArray();
        for (int i = 0; i < inputArray.Length; i++)
        {
            inputArray[i] ^= key;
        }
        return new string(inputArray);
    }
}
